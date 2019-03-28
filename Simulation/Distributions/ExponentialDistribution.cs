using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Simulations.Distributions
{
    public class ExponentialDistribution : Distribution
    {

        private double lambda;
        public ExponentialDistribution(double lambda, int seed) : base(seed)
        {
            this.lambda = lambda;
        }

        public override double GetNext()
        {
            return Math.Log(1 - _random.NextDouble()) / (-lambda);
        }
    }
}
