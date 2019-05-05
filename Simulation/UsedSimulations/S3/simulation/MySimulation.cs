using System;
using System.Linq;
using OSPABA;
using agents;
using PropertyChanged;

namespace simulation
{
    [AddINotifyPropertyChangedInterface]
    public class MySimulation : OSPABA.Simulation
    {
        public Random Random { get; set; }


        public double FinishedCasCakania { get; set; }
        public double AvrageCakania { get; set; }

        public double FinishedPocetLudi { get; set; }
        public double AvragePocetLudi { get; set; }

        public double FinishedTimes { get; set; }
        public double LastFinishTime { get; set; }
        public double AvrageFinishedTime { get; set; }
        public MySimulation()
        {
            Random = new Random();
            Init();
        }

        protected override void PrepareSimulation()
        {
            base.PrepareSimulation();
            // Create global statistcis
        }

        protected override void PrepareReplication()
        {
            base.PrepareReplication();
            // Reset entities, queues, local statistics, etc...
        }

        protected override void ReplicationFinished()
        {
            // Collect local statistics into global, update UI, etc...
            FinishedTimes += LastFinishTime;
            FinishedPocetLudi += AgentOkolia.CelkovyPocetCestujucich;

            //Na stadione
            FinishedCasCakania += (from x in AgentOkolia.Zastavky[3].Cestujuci select x.CasCakania).Average();

            AvragePocetLudi = FinishedPocetLudi / (CurrentReplication + 1);
            AvrageFinishedTime = FinishedTimes / (CurrentReplication + 1);
            AvrageCakania = FinishedCasCakania / (CurrentReplication + 1);

            base.ReplicationFinished();
        }

        protected override void SimulationFinished()
        {
            ;
            // Dysplay simulation results
            base.SimulationFinished();
        }

        //meta! userInfo="Generated code: do not modify", tag="begin"
        private void Init()
        {
            AgentModelu = new AgentModelu(SimId.AgentModelu, this, null);
            AgentOkolia = new AgentOkolia(SimId.AgentOkolia, this, AgentModelu);
            AgentLiniek = new AgentLiniek(SimId.AgentLiniek, this, AgentModelu);
            AgentZastavok = new AgentZastavok(SimId.AgentZastavok, this, AgentLiniek);
            AgentAutobusov = new AgentAutobusov(SimId.AgentAutobusov, this, AgentLiniek);
        }
        public AgentModelu AgentModelu
        { get; set; }
        public AgentOkolia AgentOkolia
        { get; set; }
        public AgentLiniek AgentLiniek
        { get; set; }
        public AgentZastavok AgentZastavok
        { get; set; }
        public AgentAutobusov AgentAutobusov
        { get; set; }
        //meta! tag="end"
    }
}
