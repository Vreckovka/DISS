using System;
using System.Threading;
using Simulation.Distributions;

namespace Simulation.Simulations
{
    public class S1 : BaseSimulation
    {

        private static EventWaitHandle waitHandle = new ManualResetEvent(true);

        #region Distributions

        private static UniformContinuousDistribution AB;
        private static UniformContinuousDistribution BC;
        private static UniformContinuousDistribution CD;
        private static UniformContinuousDistribution DE;
        private static UniformContinuousDistribution AF;
        private static UniformContinuousDistribution FH;
        private static UniformContinuousDistribution HC;
        private static UniformContinuousDistribution HD;

        private static DiscreetEmpiricalDistribution FG;

        private static UniformDiscreetDistribution GE;

        #endregion


        public S1() : base()
        {
        }

        private void CreateDistributions(Random random)
        {
            AB = new UniformContinuousDistribution(170, 217, random);
            BC = new UniformContinuousDistribution(120, 230, random);
            CD = new UniformContinuousDistribution(50, 70, random);
            DE = new UniformContinuousDistribution(19, 36, random);
            AF = new UniformContinuousDistribution(150, 240, random);
            FH = new UniformContinuousDistribution(30, 62, random);
            HC = new UniformContinuousDistribution(150, 220, random);
            HD = new UniformContinuousDistribution(170, 200, random);

            FG = new DiscreetEmpiricalDistribution(170, 195, 0.2, 196, 280, 0.8, random);

            GE = new UniformDiscreetDistribution(20, 49, random);
        }

        /// <summary>
        /// Create simulation, returns string[] where at index:
        /// 0 - A-B-C-D-E,
        /// 1 - A-F-H-D-E,
        /// 2 - A-F-G-E
        /// 3 - Best route,
        /// 4 - probability of manage in time
        /// </summary>
        /// <returns></returns>
        public override string[] Simulate(Random random, int replicationCount)
        {
            CreateDistributions(random);

            int bestRoute = 2;
            SpinWait sw = new SpinWait();
            sw.SpinOnce();

            double route_ABCDE = 0;
            double route_AFHDE = 0;
            double route_AFGE = 0;

            TimeSpan timeStart = new TimeSpan(7, 30, 0);
            TimeSpan timeEnd = new TimeSpan(15, 0, 0);

            int positive = 0;

            for (int i = 0; i < replicationCount; i++)
            {
                positive += CalculateOneReplicationOfTime(timeEnd - timeStart);

                GetBestRouteReplication(random,ref route_ABCDE, ref route_AFHDE, ref route_AFGE);

                var s = new string[]
                {
                    (route_ABCDE / (i + 1)).ToString(),
                    (route_AFHDE / (i + 1)).ToString(),
                    (route_AFGE / (i + 1)).ToString(),
                    (route_AFGE / (i + 1)).ToString(),
                    ((double)positive / i).ToString()
                };

                OnReplicationFinished(s);
                
                sw.SpinOnce();
                //sw.SpinOnce();
                //sw.SpinOnce();

                // Thread.Sleep(1);
                waitHandle.WaitOne();
            }

            return new string[]
            {
                (route_ABCDE / (replicationCount + 1)).ToString(),
                (route_AFHDE / (replicationCount + 1)).ToString(),
                (route_AFGE / (replicationCount + 1)).ToString(),
                (route_AFGE / (replicationCount + 1)).ToString(),
                ((double)positive / replicationCount).ToString()
            };
        }

        private int CalculateOneReplicationOfTime(TimeSpan timeSpan)
        {
            var time = AF.GetNext() + FG.GetNext() + GE.GetNext();

            if (time <= (timeSpan).TotalMinutes)
            {
                return 1;
            }

            return 0;
        }

        private void GetBestRouteReplication(Random random,ref double route_ABCDE,
            ref double route_AFHDE, ref double route_AFGE)
        {
            var ab = AB.GetNext();
            var bc = BC.GetNext();
            var cd = CD.GetNext();
            var de = DE.GetNext();
            var af = AF.GetNext();
            var fh = FH.GetNext();
            var hd = HD.GetNext();
            var hc = HC.GetNext();
            var fg = FG.GetNext();
            var ge = GE.GetNext();

            route_ABCDE += ab + bc + cd + de;

            if (random.NextDouble() > 0.05)
                route_AFHDE += af + fh + hd + de;
            else
                route_AFHDE += af + fh + hc + cd + de;

            route_AFGE += af + fg + ge;

        }

        public void OnPauseClick()
        {
            waitHandle.Reset();
        }

        public void OnResumeClick()
        {
            waitHandle.Set();
        }
    }
}
