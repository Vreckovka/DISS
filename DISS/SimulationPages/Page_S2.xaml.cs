using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Globalization;
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
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using DISS.Annotations;
using DISS.SimulationModels;
using OxyPlot;
using PropertyChanged;
using Simulations.ConfidenceInterval;
using Simulations.UsedSimulations.Other;
using Simulations.UsedSimulations.S2;

namespace DISS.SimulationPages
{
    /// <summary>
    /// Interaction logic for Page_S2.xaml
    /// </summary>
    [AddINotifyPropertyChangedInterface]
    public partial class Page_S2 : Page
    {
        private bool _simulationRunning;
        private Thread _simulationThread;
        public bool SimulationRunning;
        public SimulationModel_2 SimulationModel { get; set; }
        public Page_S2()
        {
            InitializeComponent();
            SimulationModel = new SimulationModel_2();
            SimulationModel.Simulation.SimulationFinished += Simulation_SimulationFinished;

            DataContext = this;
        }

        private void Simulation_SimulationFinished(object sender, double[] e)
        {
            Dispatcher.Invoke(() =>
            {
                DataContext = null;
                DataContext = this;
            });
        }

        public void StartSimulation(Random random, int numberOfWaiters, int numberOfCooks, bool cooling)
        {
            SimulationRunning = true;
            SimulationModel.Simulation.BeforeSimulation(new TimeSpan(11, 0, 0), new TimeSpan(20, 0, 0), numberOfWaiters, numberOfCooks, cooling);


            _simulationThread = new Thread(() =>
            {
                SimulationModel.StartSimulation(random, numberOfWaiters, numberOfCooks, cooling);
            })
            {
                IsBackground = true
            };

            _simulationThread.Start();
        }

        public void StartRuns(int runsCount, int numberOfReplications, int numberOfWaiter, int numberOfCooks, bool cooling)
        {
            SimulationRunning = true;

            _simulationThread = new Thread(() =>
            {
                SimulationModel.StartRuns(runsCount, numberOfReplications, numberOfWaiter, numberOfCooks, cooling);
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

        private void GeneratingColumns(object sender, DataGridAutoGeneratingColumnEventArgs e)
        {
            if (e.PropertyName.Equals("LastEventTime"))
            {
                e.Cancel = true;
            }
        }
    }
}
