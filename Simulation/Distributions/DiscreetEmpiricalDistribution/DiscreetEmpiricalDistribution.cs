using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Simulations.Distributions
{
    public class DiscreetEmpiricalDistribution : Distribution
    {
        private List<DiscreetEmpiricalDistributionData> _dataList;
        private Random _propGenerator;
        public DiscreetEmpiricalDistribution(List<DiscreetEmpiricalDistributionData> dataList,int seed) : base(seed)
        {
            _dataList = dataList;
            CheckData();
            _propGenerator = new Random(seed + 1);
        }

        private void CheckData()
        {
            double prop = 0;
            foreach (var data in _dataList)
            {
                prop += data.Probability;
            }

            if (prop != 1)
                throw new ArgumentException("Sum of probability is not 1");
        }

        public override double GetNext()
        {
            double prop = _propGenerator.NextDouble();
            double dataProp = 0;

            foreach (var data in _dataList)
            {
                dataProp += data.Probability;

                if (prop < dataProp)
                {
                    return _random.Next(data.Min, data.Max + 1);
                }
            }

            return -1;
        }
    }
}
