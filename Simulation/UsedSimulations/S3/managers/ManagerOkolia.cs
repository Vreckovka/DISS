using System;
using System.Threading;
using OSPABA;
using simulation;
using agents;
using continualAssistants;

namespace managers
{
    //meta! id="8"
    public class ManagerOkolia : Manager
    {
        public ManagerOkolia(int id, OSPABA.Simulation mySim, Agent myAgent) :
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

        //meta! sender="AgentModelu", id="41", type="Notice"
        public void ProcessCestujuciDovezeny(MessageForm message)
        {
            ;
        }

        //meta! sender="AgentModelu", id="41", type="Notice"
        public void ProcessCestujuciVygenerovany(MessageForm message)
        {
            // Console.WriteLine($"{TimeSpan.FromMinutes(MySim.CurrentTime)} Agent {((MyMessage)message).Cestujuci.Id} prisiel na zastavku {MyAgent.Zastavky[(int)message.Param].Meno}");
            //Thread.Sleep(10);
        }

        //meta! sender="AgentModelu", id="41", type="Notice"
        public void ProcessZacniGenerovatCestujucich(MessageForm message)
        {
            var sprava = (MyMessage)message.CreateCopy();
            sprava.Addressee = MyAgent.FindAssistant(SimId.PrichodyCestujucichProces);
            sprava.ZastavkaData = ((MyMessage)message).ZastavkaData;
            StartContinualAssistant(sprava);
        }

        //meta! sender="PrichodyCestujucichProces", id="67", type="Finish"
        public void ProcessFinish(MessageForm message)
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
                case Mc.CestujuciDovezeny:
                    ProcessCestujuciDovezeny(message);
                    break;

                case Mc.Finish:
                    ProcessFinish(message);
                    break;

                case Mc.ZacniGenerovatCestujucich:
                    ProcessZacniGenerovatCestujucich(message);
                    break;

                case Mc.PrichodCestujuceho:
                    ProcessCestujuciVygenerovany(message);
                    break;

                default:
                    ProcessDefault(message);
                    break;
            }
        }
        //meta! tag="end"
        public new AgentOkolia MyAgent
        {
            get
            {
                return (AgentOkolia)base.MyAgent;
            }
        }
    }
}
