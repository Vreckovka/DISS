using System;
using System.Threading;
using Simulations.Simulations;

namespace Simulation
{
    public abstract class BaseMCSimulation : BaseSimulation
    {
        public int FireOnEveryNIteration { get; set; }
       
        /// <summary>
        /// Simulation delay in running simulation max value = 100, min value = 0
        /// </summary>
       

        #region Abstract methods
        protected abstract void CreateDistributions(Random random);
        protected abstract double[] DoSimulationReplication();
        public event EventHandler<double[]> ReplicationFinished;
        #endregion

        public double[] Simulate(Random random, int replicationCount, bool liveSimulation, int everyNIteration = -1)
        {
            int numberOfParameters = -1;

            CreateDistributions(random);

            double[] totalResult = null;

            FireOnEveryNIteration = everyNIteration;

            for (int i = 0; i < replicationCount; i++)
            {
                var result = DoSimulationReplication();

                if (totalResult == null)
                {
                    totalResult = new double[result.Length];
                    numberOfParameters = result.Length;
                }

                AddIterationResults(ref totalResult, result, numberOfParameters);

                if (liveSimulation)
                {
                    if (SimulationDelay > 0)
                        ManageSimulationSpeed();

                    if (i % FireOnEveryNIteration == 0)
                        OnReplicationFinished(GetAcutalResult(totalResult, numberOfParameters, (i + 1)));

                    if (pause)
                    {
                        waitHandle.WaitOne();
                    }
                }
            }

            var finalResult = GetAcutalResult(totalResult, numberOfParameters, replicationCount);
            OnSimulationFinished(finalResult);

            return finalResult;
        }

        private void AddIterationResults(ref double[] totalResult, double[] results, int numberOfParameters)
        {
            for (int i = 0; i < numberOfParameters; i++)
            {
                totalResult[i] += results[i];
            }
        }

        private double[] GetAcutalResult(double[] totalResult, int numberOfParameters, int iterationNumber)
        {
            double[] pom = new double[totalResult.Length];
            totalResult.CopyTo(pom, 0);

            for (int i = 0; i < numberOfParameters; i++)
            {
                pom[i] /= iterationNumber;
            }

            return pom;
        }

        public double[] SimulateRuns(int numberOfRuns, int numberOfReplication)
        {
            Random random = new Random();

            long TOTAL_route_ABCDE = 0;
            long TOTAL_route_AFHDE = 0;
            long TOTAL_route_AFGE = 0;
            long TOTAL_prop = 0;

            for (int i = 0; i < numberOfRuns; i++)
            {
                double route_ABCDE = 0;
                double route_AFHDE = 0;
                double route_AFGE = 0;
                double prop = 0;

                var replicationOutput = Simulate(random, numberOfReplication, false);

                route_ABCDE += replicationOutput[0];
                route_AFHDE += replicationOutput[1];
                route_AFGE += replicationOutput[2];
                prop += replicationOutput[4];


                TOTAL_route_ABCDE += (long)route_ABCDE;
                TOTAL_route_AFHDE += (long)route_AFHDE;
                TOTAL_route_AFGE += (long)route_AFGE;
                TOTAL_prop += (long)prop;

                OnRunFinished(new double[]
                {
                    route_ABCDE,
                    route_AFHDE,
                    route_AFGE,
                    prop
                });

            }

            return new double[]
            {
                (TOTAL_route_ABCDE / (numberOfRuns)),
                (TOTAL_route_AFHDE / (numberOfRuns)),
                (TOTAL_route_AFGE / (numberOfRuns)),
                (TOTAL_prop / (numberOfRuns))
            };
        }

        protected virtual void OnReplicationFinished(double[] e)
        {
            ReplicationFinished?.Invoke(this, e);
        }
    }
}
