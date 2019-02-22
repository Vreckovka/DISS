using System;
using System.Threading;
using Simulation.Distributions;

namespace Simulation.Simulations
{
    public class S1 : BaseSimulation
    {
        private static Random _random;
        private int _replicationCount;

        public delegate void ThresholdReachedEventHandler(object sender, double e);

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


        public S1(int replicationCount, Random random) : base(replicationCount, random)
        {
            _random = random;
            _replicationCount = replicationCount;

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
        /// 0 - Best route,
        /// 1 - probability of manage in time
        /// </summary>
        /// <returns></returns>
        public override string[] Simulate()
        {
            int bestRoute = GetBestRoute();

            TimeSpan timeStart = new TimeSpan(7, 30, 0);
            TimeSpan timeEnd = new TimeSpan(15, 0, 0);

            int positive = 0;

            for (int i = 0; i < _replicationCount; i++)
            {
                var time = CalculateBestRoute(bestRoute);

                if (time <= (timeEnd - timeStart).TotalMinutes)
                {
                    positive++;
                }
            }

            double p = (double)positive / _replicationCount;

            return new string[]
            {
                bestRoute.ToString(),
                p.ToString()
            };

        }

        private static double CalculateBestRoute(int bestRoute)
        {
            switch (bestRoute)
            {
                case 0:
                    return CalculateRoute_ABCDE();
                case 1:
                    return CalculateRoute_AFHDE();
                case 2:
                    return CalculateRoute_AFGE();
                default:
                    return double.NaN;
            }
        }

        private static double CalculateRoute_ABCDE()
        {
            return AB.GetNext() + BC.GetNext() + CD.GetNext() + DE.GetNext();
        }

        private static double CalculateRoute_AFHDE()
        {
            double route = 0;

            if (_random.NextDouble() > 0.05)
                route += AF.GetNext() + FH.GetNext() + HD.GetNext() + DE.GetNext();
            else
                route += AF.GetNext() + FH.GetNext() + HC.GetNext() + CD.GetNext() + DE.GetNext();

            return route;
        }

        private static double CalculateRoute_AFGE()
        {
            return AF.GetNext() + FG.GetNext() + GE.GetNext();
        }

        /// <summary>
        /// Returns index of the best route:
        /// 0 - A-B-C-D-E,
        /// 1 - A-F-H-D-E,
        /// 2 - A-F-G-E
        /// </summary>
        /// <returns></returns>
        private int GetBestRoute()
        {
            double route_ABCDE = 0;
            double route_AFHDE = 0;
            double route_AFGE = 0;

            for (int i = 0; i < _replicationCount; i++)
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

                var s = new string[]
                {
                    (route_AFGE / (i + 1)).ToString(),
                    (route_ABCDE / (i + 1)).ToString(),
                    (route_AFHDE / (i + 1)).ToString(),
                    (route_AFGE / (i + 1)).ToString(),
                };

                OnReplicationFinished(s);
                Thread.Sleep(1);
            }


            double route_ABCDE_Time = route_ABCDE / _replicationCount;
            double route_AFHDE_Time = route_AFHDE / _replicationCount;
            double route_AFGE_Time = route_AFGE / _replicationCount;

            if (route_ABCDE_Time < route_AFHDE_Time && route_ABCDE_Time < route_AFGE_Time)
                return 0;
            else if (route_AFHDE_Time < route_ABCDE_Time && route_AFHDE_Time < route_AFGE_Time)
                return 1;
            else
                return 2;
        }
    }
}
