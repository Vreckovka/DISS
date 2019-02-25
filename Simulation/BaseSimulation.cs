using System;
using System.Threading;

namespace Simulation
{
    public abstract class BaseSimulation
    {
        protected static EventWaitHandle waitHandle = new ManualResetEvent(true);
        public event EventHandler<double[]> ReplicationFinished;
        public event EventHandler<double[]> RunFinished;

        SpinWait sw = new SpinWait();
        public int SimulationDelay { get; set; }

        public BaseSimulation()
        {
        }

        public abstract double[] Simulate(Random random,int replicationCount, bool liveSimulation);
        public abstract double[] SimulateRuns(int numberOfRuns, int numberOfReplication);

        protected void ManageSimulationSpeed()
        {
            int speed = SimulationDelay;

            if (SimulationDelay > 51)
            {
                speed = SimulationDelay * 10;
            }
               

            for (int i = 0; i < speed; i++)
            {
                sw.SpinOnce();
            }
        }

        protected virtual void OnReplicationFinished(double[] e)
        {
            ReplicationFinished?.Invoke(this, e);
        }

        public void OnPauseClick()
        {
            waitHandle.Reset();
        }

        public void OnResumeClick()
        {
            waitHandle.Set();
        }

        protected virtual void OnRunFinished(double[] e)
        {
            RunFinished?.Invoke(this, e);
        }
    }
}
