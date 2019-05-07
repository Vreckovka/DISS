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


        public OSPStat.Stat StatAvrageCakanieA { get; set; }
        public double[] StatAvrageCakanieAInterval { get; set; }
        public double AvrageCakanieA { get; set; }

        public OSPStat.Stat StatAvrageCakanieB { get; set; }
        public double[] StatAvrageCakanieBInterval { get; set; }
        public double AvrageCakanieB { get; set; }

        public OSPStat.Stat StatAvrageCakanieC { get; set; }
        public double[] StatAvrageCakanieCInterval { get; set; }
        public double AvrageCakanieC { get; set; }

        public OSPStat.Stat StatAvragePocetLudi { get; set; }
        public double[] StatAvragePocetLudiInterval { get; set; }

        public OSPStat.Stat StatAvrageNeprislo { get; set; }
        public double[] StatAvrageNeprisloInterval { get; set; }

        public OSPStat.Stat StatAvrageCakanie { get; set; }
        public double[] StatAvrageCakanieInterval { get; set; }

        public OSPStat.Stat StatAvrageMicro { get; set; }
        public double[] StatAvrageMicroInterval { get; set; }


        public double AvrageCakania { get; set; }
        public double AvragePocetLudi { get; set; }
        public double AvrageFinishedNaPo { get; set; }
        public double AvrageMicro { get; set; }

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
            AvrageFinishedNaPo = 0;

            StatAvrageCakanieA = new OSPStat.Stat();
            StatAvrageCakanieB = new OSPStat.Stat();
            StatAvrageCakanieC = new OSPStat.Stat();

            StatAvragePocetLudi = new OSPStat.Stat();
            StatAvrageNeprislo = new OSPStat.Stat();
            StatAvrageCakanie = new OSPStat.Stat();
            StatAvrageMicro = new OSPStat.Stat();

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

            try
            {
                StatAvragePocetLudi.AddSample(AgentOkolia.CelkovyPocetCestujucich);
                StatAvrageMicro.AddSample((from x in AgentAutobusov.Autobusy where x.Typ == Simulations.UsedSimulations.S3.entities.AutobusyTyp.Microbus select x.CelkovyPocetPrevezenych).Sum());
                StatAvrageCakanie.AddSample(((from x in AgentOkolia.Zastavky[3].Cestujuci select x.CasCakania).Average()) / 60);
                StatAvrageNeprislo.AddSample((1 - ((double)(from x in AgentOkolia.Zastavky[3].Cestujuci where x.PrisielNaCas select x).Count() / AgentOkolia.Zastavky[3].Cestujuci.Count)) * 100);


                var a = (from x in AgentOkolia.Zastavky[3].Cestujuci where x.Linka == AgentOkolia.Linky[0] select x.CasCakania);
                var b = (from x in AgentOkolia.Zastavky[3].Cestujuci where x.Linka == AgentOkolia.Linky[1] select x.CasCakania);
                var c = (from x in AgentOkolia.Zastavky[3].Cestujuci where x.Linka == AgentOkolia.Linky[2] select x.CasCakania);

               
                if (a.Count() > 0)
                    StatAvrageCakanieA.AddSample(a.Average() / 60);

                if (b.Count() > 0)
                    StatAvrageCakanieA.AddSample(b.Average() / 60);

                if (c.Count() > 0)
                    StatAvrageCakanieA.AddSample(c.Average() / 60);


                if (StatAvragePocetLudi.SampleSize > 1)
                {
                    StatAvragePocetLudiInterval = new double[] { StatAvragePocetLudi.ConfidenceInterval90[0], StatAvragePocetLudi.ConfidenceInterval90[1] };
                    StatAvrageCakanieInterval = new double[] { StatAvrageCakanie.ConfidenceInterval90[0], StatAvrageCakanie.ConfidenceInterval90[1] };
                    StatAvrageMicroInterval = new double[] { StatAvrageMicro.ConfidenceInterval90[0], StatAvrageMicro.ConfidenceInterval90[1] };
                    StatAvrageNeprisloInterval = new double[] { StatAvrageNeprislo.ConfidenceInterval90[0], StatAvrageNeprislo.ConfidenceInterval90[1] };

                    AvrageMicro = StatAvrageMicro.Mean();
                    AvrageFinishedNaPo = StatAvrageNeprislo.Mean();
                    AvragePocetLudi = StatAvragePocetLudi.Mean();
                    AvrageCakania = StatAvrageCakanie.Mean();

                    AvrageCakanieA = StatAvrageCakanieA.Mean();
                    AvrageCakanieB = StatAvrageCakanieB.Mean();
                    AvrageCakanieC = StatAvrageCakanieC.Mean();
                }


            }
            catch (Exception ex)
            {
                ;
            }

            base.ReplicationFinished();
        }

        protected override void SimulationFinished()
        {
            base.SimulationFinished();
        }

        public void Reset()
        {
            foreach (var zastavka in AgentOkolia.Zastavky)
            {
                zastavka.Reset();
            }

            if (AgentAutobusov.Autobusy != null)
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
