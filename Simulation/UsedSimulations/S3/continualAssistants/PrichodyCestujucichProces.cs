using System;
using System.Threading;
using OSPABA;
using simulation;
using agents;
using Simulations.Distributions;
using Simulations.UsedSimulations.S3.entities;

namespace continualAssistants
{
    //meta! id="66"
    public class PrichodyCestujucichProces : Process
    {
        private ExponentialDistribution[] _exp;
        private int cestujuciIndex;
        public PrichodyCestujucichProces(int id, OSPABA.Simulation mySim, CommonAgent myAgent) :
            base(id, mySim, myAgent)
        {

        }

        public void VytvorGeneratori()
        {
          
            _exp = new ExponentialDistribution[MyAgent.Zastavky.Count + 3];

            for (int i = 0; i < MyAgent.Zastavky.Count; i++)
            {
                _exp[i] = new ExponentialDistribution((1.0 / (65.0 / MyAgent.Zastavky[i].MaxPocetVygenerovanych)), ((MySimulation)MySim).Random.Next());
            }
        }


        override public void PrepareReplication()
        {
            base.PrepareReplication();
            MyAgent.VytvorZastavky();
            VytvorGeneratori();
            // Setup component for the next replication
        }

        //meta! sender="AgentOkolia", id="67", type="Start"
        public void ProcessStart(MessageForm message)
        {
            message.Code = Mc.PrichodCestujuceho;
            Hold(_exp[message.Param].GetNext(), message);

        }

        //meta! userInfo="Process messages defined in code", id="0"
        public void ProcessDefault(MessageForm message)
        {
            switch (message.Code)
            {
                case Mc.PrichodCestujuceho:

                    if (MySim.CurrentTime <= MyAgent.Zastavky[(int)message.Param].CasKoncaGenerovania
                        && MyAgent.Zastavky[(int)message.Param].PocetVygenerovanych  < 
                        MyAgent.Zastavky[(int)message.Param].MaxPocetVygenerovanych)
                    {
                        Hold(_exp[(int)message.Param].GetNext(), message.CreateCopy());

                        var sprava = (MyMessage)message;
                        MyAgent.Zastavky[(int)message.Param].Cestujuci.Enqueue(new Cestujuci(cestujuciIndex++, MySim));
                        MyAgent.Zastavky[(int)message.Param].PocetCestujucich++;

                        MyAgent.Zastavky[(int)message.Param].PocetVygenerovanych++;
                        MyAgent.CelkovyPocetCestujucich++;

                        sprava.Code = Mc.PrichodCestujuceho;
                        sprava.Addressee = MyAgent;
                        Notice(sprava);

                       
                        //Console.WriteLine($"Cestujuci prisiel na zastavku {MyAgent.Zastavky[(int)message.Param].Meno}");
                    }
                    else
                    {
                        AssistantFinished(message);
                    }
                   
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
