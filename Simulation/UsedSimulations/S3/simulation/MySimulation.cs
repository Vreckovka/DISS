using System;
using System.Linq;
using OSPABA;
using agents;
using PropertyChanged;
using Simulations.UsedSimulations.S3;

namespace simulation
{
    [AddINotifyPropertyChangedInterface]
    public class MySimulation : OSPABA.Simulation
    {
        public Random Random { get; set; }
        public Configuration Configration { get; set; }

        public double FinishedCasCakania { get; set; }
        public double AvrageCakania { get; set; }

        public double FinishedPocetLudi { get; set; }
        public double AvragePocetLudi { get; set; }


        public double FinishedNaPo { get; set; }
        public double AvrageFinishedNaPo { get; set; }

        public double FinishedMicro { get; set; }
        public double AvrageMicro { get; set; }


        public double FinishedTimes { get; set; }
        public double LastFinishTime { get; set; }
        public double AvrageFinishedTime { get; set; }
        public MySimulation(Configuration configration)
        {
            Random = new Random();
            Configration = configration;
            Init();
        }

        protected override void PrepareSimulation()
        {
            AvragePocetLudi = 0;
            AvrageFinishedTime = 0;
            AvrageCakania = 0;
            AvrageMicro = 0;

            FinishedMicro = 0;
            FinishedCasCakania = 0;
            FinishedTimes = 0;
            FinishedPocetLudi = 0;
            FinishedNaPo = 0;
            AvrageFinishedNaPo = 0;

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

            FinishedTimes += LastFinishTime;
            FinishedPocetLudi += AgentOkolia.CelkovyPocetCestujucich;
            FinishedNaPo += (1 - (  (double)(from x in AgentOkolia.Zastavky[3].Cestujuci where x.PrisielNaCas select x).Count() / AgentOkolia.Zastavky[3].Cestujuci.Count)) * 100;
            FinishedCasCakania += ((from x in AgentOkolia.Zastavky[3].Cestujuci select x.CasCakania).Average() ) / 60;
            FinishedMicro += (from x in AgentAutobusov.Autobusy where x.Typ == Simulations.UsedSimulations.S3.entities.AutobusyTyp.Microbus select x.CelkovyPocetPrevezenych).Sum();


            AvrageMicro = FinishedMicro / (CurrentReplication + 1);
            AvrageFinishedNaPo = FinishedNaPo / (CurrentReplication + 1);
            AvragePocetLudi = FinishedPocetLudi / (CurrentReplication + 1);
            AvrageFinishedTime = FinishedTimes / (CurrentReplication + 1);
            AvrageCakania = FinishedCasCakania / (CurrentReplication + 1);

            base.ReplicationFinished();
        }

        protected override void SimulationFinished()
        {

            // Dysplay simulation results



            base.SimulationFinished();
        }

        public void Reset()
        {
            foreach (var zastavka in AgentOkolia.Zastavky)
            {
                zastavka.Reset();
            }



            foreach (var autobus in AgentAutobusov.Autobusy)
            {
                autobus.HardReset();
            }
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
