using System;

namespace Simulation
{
    public abstract class BaseSimulation
    {

        public event EventHandler<string[]> ReplicationFinished;

        public BaseSimulation()
        {
        }

        public abstract string[] Simulate(Random random,int replicationCount);

        protected virtual void OnReplicationFinished(string[] e)
        {
            ReplicationFinished?.Invoke(this, e);
        }
    }
}
