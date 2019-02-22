using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Simulation.Distributions
{
    class UniformDiscreetDistribution : Distribution
    {
        #region Global variables

        private int _min;
        private int _max;
        private Random _random;

        #endregion

        public UniformDiscreetDistribution(int min, int max,Random random) : base(random)
        {
            _min = min;
            _max = max;
            _random = random;
        }

        public override double GetNext()
        {
            return _random.Next(_min, _max);
        }
    }
}
