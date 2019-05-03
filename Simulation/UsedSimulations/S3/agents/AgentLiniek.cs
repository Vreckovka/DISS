using OSPABA;
using simulation;
using managers;
using continualAssistants;

namespace agents
{
	//meta! id="25"
	public class AgentLiniek : Agent
	{
        
		public AgentLiniek(int id, OSPABA.Simulation mySim, Agent parent) :
			base(id, mySim, parent)
		{
			Init();
		}

		override public void PrepareReplication()
		{
			base.PrepareReplication();
			// Setup component for the next replication
		}

		//meta! userInfo="Generated code: do not modify", tag="begin"
		private void Init()
		{
			new ManagerLiniek(SimId.ManagerLiniek, MySim, this);
			AddOwnMessage(Mc.InitJazda);
			AddOwnMessage(Mc.PrepravaCestujucich);
		}
		//meta! tag="end"
	}
}
