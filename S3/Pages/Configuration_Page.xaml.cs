using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
using PropertyChanged;
using Simulations.UsedSimulations.S3;
using Simulations.UsedSimulations.S3.entities;

namespace S3.Pages
{
    /// <summary>
    /// Interaction logic for Configuration_Page.xaml
    /// </summary>

    [AddINotifyPropertyChangedInterface]
    public partial class Configuration_Page : Page
    {
        public List<MyLinka> Linky { get; set; }
     
        public Configuration_Page()
        {
            InitializeComponent();
            DataContext = this;

            Linky = new List<MyLinka>()
            {
                new MyLinka(),
                new MyLinka(),
                new MyLinka()
            };


            var autobus = new Autobus(SimWrapper.SimulationModel.Simulation, VelkostAutobusu.K186D4,
                SimWrapper.SimulationModel.Simulation.AgentOkolia.Linky[0])
            {
                CasZaciatkuJazdy = 0
            };

            Linky[0].Autobusy.Add(autobus);

            MyConfiguration.Configuration.Autobusy.Add(autobus);

        }

        private void PridajA(object sender, RoutedEventArgs e)
        {
            var autobus = new Autobus(SimWrapper.SimulationModel.Simulation, VelkostAutobusu.K186D4,
                SimWrapper.SimulationModel.Simulation.AgentOkolia.Linky[0]);

            Linky[0].Autobusy.Add(autobus);

            MyConfiguration.Configuration.Autobusy.Add(autobus);
        }


        private void PridajB(object sender, RoutedEventArgs e)
        {
            var autobus = new Autobus(SimWrapper.SimulationModel.Simulation, VelkostAutobusu.K186D4,
                SimWrapper.SimulationModel.Simulation.AgentOkolia.Linky[1]);

            Linky[1].Autobusy.Add(autobus);

            MyConfiguration.Configuration.Autobusy.Add(autobus);
        }

        private void PridajC(object sender, RoutedEventArgs e)
        {
            var autobus = new Autobus(SimWrapper.SimulationModel.Simulation, VelkostAutobusu.K186D4,
                SimWrapper.SimulationModel.Simulation.AgentOkolia.Linky[2]);

            Linky[2].Autobusy.Add(autobus);

            MyConfiguration.Configuration.Autobusy.Add(autobus);
        }

        private void Unchecked(object sender, RoutedEventArgs e)
        {
            MyConfiguration.Configuration.Cakanie = false;
        }

        private void Checked(object sender, RoutedEventArgs e)
        {
            MyConfiguration.Configuration.Cakanie = true;
        }
    }


    [AddINotifyPropertyChangedInterface]
    public class MyLinka
    {
        public ObservableCollection<Autobus> Autobusy { get; set; } = new ObservableCollection<Autobus>();

        public MyLinka()
        {
            Autobusy.CollectionChanged += Autobusy_CollectionChanged;
        }

        private void Autobusy_CollectionChanged(object sender,
            System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            if (e.OldItems != null)
            {
                foreach (Autobus item in e.OldItems)
                {
                    MyConfiguration.Configuration.Autobusy.Remove(item);
                }
            }
        }
    }

    public static class MyConfiguration
    {
        public static Configuration Configuration { get; set; } = new Configuration(113.1, true);
    }
}
