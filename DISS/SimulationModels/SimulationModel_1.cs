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
        private double[] _simulationData;
        private S1 simulationS1;

        #region Properties
      
        public double[] SimulationData
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

        public SimulationModel_1() : base()
        {
            simulationS1 = new S1();
            simulationS1.ReplicationFinished += SimulationS1_ReplicationFinished;
            Simulation = simulationS1;
        }

        public override void SetSimulationSpeed(int speed)
        {
            simulationS1.SimulationDelay = speed;
        }

        public override void ResumeSimulation()
        {
            simulationS1.OnResumeClick();
        }

        public override void SimulationFinished()
        {
            throw new NotImplementedException();
        }

        public override double[] StartSimulation(Random random,int replicationCount)
        {
            return simulationS1.Simulate(random,replicationCount, true);
        }

        public override void StartRuns(int runsCount, int replicationCount)
        {
            simulationS1.SimulateRuns(runsCount, replicationCount);
        }

        public override void PauseSimulation()
        {
            simulationS1.OnPauseClick();
        }

        public override void SimulationS1_ReplicationFinished(object sender, double[] e)
        {
            SimulationData = e;
        }
    }
}
