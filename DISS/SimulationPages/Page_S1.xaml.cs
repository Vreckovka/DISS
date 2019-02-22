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

namespace DISS.SimulationPages
{
    /// <summary>
    /// Interaction logic for S1.xaml
    /// </summary>
    public partial class Page_S1 : Page, INotifyPropertyChanged
    {
        private Random random = new Random();
        private S1 simulationS1;
        public event EventHandler<string[]> SimulationReplicationFinished;

        private string _ABCDE;
        private string _AFHDE;
        private string _AFGE;

        public bool Started { get; set; }

        private Thread simulationThread;
        public string ABCDE
        {
            get { return _ABCDE; }
            set
            {
                if (value != _ABCDE)
                {
                    OnPropertyChanged(nameof(ABCDE));
                    _ABCDE = value;
                }

            }
        }

        public string AFHDE
        {
            get { return _AFHDE; }
            set
            {
                if (value != _AFHDE)
                {
                    OnPropertyChanged(nameof(AFHDE));
                    _AFHDE = value;
                }

            }
        }

        public string AFGE
        {
            get { return _AFGE; }
            set
            {
                if (value != _AFGE)
                {
                    OnPropertyChanged(nameof(AFGE));
                    _AFGE = value;
                }

            }
        }

        public Page_S1()
        {
            InitializeComponent();
            DataContext = this;

            simulationS1 = new S1(100000, random);
            simulationS1.ReplicationFinished += SimulationS1_ReplicationFinished;

            simulationThread = new Thread(() =>
            {
                simulationS1.Simulate();
            });

            simulationThread.IsBackground = true;

        }

        private void SimulationS1_ReplicationFinished(object sender, string[] e)
        {
            OnSimulationReplicationFinished(e);
            ABCDE = e[1];
            AFHDE = e[2];
            AFGE = e[3];
        }

        protected virtual void OnSimulationReplicationFinished(string[] e)
        {
            SimulationReplicationFinished?.Invoke(this, e);

        }

        public void StartSimulation()
        {
            if (!simulationThread.IsAlive)
                if (simulationThread.ThreadState == ThreadState.Aborted)
                {
                    simulationThread = new Thread(() =>
                    {
                        simulationS1.Simulate();
                    });

                    simulationThread.IsBackground = true;
                    simulationThread.Start();
                }
                else
                    simulationThread.Start();
            else
                simulationThread.Resume();
        }

        public void PauseSimulation()
        {
            simulationThread.Suspend();
        }

        public void StopSimulation()
        {
            try
            {
                if (simulationThread.ThreadState != ThreadState.Suspended &&
                       simulationThread.ThreadState != ThreadState.AbortRequested)
                    simulationThread.Abort();
            }
            catch (Exception)
            {
                MessageBox.Show("AHoj");
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
