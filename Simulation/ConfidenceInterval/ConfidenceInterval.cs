using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.UI.DataVisualization.Charting;
using PropertyChanged;


namespace Simulations.ConfidenceInterval
{
    public static class ConfidenceInterval
    {
        private static double GetMean(List<double> data)
        {
            var sum = data.Sum();
            return sum / data.Count;
        }

        private static double GetStandardDeviation(List<double> data, double mean)
        {
            double standardDeviation = 0;

            for (int i = 0; i < data.Count; i++)
            {
                standardDeviation += Math.Pow(data[i] - mean, 2);
            }

            standardDeviation = standardDeviation / (data.Count - 1);

            return Math.Sqrt(standardDeviation);
        }

        public static double[] GetInterval(double confidence, List<double> data)
        {
            double[] interval = new double[2];
            var mean = GetMean(data);
            var standardDeviation = GetStandardDeviation(data, mean);

            double res = new Chart().DataManipulator.Statistics.InverseTDistribution(1 - confidence, data.Count - 1);

            interval[0] = mean - res * (standardDeviation / Math.Sqrt(data.Count));
            interval[1] = mean + res * (standardDeviation / Math.Sqrt(data.Count));

            return interval;
        }

        public static double[] GetInterval(double confidence, SampleStandardDeviationData sampleStandardDeviationData)
        {
            if (sampleStandardDeviationData != null && sampleStandardDeviationData.Count > 1)
            {
                double[] interval = new double[2];
                var standardDeviation = sampleStandardDeviationData.GetSampleStandardDeviation();

                double res =
                    new Chart().DataManipulator.Statistics.InverseTDistribution(1 - confidence,
                        sampleStandardDeviationData.Count - 1);

                interval[0] = sampleStandardDeviationData.Mean -
                              res * (standardDeviation / Math.Sqrt(sampleStandardDeviationData.Count));
                interval[1] = sampleStandardDeviationData.Mean +
                              res * (standardDeviation / Math.Sqrt(sampleStandardDeviationData.Count));

                return interval;
            }

            return null;
        }

        public static string ToStringInterval(double confidence, List<double> data)
        {
            var interval = GetInterval(confidence, data);
            return $"IS {confidence * 100}% <{interval[0]:N5},{interval[1]:N5}>";
        }

        public static string ToStringInterval(double confidence, SampleStandardDeviationData sampleStandardDeviationData)
        {
            var interval = GetInterval(confidence, sampleStandardDeviationData);
            if (interval != null)
                return $"Confidence interval {confidence * 100}% <{interval[0]:N5},{interval[1]:N5}>";
            return "";
        }

        [AddINotifyPropertyChangedInterface]
        public class SampleStandardDeviationData
        {
            private double Sum { get; set; }
            private double SumSqrt { get; set; }
            public int Count { get; set; }

            public double Mean
            {
                get { return Sum / Count; }
            }

            public void AddValue(double value)
            {
                Count++;
                Sum += value;
                SumSqrt += value * value;
            }

            public double GetSampleStandardDeviation()
            {
                var meanSqrt = (SumSqrt) / (Count);

                return Math.Sqrt(meanSqrt - (Mean * Mean));
            }
        }
    }
}
