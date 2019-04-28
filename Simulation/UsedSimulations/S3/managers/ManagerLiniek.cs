using OSPABA;
using simulation;
using agents;
using continualAssistants;

namespace managers
{
	//meta! id="25"
	public class ManagerLiniek : Manager
	{
		public ManagerLiniek(int id, OSPABA.Simulation mySim, Agent myAgent) :
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

		//meta! sender="AgentAutobusov", id="40", type="Response"
		public void ProcessJazda(MessageForm message)
        {
            ;
        }

		//meta! sender="AgentModelu", id="36", type="Request"
		public void ProcessPrepravaCestujucich(MessageForm message)
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
			case Mc.PrepravaCestujucich:
				ProcessPrepravaCestujucich(message);
			break;

			case Mc.InitJazda:
				ProcessJazda(message);
			break;

			default:
				ProcessDefault(message);
			break;
			}
		}
		//meta! tag="end"
		public new AgentLiniek MyAgent
		{
			get
			{
				return (AgentLiniek)base.MyAgent;
			}
		}
	}
}
