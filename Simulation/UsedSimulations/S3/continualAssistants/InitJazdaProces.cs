using System;
using OSPABA;
using simulation;
using agents;
using Simulations.UsedSimulations.S3;

namespace continualAssistants
{
    //meta! id="49"
    public class InitJazdaProces : Process
    {
        public InitJazdaProces(int id, OSPABA.Simulation mySim, CommonAgent myAgent) :
            base(id, mySim, myAgent)
        {
        }

        override public void PrepareReplication()
        {
            base.PrepareReplication();
            // Setup component for the next replication
        }

        //meta! sender="AgentAutobusov", id="50", type="Start"
        public void ProcessStart(MessageForm message)
        {
            var sprava = (MyMessage) message.CreateCopy();
            sprava.Code = Mc.InitJazda;
            sprava.Autobus = ((MyMessage) message).Autobus;

            Hold(sprava.Autobus.ZacitokJazdyCas, sprava);
        }

        //meta! userInfo="Process messages defined in code", id="0"
        public void ProcessDefault(MessageForm message)
        {
            switch (message.Code)
            {
                case Mc.InitJazda:
                    MyMessage sprava = (MyMessage)message.CreateCopy();

                    sprava.Autobus = ((MyMessage)message).Autobus;
                    sprava.Autobus.JazdaStart = MySim.CurrentTime;
                    //Console.WriteLine($"{TimeSpan.FromMinutes(MySim.CurrentTime)} Zaciatok jazdy {sprava.Autobus.Id}");
                    sprava.Addressee = MyAgent;
                    Notice(sprava);

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
        public new AgentAutobusov MyAgent
        {
            get
            {
                return (AgentAutobusov)base.MyAgent;
            }
        }
    }
}
