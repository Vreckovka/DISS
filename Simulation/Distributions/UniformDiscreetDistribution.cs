using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Simulations.Distributions
{
   public class UniformDiscreetDistribution : Distribution
    {
        #region Global variables

        private readonly int _min;
        private readonly int _max;
        private Random _random;
        #endregion

        public UniformDiscreetDistribution(int min, int max,int seed) : base(seed)
        {
            _random = new Random(seed);
            _min = min;
            _max = max;
        }

        public override double GetNext()
        {
            return _random.Next(_min, _max + 1);
        }
    }
}
