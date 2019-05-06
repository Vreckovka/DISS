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

                if (!SimWrapper.SimulationModel.Simulation.IsRunning())
                {
                    SimWrapper.SimulationModel.Simulation.OnRefreshUI(null);
                    MainWindow._live_Page.DataContext = null;
                    MainWindow._rep_Page.DataContext = SimWrapper.SimulationModel;

                    Task.Run(() => { SimWrapper.SimulationModel.Simulation.Simulate(pocet); });
                }
                else
                    MessageBox.Show("Simulacia uz prebieha");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}
