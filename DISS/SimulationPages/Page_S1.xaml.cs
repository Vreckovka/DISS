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
    public partial class Page_S1 : Page
    {
        public SimulationModel simulationModel { get; }
        public bool SimulationRunning { get; private set; }
        private Thread _simulationThread;
        public Page_S1()
        {
            InitializeComponent();

            simulationModel = new SimulationModel_1();
            DataContext = simulationModel;
        }

        public void StartSimulation(Random random, int replicationCount)
        {
            _simulationThread = new Thread(() =>
            {
                SimulationRunning = true;
                simulationModel.StartSimulation(random,replicationCount);
            });

            _simulationThread.IsBackground = true;
            _simulationThread.Start();
        }

        public void StopSimulation()
        {
            _simulationThread.Abort();
            _simulationThread.Join();

            SimulationRunning = false;
        }
        public void PauseSimulation()
        {
            simulationModel.PauseSimulation();
        }

        public void ResumeSimulation()
        {
            simulationModel.ResumeSimulation();
        }
    }
}
