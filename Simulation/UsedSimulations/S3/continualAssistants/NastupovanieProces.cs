using System;
using System.Threading;
using OSPABA;
using simulation;
using agents;
using Simulations.Distributions;

namespace continualAssistants
{
    //meta! id="70"
    public class NastupovanieProces : Process
    {
        private TriangularDistribution triangularDistribution;
        public NastupovanieProces(int id, OSPABA.Simulation mySim, CommonAgent myAgent) :
            base(id, mySim, myAgent)
        {
            triangularDistribution = new TriangularDistribution(((MySimulation)MySim).Random.Next(), 0.6 / 60, 4.2 / 60, 1.2 / 60);
        }

        override public void PrepareReplication()
        {
            base.PrepareReplication();
            // Setup component for the next replication
        }

        //meta! sender="AgentAutobusov", id="71", type="Start"
        public void ProcessStart(MessageForm message)
        {
            if (((MyMessage)message).Autobus.KoniecProcesu)
            {
                AssistantFinished(message);
            }
            else
            {
                message.Code = Mc.CestujuciNastupil;

                ((MyMessage)message).Autobus.StojiNaZastavke = true;


                //((MyMessage)message).Autobus.AktualnaZastavka =
                //    ((MyMessage)message).Autobus.Linka
                //    .Zastavky[((MyMessage)message).Autobus.IndexAktualnaZastavkaVLinke].Key;


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
                    var next = triangularDistribution.GetNext();
                    Hold(next, message);
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

                    if (sprava.Autobus.StojiNaZastavke)
                    {
                        if (sprava.Autobus.IndexAktualnaZastavkaVLinke == sprava.Autobus.Linka.Zastavky.Count)
                        {
                            if (sprava.Autobus.StojiNaZastavke)
                            {
                                newMessage = new MyMessage(MySim);

                                newMessage.Autobus = sprava.Autobus;
                                newMessage.Code = Mc.KoniecJazdy;

                                sprava.Autobus.StojiNaZastavke = false;
                                AssistantFinished(message);

                                Notice(newMessage);
                            }
                        }
                        else
                        {

                            if (sprava.Autobus.KapacitaOsob > sprava.Autobus.Cestujuci.Count)
                            {
                                if (sprava.Autobus.AktualnaZastavka.Zastavka.Cestujuci.Count != 0)
                                {
                                    Hold(triangularDistribution.GetNext(), newMessage);
                                    var cestujuci = sprava.Autobus.AktualnaZastavka.Zastavka.Cestujuci.Dequeue();

                                    sprava.Autobus.Cestujuci.Add(cestujuci);
                                    sprava.Autobus.CelkovyPocetPrevezenych++;
                                    sprava.Autobus.AktualnyPocetPrevezenych++;
                                    sprava.Autobus.AktualnaZastavka.Zastavka.PocetCestujucich--;
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

                if (sprava.Autobus.Linka.Zastavky.Count - 1 != sprava.Autobus.IndexAktualnaZastavkaVLinke)
                    sprava.Autobus.IndexAktualnaZastavkaVLinke++;
                else
                {
                    //TODO: Vystupovanie
                    sprava.Autobus.Reset();
                    sprava.Autobus.IndexAktualnaZastavkaVLinke = 0;
                }

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
