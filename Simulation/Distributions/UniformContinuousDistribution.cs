using System;
using Simulations.Distributions;

namespace Simulations.Distributions
{
    public class UniformContinuousDistribution : Distribution
    {
        #region Global variables

        private readonly double _min;
        private readonly double _max;
        #endregion

        public UniformContinuousDistribution(double min, double max, int seed) : base(seed)
        {
            _min = min;
            _max = max;
        }

        public override double GetNext()
        {
            return _min + ((_max - _min) * _random.NextDouble());
        }
    }
}
