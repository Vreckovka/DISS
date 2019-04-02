using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Simulations.Distributions
{
   public class DiscreetEmpiricalDistributionData
    {
        public int Max { get; set; }
        public int Min { get; set; }
        public double Probability { get; set; }
        public Random Random { get; set; }
        public DiscreetEmpiricalDistributionData(int min, int max, double probability, int seed)
        {
            Max = max;
            Min = min;
            Probability = probability;
            Random = new Random(seed);
        }
    }
}
