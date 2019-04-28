using OSPABA;
using simulation;
using managers;
using continualAssistants;

namespace agents
{
	//meta! id="1"
	public class AgentModelu : Agent
	{
		public AgentModelu(int id, OSPABA.Simulation mySim, Agent parent) :
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
			new ManagerModelu(SimId.ManagerModelu, MySim, this);
			AddOwnMessage(Mc.PrichodCestujuceho);
			AddOwnMessage(Mc.PrepravaCestujucich);
		}
		//meta! tag="end"
	}
}
