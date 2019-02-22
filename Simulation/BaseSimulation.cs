using System;

namespace Simulation
{
    public abstract class BaseSimulation
    {

        public event EventHandler<string[]> ReplicationFinished;

        public BaseSimulation(Random random)
        {
        }

        public abstract string[] Simulate(int replicationCount);

        protected virtual void OnReplicationFinished(string[] e)
        {
            ReplicationFinished?.Invoke(this, e);
        }
    }
}
