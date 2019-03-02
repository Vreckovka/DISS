using System;

namespace Simulation.Distributions
{
    class UniformContinuousDistribution : Distribution
    {
        #region Global variables

        private readonly double _min;
        private readonly double _max;
        private Random _random;
        #endregion

        public UniformContinuousDistribution(double min, double max, int seed) : base(seed)
        {
            _random = new Random(seed);
            _min = min;
            _max = max;
        }

        public override double GetNext()
        {
            return _min + ((_max - _min) * _random.NextDouble());
        }
    }
}
