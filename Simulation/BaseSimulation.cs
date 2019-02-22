using System;

namespace Simulation
{
    public abstract class BaseSimulation
    {
        #region Properties
        public Random Random { get; set; }
        public int ReplicationCount { get; set; }

        #endregion

        public event EventHandler<string[]> ReplicationFinished;

        public BaseSimulation(int replicationCount, Random random)
        {
            ReplicationCount = replicationCount;
            Random = random;
        }

        public abstract string[] Simulate();

        protected virtual void OnReplicationFinished(string[] e)
        {
            ReplicationFinished?.Invoke(this, e);
        }
    }
}
