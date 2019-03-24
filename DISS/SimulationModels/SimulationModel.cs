using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using DISS.Annotations;
using Simulation;

namespace DISS.SimulationModels
{
    public abstract class SimulationModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        public BaseMCSimulation McSimulation { get; set; }
        public SimulationModel()
        {
        }

        public abstract void SetSimulationSpeed(int speed);
        public abstract void ResumeSimulation();
        public abstract void SimulationFinished();
        public abstract double[] StartSimulation(Random random,int replicationCount, int fireEveryNIteration);
        public abstract void StartRuns (int runsCount, int replicationCount);
        public abstract void PauseSimulation();
        public abstract void SimulationS1_ReplicationFinished(object sender, double[] e);

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
