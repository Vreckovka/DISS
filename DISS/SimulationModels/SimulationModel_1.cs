using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Simulation.Simulations;

namespace DISS.SimulationModels
{
    class SimulationModel_1 : SimulationModel
    { 
        private string[] _simulationData;
        private S1 simulationS1;

        #region Properties
      
        public string[] SimulationData
        {
            get { return _simulationData; }
            set
            {
                if (value != _simulationData)
                {
                    OnPropertyChanged(nameof(SimulationData));
                    _simulationData = value;
                }

            }
        }

        #endregion

        public SimulationModel_1(Random random) : base(random)
        {
            simulationS1 = new S1(random);
            simulationS1.ReplicationFinished += SimulationS1_ReplicationFinished;
        }

      
        public override event EventHandler<string[]> SimulationReplicationFinished;

        public override void ResumeSimulation()
        {
            simulationS1.OnResumeClick();
        }

        public override void SimulationFinished()
        {
            throw new NotImplementedException();
        }

        public override void StartSimulation(int replicationCount)
        {
            simulationS1.Simulate(replicationCount);
        }

        public override void PauseSimulation()
        {
            simulationS1.OnPauseClick();
        }

        public override void SimulationS1_ReplicationFinished(object sender, string[] e)
        {
            OnSimulationReplicationFinished(e);
            SimulationData = e;
        }

        protected virtual void OnSimulationReplicationFinished(string[] e)
        {
            SimulationReplicationFinished?.Invoke(this, e);
        }
    }
}
