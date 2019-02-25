using System;
using System.Threading;
using Simulation.Distributions;

namespace Simulation.Simulations
{
    public class S1 : BaseSimulation
    {
        #region Distributions

        private  UniformContinuousDistribution AB;
        private  UniformContinuousDistribution BC;
        private  UniformContinuousDistribution CD;
        private  UniformContinuousDistribution DE;
        private  UniformContinuousDistribution AF;
        private  UniformContinuousDistribution FH;
        private  UniformContinuousDistribution HC;
        private  UniformContinuousDistribution HD;

        private  DiscreetEmpiricalDistribution FG;

        private  UniformDiscreetDistribution GE;

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
        public override double[] Simulate(Random random, int replicationCount, bool liveSimulation)
        {
            CreateDistributions(random);

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

                GetBestRouteReplication(random, ref route_ABCDE, ref route_AFHDE, ref route_AFGE);

                var s = new double[]
                {
                    (route_ABCDE / (i + 1)),
                    (route_AFHDE / (i + 1)),
                    (route_AFGE / (i + 1)),
                    (route_AFGE / (i + 1)),
                    ((double)positive / i)
                };

                if (liveSimulation)
                {
                    ManageSimulationSpeed();
                    OnReplicationFinished(s);
                }
            }

            return new double[]
            {
                (route_ABCDE / (replicationCount)),
                (route_AFHDE / (replicationCount)),
                (route_AFGE / (replicationCount)),
                (route_AFGE / (replicationCount)),
                ((double)positive / replicationCount)
            };
        }

        public override double[] SimulateRuns(int numberOfRuns, int numberOfReplication)
        {
            Random random = new Random();

            long TOTAL_route_ABCDE = 0;
            long TOTAL_route_AFHDE = 0;
            long TOTAL_route_AFGE = 0;
            long TOTAL_prop = 0;

            for (int i = 0; i < numberOfRuns; i++)
            {
                double route_ABCDE = 0;
                double route_AFHDE = 0;
                double route_AFGE = 0;
                double prop = 0;


                var replicationOutput = Simulate(random, numberOfReplication, false);
                route_ABCDE += Convert.ToDouble(replicationOutput[0]);
                route_AFHDE += Convert.ToDouble(replicationOutput[1]);
                route_AFGE += Convert.ToDouble(replicationOutput[2]);
                prop += Convert.ToDouble(replicationOutput[4]);


                TOTAL_route_ABCDE += (long)route_ABCDE;
                TOTAL_route_AFHDE += (long)route_AFHDE;
                TOTAL_route_AFGE += (long)route_AFGE;
                TOTAL_prop += (long)prop;

                OnRunFinished(new double[]
                {
                  route_ABCDE,
                  route_AFHDE,
                  route_AFGE,
                  prop
                });

            }

            return new double[]
            {
                (TOTAL_route_ABCDE / (numberOfRuns)),
                (TOTAL_route_AFHDE / (numberOfRuns)),
                (TOTAL_route_AFGE / (numberOfRuns)),
                (TOTAL_prop / (numberOfRuns))
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

        private void GetBestRouteReplication(Random random, ref double route_ABCDE,
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
    }
}
