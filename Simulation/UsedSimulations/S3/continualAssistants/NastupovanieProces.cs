using System;
using System.Linq;
using System.Threading;
using OSPABA;
using simulation;
using agents;
using Simulations.Distributions;
using Simulations.UsedSimulations.S3;
using Simulations.UsedSimulations.S3.entities;

namespace continualAssistants
{
    //meta! id="70"
    public class NastupovanieProces : Process
    {
        public TriangularDistribution triangularDistribution;
        private UniformContinuousDistribution uniformContinuous;

        public double Cas { get; set; } = 3.1 / 60.0;
        public NastupovanieProces(int id, OSPABA.Simulation mySim, CommonAgent myAgent) :
            base(id, mySim, myAgent)
        {
            triangularDistribution = new TriangularDistribution(((MySimulation)MySim).Random.Next(), 0.6 / 60, 4.2 / 60, 1.2 / 60);
            uniformContinuous = new UniformContinuousDistribution(6.0 / 60, 10.0 / 60, ((MySimulation)MySim).Random.Next());
        }

        override public void PrepareReplication()
        {
            base.PrepareReplication();
            // Setup component for the next replication
        }

        //meta! sender="AgentAutobusov", id="71", type="Start"
        public void ProcessStart(MessageForm message)
        {
            var messagePom = message;
            message = message.CreateCopy();
            ((MyMessage)message).Autobus = ((MyMessage)messagePom).Autobus;


            if (((MyMessage)message).Autobus.KoniecProcesu)
            {
                AssistantFinished(message);
            }
            else
            {
                message.Code = Mc.CestujuciNastupil;

                ((MyMessage)message).Autobus.StojiNaZastavke = true;

                if (((MyMessage)message).Autobus.KapacitaOsob == ((MyMessage)message).Autobus.Cestujuci.Count &&
                    !((MyMessage)message).Autobus.KoniecProcesu)
                {
                    UkonciNastupovanie(message);
                }
                else if (((MyMessage)message).Autobus.AktualnaZastavka.Zastavka.Cestujuci.Count
                         == 0 && !((MyMessage)message).Autobus.KoniecProcesu && ((MyMessage)message).Autobus.PocetDveriObsadene == 0)
                {
                    UkonciNastupovanie(message);
                }
                else
                {
                    double holdTime = -1;
                    bool nastupiNiekto = true;

                    switch (((MyMessage)message).Autobus.Typ)
                    {
                        case AutobusyTyp.Autobus:
                            holdTime = triangularDistribution.GetNext();
                            break;
                        case AutobusyTyp.Microbus:
                            holdTime = uniformContinuous.GetNext();

                            if (MySim.CurrentTime - ((MyMessage)message).Autobus.AktualnaZastavka.Zastavka.Cestujuci.Peek().CasZacatiaCakania < 6)
                            {
                                nastupiNiekto = false;
                                //Console.WriteLine("Nikto nenastupi na zastavke " + ((MyMessage)message).Autobus.AktualnaZastavka.Zastavka);
                            }
                            break;
                        default:
                            throw new ArgumentOutOfRangeException();
                    }

                    //holdTime = Cas;

                    if (nastupiNiekto && !((MyMessage)message).Autobus.AktualnaZastavka.Konecna)
                    {

                        if (((MyMessage)message).Autobus.AktualnaZastavka.Zastavka.Cestujuci.Count > 0)
                        {
                            var cestujuci = ((MyMessage)message).Autobus.AktualnaZastavka.Zastavka.Cestujuci.Dequeue();
                            cestujuci.CasCakania = (MySim.CurrentTime * 60) - (cestujuci.CasZacatiaCakania * 60);
                           // Console.WriteLine((MySim.CurrentTime * 60) + "NASTUPUJE, CAKAL " + cestujuci.CasCakania + " " + ((MyMessage)message).Autobus.AktualnaZastavka.Zastavka);
                            ((MyMessage)message).Autobus.Cestujuci.Enqueue(cestujuci);
                            ((MyMessage)message).Autobus.AktualnaZastavka.Zastavka.PocetCestujucich--;
                            ((MyMessage)message).Autobus.PocetDveriObsadene++;
                            Hold(holdTime, message);
                        }
                    }
                    else
                        UkonciNastupovanie(message);
                }
            }
        }

        //meta! userInfo="Process messages defined in code", id="0"
        public void ProcessDefault(MessageForm message)
        {
            switch (message.Code)
            {
                case Mc.CestujuciNastupil:
                    var newMessage = new MyMessage(MySim);
                    var sprava = ((MyMessage)message);

                    newMessage.Param = message.Param;
                    newMessage.Autobus = sprava.Autobus;
                    newMessage.Code = sprava.Code;


                    ((MyMessage)message).Autobus.PocetDveriObsadene--;
                    //Console.WriteLine((MySim.CurrentTime * 60) + " nastupil " + sprava.Autobus.AktualnaZastavka.Zastavka);


                    if (sprava.Autobus.StojiNaZastavke && !sprava.Autobus.AktualnaZastavka.Konecna)
                    {
                        double holdTime = -1;

                        switch (((MyMessage)message).Autobus.Typ)
                        {
                            case AutobusyTyp.Autobus:
                                holdTime = triangularDistribution.GetNext();
                                break;
                            case AutobusyTyp.Microbus:
                                holdTime = uniformContinuous.GetNext();
                                break;
                            default:
                                throw new ArgumentOutOfRangeException();
                        }



                        if (sprava.Autobus.KapacitaOsob > sprava.Autobus.Cestujuci.Count)
                        {
                            if (sprava.Autobus.AktualnaZastavka.Zastavka.Cestujuci.Count != 0)
                            {
                                if (MySim.CurrentTime - sprava.Autobus.AktualnaZastavka.Zastavka.Cestujuci.Peek().CasZacatiaCakania < 6 && sprava.Autobus.Typ == AutobusyTyp.Microbus)
                                {
                                    UkonciNastupovanie(message);
                                }
                                else
                                {
                                    var cestujuci = sprava.Autobus.AktualnaZastavka.Zastavka.Cestujuci.Dequeue();
                                    cestujuci.CasCakania = (MySim.CurrentTime * 60) - (cestujuci.CasZacatiaCakania * 60);

                                    //Console.WriteLine((MySim.CurrentTime * 60) + "NASTUPUJE, CAKAL " + cestujuci.CasCakania + " " + ((MyMessage)message).Autobus.AktualnaZastavka.Zastavka);

                                    sprava.Autobus.Cestujuci.Enqueue(cestujuci);
                                    sprava.Autobus.CelkovyPocetPrevezenych++;
                                    sprava.Autobus.AktualnyPocetPrevezenych++;
                                    sprava.Autobus.AktualnaZastavka.Zastavka.PocetCestujucich--;
                                    ((MyMessage)message).Autobus.PocetDveriObsadene++;

                                    //holdTime = Cas;
                                    Hold(holdTime, message);
                                }
                            }
                            else if (((MyMessage)message).Autobus.PocetDveriObsadene == 0)
                                if (((MySimulation)MySim).Configration.Cakanie && sprava.Autobus.Typ == AutobusyTyp.Autobus && !sprava.Autobus.CakalNavyse)
                                {
                                    //Console.WriteLine((MySim.CurrentTime * 60) + " CAKANA  1.5 ");
                                    Hold(1.5, message);
                                    sprava.Autobus.CakalNavyse = true;
                                    sprava.Autobus.PocetDveriObsadene++;
                                }
                                else
                                {
                                    UkonciNastupovanie(message);
                                }
                        }
                        else
                        {
                            UkonciNastupovanie(message);
                        }
                    }
                    else
                    {
                        UkonciNastupovanie(message);
                    }

                    break;
            }
        }

        public void UkonciNastupovanie(MessageForm message)
        {
            AssistantFinished(message);
            var sprava = ((MyMessage)message);

            if (sprava.Autobus.StojiNaZastavke)
            {
                var newMessage = new MyMessage(MySim);
                newMessage.Autobus = sprava.Autobus;
                newMessage.Code = sprava.Code;

                sprava.Autobus.KoniecProcesu = true;
                sprava.Autobus.StojiNaZastavke = false;
                sprava.Autobus.CakalNavyse = false;
                sprava.Autobus.AktualnaZastavka.Zastavka.CakajuciAutobus = null;
                newMessage.Code = Mc.JazdaNaZastavku;
                newMessage.Addressee = MySim.FindAgent(SimId.AgentAutobusov);
                Notice(newMessage);

                //Console.WriteLine(MySim.CurrentTime + " Koniec nastupovania");
            }
        }

        //meta! userInfo="Generated code: do not modify", tag="begin"
        override public void ProcessMessage(MessageForm message)
        {
            switch (message.Code)
            {
                case Mc.Start:
                    ProcessStart(message);
                    break;

                default:
                    ProcessDefault(message);
                    break;
            }
        }
        //meta! tag="end"
        public new AgentAutobusov MyAgent
        {
            get
            {
                return (AgentAutobusov)base.MyAgent;
            }
        }
    }
}
