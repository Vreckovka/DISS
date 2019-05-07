using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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

namespace S3.Pages
{
    /// <summary>
    /// Interaction logic for Replication_Page.xaml
    /// </summary>
    public partial class Replication_Page : Page
    {
        public Replication_Page()
        {
            InitializeComponent();
            DataContext = SimWrapper.SimulationModel;
        }

        private void StartClick(object sender, RoutedEventArgs e)
        {
            int pocet = 0;
            try
            {
                pocet = Convert.ToInt32(PocetRep.Text);

                
                SimWrapper.SimulationModel.Simulation.SetSimSpeed(1 / 60d, 0.0001);
                SimWrapper.SimulationModel.Simulation.OnRefreshUI(SimWrapper.SimulationModel.RefreshGui);
                MainWindow._live_Page.DataContext = null;
                MainWindow._rep_Page.DataContext = SimWrapper.SimulationModel;

                Task.Run(() =>
                {
                    if (!SimWrapper.SimulationModel.Simulation.IsRunning())
                    {
                        Task.Run(() =>
                        {
                            SimWrapper.SimulationModel.Simulation.Simulate(pocet);

                        });
                    }
                    else if (SimWrapper.SimulationModel.Simulation.IsPaused())
                    {
                        SimWrapper.SimulationModel.Simulation.ResumeSimulation();
                    }
                    else
                        MessageBox.Show("Simulacia uz prebieha");
                });

              
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void PauseClick(object sender, RoutedEventArgs e)
        {
            SimWrapper.SimulationModel.Simulation.PauseSimulation();
        }

        private void StopClick(object sender, RoutedEventArgs e)
        {
            SimWrapper.SimulationModel.Simulation.StopSimulation();
            SimWrapper.SimulationModel.Simulation.Reset();
            SimWrapper.SimulationModel.SimTime = "0";
        }
    }
}
