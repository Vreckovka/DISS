using System;
using System.Threading;
using OSPABA;
using simulation;
using agents;
using Simulations.Distributions;

namespace continualAssistants
{
    //meta! id="70"
    public class VystupovanieProces : Process
    {
        private TriangularDistribution triangularDistribution;
        public VystupovanieProces(int id, OSPABA.Simulation mySim, CommonAgent myAgent) :
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
            var sprava = (MyMessage)message.CreateCopy();
            sprava.Autobus = ((MyMessage)message).Autobus;
            sprava.Code = Mc.CestujuciVystupil;
            // sprava.Autobus.StojiNaZastavke = true;

            if (sprava.Autobus.Cestujuci.Count > 0)
                Hold(triangularDistribution.GetNext(), sprava);
            else
                UkonciVystupovanie(sprava);
        }

        //meta! userInfo="Process messages defined in code", id="0"
        public void ProcessDefault(MessageForm message)
        {
            switch (message.Code)
            {
                //TODO: NEJAKY CESTUJUCI SU TAM
                case Mc.CestujuciVystupil:

                    var newMessage = new MyMessage(MySim);
                    var sprava = ((MyMessage)message);

                    newMessage.Param = message.Param;
                    newMessage.Autobus = sprava.Autobus;
                    newMessage.Code = sprava.Code;

                    if (!sprava.Autobus.KoniecProcesu && sprava.Autobus.Cestujuci.Count > 0)
                    {
                        sprava.Autobus.AktualnaZastavka.Zastavka.Cestujuci.Enqueue(sprava.Autobus.Cestujuci.Dequeue());
                        sprava.Autobus.AktualnyPocetPrevezenych--;

                        sprava.Autobus.AktualnaZastavka.Zastavka.PocetCestujucich =
                            sprava.Autobus.AktualnaZastavka.Zastavka.Cestujuci.Count;


                        Hold(triangularDistribution.GetNext(), newMessage);
                    }
                    else
                    {
                        UkonciVystupovanie(message);
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
