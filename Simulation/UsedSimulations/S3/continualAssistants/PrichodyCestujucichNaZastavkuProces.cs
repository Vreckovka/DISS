using System;
using OSPABA;
using simulation;
using agents;
using Simulations.Distributions;

namespace continualAssistants
{
    //meta! id="68"
    public class PrichodyCestujucichNaZastavkuProces : Process
    {
     
        public PrichodyCestujucichNaZastavkuProces(int id, OSPABA.Simulation mySim, CommonAgent myAgent) :
            base(id, mySim, myAgent)
        {
           
        }

        override public void PrepareReplication()
        {
            base.PrepareReplication();
            // Setup component for the next replication
        }

        //meta! sender="AgentZastavok", id="69", type="Start"
        public void ProcessStart(MessageForm message)
        {
            message.Code = Mc.ZacniGenerovatCestujucich;
            Hold(message.Param, message);
        }

        //meta! userInfo="Process messages defined in code", id="0"
        public void ProcessDefault(MessageForm message)
        {
            switch (message.Code)
            {
                case Mc.ZacniGenerovatCestujucich:

                    MyMessage sprava = (MyMessage) message;

                    sprava.Addressee = MyAgent;
                    sprava.Code = Mc.ZacniGenerovatCestujucich;
                    sprava.Param = ((MyMessage) message).IndexZastavky;
                    Notice(sprava);

                   // Console.WriteLine(MySim.CurrentTime);
                    //Console.WriteLine(TimeSpan.FromMinutes(MySim.CurrentTime) + $" {MyAgent.Zastavky[((MyMessage)message).IndexZastavky].Meno} started generating ppl");
                    
                    break;
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
        public new AgentOkolia MyAgent
        {
            get
            {
                return (AgentOkolia)base.MyAgent;
            }
        }
    }
}
