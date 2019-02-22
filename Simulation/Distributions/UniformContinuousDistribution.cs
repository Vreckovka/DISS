using System;

namespace Simulation.Distributions
{
    class UniformContinuousDistribution : Distribution
    {
        #region Global variables

        private double _min;
        private double _max;
        private Random _random;
        
        #endregion

        public UniformContinuousDistribution(double min, double max,Random random) : base(random)
        {
            _random = random;
            _min = min;
            _max = max;
        }

        public override double GetNext()
        {
            return _min + (_max - _min) * _random.NextDouble();
        }
    }
}
