using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using DISS.Annotations;
using Simulation.Simulations;
using DISS.SimulationModels;

namespace DISS.SimulationPages
{
    /// <summary>
    /// Interaction logic for S1.xaml
    /// </summary>
    public partial class Page_S1 : Page, INotifyPropertyChanged
    {
        public SimulationModel SimulationModel { get; private set; }
        private bool _simulationRunning;

        public bool SimulationRunning
        {
            get { return _simulationRunning; }
            private set
            {
                _simulationRunning = value;
                OnPropertyChanged(nameof(SimulationRunning));

            }
        }

        private Thread _simulationThread;
        public Page_S1()
        {
            InitializeComponent();

            SimulationModel = new SimulationModel_1();
            DataContext = SimulationModel;
        }

        public void StartSimulation(Random random, int replicationCount, int fireEveryNIteration)
        {
            SimulationRunning = true;

            _simulationThread = new Thread(() =>
            {
                SimulationModel.StartSimulation(random, replicationCount, fireEveryNIteration);
            })
            {
                IsBackground = true
            };

            _simulationThread.Start();
        }

        public void StartRuns(int runsCount, int replicationCount)
        {
            SimulationRunning = true;

            _simulationThread = new Thread(() =>
            {
                    SimulationModel.StartRuns(runsCount, replicationCount);
            })
            {
                IsBackground = true
            };

            _simulationThread.Start();
        }

        public void StopSimulation()
        {
            SimulationRunning = false;

            if (_simulationThread != null)
            {
                _simulationThread.Abort();
                _simulationThread.Join();
            }
        }
        public void PauseSimulation()
        {
            SimulationModel.PauseSimulation();
        }

        public void ResumeSimulation()
        {
            SimulationModel.ResumeSimulation();
        }

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
