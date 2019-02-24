using System;
using System.Threading;

namespace Simulation
{
    public abstract class BaseSimulation
    {
        protected static EventWaitHandle waitHandle = new ManualResetEvent(true);
        public event EventHandler<string[]> ReplicationFinished;
        SpinWait sw = new SpinWait();
        public int SimulationDelay { get; set; }

        public BaseSimulation()
        {
        }

        public abstract string[] Simulate(Random random,int replicationCount);

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

        protected virtual void OnReplicationFinished(string[] e)
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
    }
}
