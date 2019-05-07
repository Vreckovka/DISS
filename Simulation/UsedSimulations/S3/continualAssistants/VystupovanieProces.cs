using System;
using System.Linq;
using System.Threading;
using OSPABA;
using simulation;
using agents;
using Simulations.Distributions;
using Simulations.UsedSimulations.S3;
using Simulations.UsedSimulations.S3.entities;
using System.Linq;

namespace continualAssistants
{
    //meta! id="70"
    public class VystupovanieProces : Process
    {

        public double Cislo { get; set; } = 3.1 / 60;
        private int pocetUkoncenychAutobusov;
        private TriangularDistribution triangularDistribution;
        public VystupovanieProces(int id, OSPABA.Simulation mySim, CommonAgent myAgent) :
            base(id, mySim, myAgent)
        {
            triangularDistribution = new TriangularDistribution(((MySimulation)MySim).Random.Next(), 0.6 / 60, 4.2 / 60, 1.2 / 60);
        }

        override public void PrepareReplication()
        {
            pocetUkoncenychAutobusov = 0;
            base.PrepareReplication();
            // Setup component for the next replication
        }

        //meta! sender="AgentAutobusov", id="71", type="Start"
        public void ProcessStart(MessageForm message)
        {
            var sprava = (MyMessage)message.CreateCopy();
            sprava.Autobus = ((MyMessage)message).Autobus;
            sprava.Code = Mc.CestujuciVystupil;
            sprava.Autobus.StojiNaZastavke = true;

            if (sprava.Autobus.Cestujuci.Count > 0)
            {
                double holdTime = -1;
                switch (sprava.Autobus.Typ)
                {
                    case AutobusyTyp.Autobus:
                        holdTime = triangularDistribution.GetNext();

                        break;
                    case AutobusyTyp.Microbus:
                        holdTime = 4.0 / 60;

                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }


                if (MySim.CurrentTime <= ((MySimulation)MySim).Configration.ZaciatokZapasu)
                    foreach (var cestujuci in sprava.Autobus.Cestujuci)
                    {
                        cestujuci.PrisielNaCas = true;
                    }


                //holdTime = Cislo;
                Hold(holdTime, sprava);
            }
            else if (MySim.CurrentTime >= ((MySimulation)MySim).Configration.ZaciatokZapasu && 
                sprava.Autobus.Cestujuci.Count == 0 && !sprava.Autobus.KoniecJazd)
            {
                sprava.Autobus.KoniecJazd = true;
                AssistantFinished(sprava);
                pocetUkoncenychAutobusov++;

                if (pocetUkoncenychAutobusov == ((MySimulation)MySim).AgentAutobusov.Autobusy.Count)
                {
                    ((MySimulation)MySim).LastFinishTime = MySim.CurrentTime;
                    MySim.StopReplication();
                }
            }
            else if (sprava.Autobus.KoniecJazd)
            {
                AssistantFinished(sprava);
            }
            else
                UkonciVystupovanie(sprava);


        }

        //meta! userInfo="Process messages defined in code", id="0"
        public void ProcessDefault(MessageForm message)
        {
            switch (message.Code)
            {
                case Mc.CestujuciVystupil:

                    var newMessage = new MyMessage(MySim);
                    var sprava = ((MyMessage)message);

                    newMessage.Param = message.Param;
                    newMessage.Autobus = sprava.Autobus;
                    newMessage.Code = sprava.Code;

                    if (!newMessage.Autobus.KoniecProcesu)
                    {
                        newMessage.Autobus.AktualnaZastavka.Zastavka.Cestujuci.Enqueue(newMessage.Autobus.Cestujuci.Dequeue());
                        newMessage.Autobus.AktualnyPocetPrevezenych = newMessage.Autobus.Cestujuci.Count;
                        newMessage.Autobus.AktualnaZastavka.Zastavka.PocetCestujucich =
                            newMessage.Autobus.AktualnaZastavka.Zastavka.Cestujuci.Count;

                        double holdTime = 0;
                        switch (newMessage.Autobus.Typ)
                        {
                            case AutobusyTyp.Autobus:
                                holdTime = triangularDistribution.GetNext();
                                break;
                            case AutobusyTyp.Microbus:
                                holdTime = 4.0 / 60;

                                break;
                            default:
                                throw new ArgumentOutOfRangeException();

                        }

                        // Console.WriteLine(MySim.CurrentTime + " " +  newMessage.Autobus.AktualnaZastavka.Zastavka.Cestujuci.Count);

                        if (newMessage.Autobus.Cestujuci.Count > 0)
                        {
                            //holdTime = Cislo;
                            Hold(holdTime, newMessage);
                        }
                        else
                        {
                            UkonciVystupovanie(newMessage);
                        }
                    }
                    else
                    {
                        UkonciVystupovanie(newMessage);
                    }

                    break;
            }
        }

        public void UkonciVystupovanie(MessageForm message)
        {
            AssistantFinished(message);
            var sprava = ((MyMessage)message);

            if (!sprava.Autobus.NovaJazda)
            {
                var newMessage = new MyMessage(MySim);
                newMessage.Autobus = sprava.Autobus;

                sprava.Autobus.KoniecProcesu = true;
                sprava.Autobus.StojiNaZastavke = false;
                sprava.Autobus.NovaJazda = true;
                sprava.Autobus.Reset();

                newMessage.Code = Mc.KoniecJazdy;
                newMessage.Addressee = MySim.FindAgent(SimId.AgentAutobusov);
                Notice(newMessage);

                //Console.WriteLine(MySim.CurrentTime +  " Koniec vystupovania");
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
