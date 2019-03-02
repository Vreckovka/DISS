using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Simulation.Distributions
{
    class DiscreetEmpiricalDistribution : Distribution
    {
        #region Global variables

        private readonly int _min1;
        private readonly int _max1;
        private readonly double _p1;

        private readonly int _min2;
        private readonly int _max2;

        private Random _random;
        #endregion

        /// <summary>
        /// Constructor for DiscreetEmpiricalDistribution
        /// </summary>
        /// <param name="min1">T1_MIN</param>
        /// <param name="max1">T1_MAX</param>
        /// <param name="p1">Probability for T1</param>
        /// <param name="min2">T2_MIN</param>
        /// <param name="max2">T2_MAX</param>
        /// <param name="p2">Probability for T2</param>
        /// <param name="seed"></param>
        public DiscreetEmpiricalDistribution(int min1,int max1,double p1,int min2, int max2, 
            int seed) : base(seed)
        {
            _random = new Random(seed);
            _min1 = min1;
            _max1 = max1;
            _p1 = p1;

            _min2 = min2;
            _max2 = max2;

        }

        public override double GetNext()
        {
            if (_random.NextDouble() < _p1)
                return _random.Next(_min1, _max1 + 1);
            else
                return _random.Next(_min2, _max2 + 1);
        }
    }
}
