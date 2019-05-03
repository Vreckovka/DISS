using OSPABA;
using simulation;
using agents;
using continualAssistants;

namespace managers
{
    //meta! id="14"
    public class ManagerAutobusov : Manager
    {
        public ManagerAutobusov(int id, OSPABA.Simulation mySim, Agent myAgent) :
            base(id, mySim, myAgent)
        {
            Init();
        }

        override public void PrepareReplication()
        {
            base.PrepareReplication();
            // Setup component for the next replication

            if (PetriNet != null)
            {
                PetriNet.Clear();
            }
        }

        //meta! sender="AgentLiniek", id="40", type="Request"
        public void ProcessInitJazda(MessageForm message)
        {

            message.Code = Mc.JazdaNaZastavku;
            message.Param = message.Param;
            message.Addressee = MySim.FindAgent(SimId.AgentAutobusov);

            Notice(message);
        }

        public void ProcessJazdaNaZastavku(MessageForm message)
        {
            MyMessage sprava = new MyMessage(MySim);
            sprava.Autobus = ((MyMessage)message).Autobus;
            sprava.Autobus.KoniecProcesu = false;

            if (sprava.Autobus.AktualnaZastavka.Konecna)
            {
                var autobus = ((MyMessage)message).Autobus;
                autobus.KoniecProcesu = false;
                autobus.StojiNaZastavke = true;

                for (int i = 0; i < ((MyMessage)message).Autobus.PocetDveri; i++)
                {
                    sprava = (MyMessage)message.CreateCopy();
                    sprava.Autobus = autobus;
                    sprava.Addressee = MyAgent.FindAssistant(SimId.VystupovanieProces);
                    StartContinualAssistant(sprava);
                }
            }
            else
            {
                sprava.Addressee = MyAgent.FindAssistant(SimId.JazdaNaZastavkuProces);
                StartContinualAssistant(sprava);
            }
        }

        public void ProcessKoniecJazdy(MessageForm message)
        {
            var sprava = new MyMessage(MySim);
            sprava.Addressee = MyAgent.FindAssistant(SimId.JazdaNaZastavkuProces);
            sprava.Autobus = ((MyMessage)message).Autobus;

            StartContinualAssistant(sprava);
        }

        //meta! sender="NastupovanieProces", id="71", type="Finish"
        public void ProcessFinishNastupovanieProces(MessageForm message)
        {
            ;
        }

        //meta! sender="InitJazdaProces", id="50", type="Finish"
        public void ProcessFinishJazdaProces(MessageForm message)
        {
            ;
        }

        //meta! userInfo="Process messages defined in code", id="0"
        public void ProcessDefault(MessageForm message)
        {
            switch (message.Code)
            {
            }
        }

        //meta! userInfo="Generated code: do not modify", tag="begin"
        public void Init()
        {
        }

        override public void ProcessMessage(MessageForm message)
        {
            switch (message.Code)
            {
                case Mc.Finish:
                    switch (message.Sender.Id)
                    {
                        case SimId.NastupovanieProces:
                            ProcessFinishNastupovanieProces(message);
                            break;

                        case SimId.InitJazdaProces:
                            ProcessFinishJazdaProces(message);
                            break;
                    }
                    break;

                case Mc.InitJazda:
                    ProcessInitJazda(message);
                    break;

                case Mc.JazdaNaZastavku:
                    ProcessJazdaNaZastavku(message);
                    break;
                case Mc.KoniecJazdy:
                    ProcessKoniecJazdy(message);
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
