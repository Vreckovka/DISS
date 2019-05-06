using System;
using System.Collections.Generic;
using System.Linq;
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
using PropertyChanged;
using simulation;

namespace S3.Pages
{
    /// <summary>
    /// Interaction logic for LiveSimulation_Page.xaml
    /// </summary>
    

    [AddINotifyPropertyChangedInterface]
    public partial class LiveSimulation_Page : Page
    {

        public LiveSimulation_Page()
        {
            InitializeComponent();
            DataContext = SimWrapper.SimulationModel;
        }

        private void StartClick(object sender, RoutedEventArgs e)
        {

            SimWrapper.SimulationModel.Simulation.SetSimSpeed(1 / 60d, 0.001);
            SimWrapper.SimulationModel.Simulation.OnRefreshUI(SimWrapper.SimulationModel.RefreshGui);
            MainWindow._rep_Page.DataContext = null;
            MainWindow._live_Page.DataContext = SimWrapper.SimulationModel;

            Task.Run(() =>
                    {
                      

                        if (!SimWrapper.SimulationModel.Simulation.IsRunning())
                        {
                            Task.Run(() =>
                            {
                                SimWrapper.SimulationModel.Simulation.Simulate(1); 

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

        private void StopClick(object sender, RoutedEventArgs e)
        {
            SimWrapper.SimulationModel.Simulation.StopSimulation();
            SimWrapper.SimulationModel.Simulation.Reset();
            SimWrapper.SimulationModel.SimTime = "0";
        }

        private void PauseClick(object sender, RoutedEventArgs e)
        {
            SimWrapper.SimulationModel.Simulation.PauseSimulation();
        }
    }
}
