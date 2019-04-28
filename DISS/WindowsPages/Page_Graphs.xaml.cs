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
using LiveCharts;
using LiveCharts.Defaults;
using PropertyChanged;
using Simulations.UsedSimulations.S2;

namespace DISS.WindowsPages
{
    /// <summary>
    /// Interaction logic for Page_Graphs.xaml
    /// </summary>
    [AddINotifyPropertyChangedInterface]
    public partial class Page_Graphs : Page
    {
        public ChartValues<ObservablePoint> CooksChartValues { get; set; } = new ChartValues<ObservablePoint>();
        public ChartValues<ObservablePoint> WaiterChartValues { get; set; } = new ChartValues<ObservablePoint>();
        public int NumberOfReplications { get; set; } = 1000;
        public Page_Graphs()
        {
            InitializeComponent();
            DataContext = this;
        }

        public async void GetWaitersData()
        {
            List<Task<ObservablePoint>> tasks = new List<Task<ObservablePoint>>();

            for (int i = 1; i <= 5; i++)
            {
                int number = i;
                Task<ObservablePoint> task = new Task<ObservablePoint>(() => GetWaiters(number));
                tasks.Add(task);
                task.Start();
            }

            await Task.WhenAll(tasks);

            foreach (Task<ObservablePoint> task in tasks)
            {
                WaiterChartValues.Add(task.Result);
            }
        }

        public async void GetCooksData()
        {
            List<Task<ObservablePoint>> tasks = new List<Task<ObservablePoint>>();

            for (int i = 10; i <= 20; i++)
            {
                int number = i;
                Task<ObservablePoint> task = new Task<ObservablePoint>(() => GetCooks(number));
                tasks.Add(task);
                task.Start();
            }

            await Task.WhenAll(tasks);

            foreach (Task<ObservablePoint> task in tasks)
            {
                CooksChartValues.Add(task.Result);
            }
        }

        private ObservablePoint GetWaiters(int numberOfWaiters)
        {
            var start = new TimeSpan(11, 0, 0);
            var end = new TimeSpan(20, 0, 0);

            var simulation = new SimulationCore_S2();

            var result = simulation.SimulateRuns(start, end, numberOfWaiters, 14, false, NumberOfReplications);

            return new ObservablePoint()
            {
                X = numberOfWaiters,
                Y = result[0]
            }; ;
        }

        private ObservablePoint GetCooks(int numberOfCooks)
        {
            var start = new TimeSpan(11, 0, 0);
            var end = new TimeSpan(20, 0, 0);

            var simulation = new SimulationCore_S2();

            var result = simulation.SimulateRuns(start, end, 4, numberOfCooks, false, NumberOfReplications);

            return new ObservablePoint()
            {
                X = numberOfCooks,
                Y = result[0]
            }; ;
        }

        private void Simulate_Click(object sender, RoutedEventArgs e)
        {
            GetWaitersData();
            GetCooksData();
        }

        private void Reset_Click(object sender, RoutedEventArgs e)
        {
            CooksChartValues = new ChartValues<ObservablePoint>();
            WaiterChartValues = new ChartValues<ObservablePoint>();
        }
    }
}
