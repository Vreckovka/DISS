using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Simulations.Distributions
{
    public class TriangularDistribution : Distribution
    {
        private int a;
        private int b;
        private int c;
        private Random _random;
        
        public TriangularDistribution(int seed, int min, int max, int modus) : base(seed)
        {
            _random = new Random(seed);
            a = min;
            b = max;
            c = modus;
        }

        public override double GetNext()
        {
            double F = (double)(c - a) / (b - a);
            double rand = _random.NextDouble();
                
            if (rand < F)
            {
                return a + Math.Sqrt(rand * (b - a) * (c - a));
            }
            else
            {
                return b - Math.Sqrt((1 - rand) * (b - a) * (b - c));
            }
        }
    }
}
