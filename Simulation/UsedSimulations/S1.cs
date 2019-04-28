using System;
using System.Collections.Generic;
using System.Threading;
using Simulations.Distributions;

namespace Simulation.Simulations
{
    public class S1 : BaseMCSimulation
    {
       
        #region Distributions

        private UniformContinuousDistribution AB;
        private UniformContinuousDistribution BC;
        private UniformContinuousDistribution CD;
        private UniformContinuousDistribution DE;
        private UniformContinuousDistribution AF;
        private UniformContinuousDistribution FH;
        private UniformContinuousDistribution HC;
        private UniformContinuousDistribution HD;

        private DiscreetEmpiricalDistribution FG;

        private UniformDiscreetDistribution GE;

        #endregion

        private Random routeRandom;
        private TimeSpan timeStart;
        private TimeSpan timeEnd;
        public S1() : base()
        {
            routeRandom = new Random();
            timeStart = new TimeSpan(7, 30, 0);
            timeEnd = new TimeSpan(15, 0, 0);
        }

        protected override void CreateDistributions(Random random)
        {
            AB = new UniformContinuousDistribution(170, 217, random.Next());
            BC = new UniformContinuousDistribution(120, 230, random.Next());
            CD = new UniformContinuousDistribution(50, 70, random.Next());
            DE = new UniformContinuousDistribution(19, 36, random.Next());
            AF = new UniformContinuousDistribution(150, 240, random.Next());
            FH = new UniformContinuousDistribution(30, 62, random.Next());
            HC = new UniformContinuousDistribution(150, 220, random.Next());
            HD = new UniformContinuousDistribution(170, 200, random.Next());

            var FGData = new List<DiscreetEmpiricalDistributionData>()
            {
                new DiscreetEmpiricalDistributionData(170, 195, 0.2, random.Next()),
                new DiscreetEmpiricalDistributionData(196, 280, 0.8,random.Next()),
            };

            FG = new DiscreetEmpiricalDistribution(FGData, random.Next());

            GE = new UniformDiscreetDistribution(20, 49, random.Next());
        }

        /// <summary>
        /// Create replication, returns double[] where at index:
        /// 0 - A-B-C-D-E,
        /// 1 - A-F-H-D-E,
        /// 2 - A-F-G-E
        /// 3 - Best route,
        /// 4 - probability of manage in time
        /// </summary>
        /// <returns></returns>
        protected override double[] DoSimulationReplication()
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

            var route_ABCDE = ab + bc + cd + de;

            double route_AFHDE = 0;

            if (routeRandom.NextDouble() > 0.05)
                route_AFHDE += af + fh + hd + de;
            else
                route_AFHDE += af + fh + hc + cd + de;

            double route_AFGE = af + fg + ge;

            double positive = 0;

            //Probability 
            if (route_ABCDE <= (timeEnd - timeStart).TotalMinutes)
            {
                positive++;
            }
            
            return new double[]
            {
                route_ABCDE,
                route_AFHDE,
                route_AFGE,
                route_ABCDE,
                positive
            };
        }
    }
}
