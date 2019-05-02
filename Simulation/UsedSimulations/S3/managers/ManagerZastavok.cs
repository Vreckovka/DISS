using OSPABA;
using simulation;
using agents;
using continualAssistants;

namespace managers
{
    //meta! id="28"
    public class ManagerZastavok : Manager
    {
        public ManagerZastavok(int id, OSPABA.Simulation mySim, Agent myAgent) :
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

        //meta! sender="AgentLiniek", id="63", type="Notice"
        public void ProcessPrichodNaZastavku(MessageForm message)
        {
            for (int i = 0; i < ((MyMessage)message).Autobus.PocetDveri; i++)
            {
                MyMessage sprava = new MyMessage(MySim);
                sprava.Addressee = MyAgent.FindAssistant(SimId.NastupovanieProces);
                sprava.Autobus = ((MyMessage)message).Autobus;
                StartContinualAssistant(sprava);
            }
        }


        //meta! sender="PrichodyCestujucichNaZastavkuProces", id="69", type="Finish"
        public void ProcessFinish(MessageForm message)
        {
            
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
                    ProcessFinish(message);
                    break;

                case Mc.PrichodNaZastavku:
                    ProcessPrichodNaZastavku(message);
                    break;

                default:
                    ProcessDefault(message);
                    break;
            }
        }
        //meta! tag="end"
        public new AgentZastavok MyAgent
        {
            get
            {
                return (AgentZastavok)base.MyAgent;
            }
        }
    }
}
