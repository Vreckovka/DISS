using System;
using System.Linq;
using System.Threading;
using OSPABA;
using simulation;
using agents;
using Simulations.Distributions;
using Simulations.UsedSimulations.S3.entities;

namespace continualAssistants
{
    //meta! id="70"
    public class NastupovanieProces : Process
    {
        private TriangularDistribution triangularDistribution;
        private UniformContinuousDistribution uniformContinuous;
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
                         == 0 && !((MyMessage)message).Autobus.KoniecProcesu)
                {
                    UkonciNastupovanie(message);
                }
                else
                {
                    double holdTime = 0;
                    bool nastupiNiekto = true;


                    switch (((MyMessage)message).Autobus.Typ)
                    {
                        case AutobusyTyp.Autobus:
                            holdTime = triangularDistribution.GetNext();
                            break;
                        case AutobusyTyp.Microbus:
                            holdTime = uniformContinuous.GetNext();

                            var jeTam = (from x in ((MyMessage)message).Autobus.AktualnaZastavka.Zastavka.Cestujuci
                                         where MySim.CurrentTime - x.CasZacatiaCakania > 6
                                         select x).FirstOrDefault();

                            if (jeTam == null)
                                nastupiNiekto = false;

                            break;
                        default:
                            throw new ArgumentOutOfRangeException();
                    }

                    if (nastupiNiekto)
                        Hold(holdTime, message);
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

                    bool nastupiNiekto = true;

                    if (sprava.Autobus.StojiNaZastavke)
                    {
                        if (sprava.Autobus.KapacitaOsob > sprava.Autobus.Cestujuci.Count)
                        {
                            if (sprava.Autobus.AktualnaZastavka.Zastavka.Cestujuci.Count != 0
                                 && !sprava.Autobus.AktualnaZastavka.Konecna)
                            {
                                double holdTime = 0;

                                switch (((MyMessage)message).Autobus.Typ)
                                {
                                    case AutobusyTyp.Autobus:
                                        holdTime = triangularDistribution.GetNext();
                                        break;
                                    case AutobusyTyp.Microbus:
                                        holdTime = uniformContinuous.GetNext();

                                        if (MySim.CurrentTime - sprava.Autobus.AktualnaZastavka.Zastavka.Cestujuci
                                                .Peek().CasZacatiaCakania < 6)
                                        {
                                            nastupiNiekto = false;
                                        }

                                        break;
                                    default:
                                        throw new ArgumentOutOfRangeException();
                                }

                                if (nastupiNiekto)
                                {

                                    var cestujuci = sprava.Autobus.AktualnaZastavka.Zastavka.Cestujuci.Dequeue();

                                    cestujuci.CasCakania = MySim.CurrentTime - cestujuci.CasZacatiaCakania;

                                    sprava.Autobus.Cestujuci.Enqueue(cestujuci);
                                    sprava.Autobus.CelkovyPocetPrevezenych++;
                                    sprava.Autobus.AktualnyPocetPrevezenych++;
                                    sprava.Autobus.AktualnaZastavka.Zastavka.PocetCestujucich =
                                        sprava.Autobus.AktualnaZastavka.Zastavka.Cestujuci.Count;
                                    
                                    Hold(holdTime, message);
                                }
                                else
                                    UkonciNastupovanie(message);


                                //Console.WriteLine(
                                //    $"{TimeSpan.FromMinutes(MySim.CurrentTime)} CESTUJUCI NASTUPIL {cestujuci.Id} cez dvere {message.Param}");
                            }
                            else
                            {
                                UkonciNastupovanie(message);
                                //Console.WriteLine("Zastavka je prazdna");
                            }
                        }
                        else
                        {
                            UkonciNastupovanie(message);
                            //Console.WriteLine("Autobus je plny");
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

                newMessage.Code = Mc.JazdaNaZastavku;
                newMessage.Addressee = MySim.FindAgent(SimId.AgentAutobusov);
                Notice(newMessage);
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
