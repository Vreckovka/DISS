using System;
using System.Threading;
using Simulation.Distributions;

namespace Simulation.Simulations
{
    public class S1 : BaseSimulation
    {
        private static Random _random;

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


        public S1(Random random) : base(random)
        {
            _random = random;

            AB = new UniformContinuousDistribution(170, 217, _random);
            BC = new UniformContinuousDistribution(120, 230, _random);
            CD = new UniformContinuousDistribution(50, 70, _random);
            DE = new UniformContinuousDistribution(19, 36, _random);
            AF = new UniformContinuousDistribution(150, 240, _random);
            FH = new UniformContinuousDistribution(30, 62, _random);
            HC = new UniformContinuousDistribution(150, 220, _random);
            HD = new UniformContinuousDistribution(170, 200, _random);

            FG = new DiscreetEmpiricalDistribution(170, 195, 0.2, 196, 280, 0.8, _random);

            GE = new UniformDiscreetDistribution(20, 49, _random);
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
        public override string[] Simulate(int replicationCount)
        {
            int bestRoute = 2;

            double route_ABCDE = 0;
            double route_AFHDE = 0;
            double route_AFGE = 0;

            TimeSpan timeStart = new TimeSpan(7, 30, 0);
            TimeSpan timeEnd = new TimeSpan(15, 0, 0);

            int positive = 0;

            for (int i = 0; i < replicationCount; i++)
            {
                positive += CalculateOneReplicationOfTime(timeEnd - timeStart);

                GetBestRouteReplication(ref route_ABCDE, ref route_AFHDE, ref route_AFGE);

                var s = new string[]
                {
                    (route_ABCDE / (i + 1)).ToString(),
                    (route_AFHDE / (i + 1)).ToString(),
                    (route_AFGE / (i + 1)).ToString(),
                    (route_AFGE / (i + 1)).ToString(),
                    ((double)positive / i).ToString()
                };

                OnReplicationFinished(s);
                Thread.Sleep(1);
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

        private void GetBestRouteReplication(ref double route_ABCDE,
            ref double route_AFHDE,ref double route_AFGE)
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

            if (_random.NextDouble() > 0.05)
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
