#define Live1

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
using simulation;
using Simulations.UsedSimulations.S3;
using Simulations.UsedSimulations.S3.entities;
using PropertyChanged;




namespace S3
{
    [AddINotifyPropertyChangedInterface]
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public Action<OSPABA.Simulation> Action { get; set; }
        public MySimulation Simulation { get; set; }
        public MainWindow()
        {
            InitializeComponent();
            Simulation = new MySimulation();
#if Live
            DataContext = Simulation;
#endif
            double pocet = 0;
            double count = 0;


            double pocetHodin = 0;
            Simulation.OnReplicationDidFinish((s)
                =>
            {              
                Console.Clear();
                Console.WriteLine("Replikacia: " + s.CurrentReplication);
            });

            Simulation.OnSimulationDidFinish((s)
                =>
            {
                Vypis(s);
            });

            Action = RefreshGui;
        }

        public void Vypis(OSPABA.Simulation s)
        {
            Console.Clear();
            Console.WriteLine("Replikacia: " + (s.CurrentReplication + 1));
            Console.WriteLine($"Cas ukoncenia: {(int)TimeSpan.FromMinutes(((MySimulation)s).AvrageFinishedTime).TotalHours}:" +
                              $"{TimeSpan.FromMinutes(((MySimulation)s).AvrageFinishedTime):mm}:" +
                              $"{TimeSpan.FromMinutes(((MySimulation)s).AvrageFinishedTime):ss}");
            Console.WriteLine("Pocet ludi: " + ((MySimulation)s).AvragePocetLudi);

            Console.WriteLine("Cas cakania ludi: " + ((MySimulation)s).AvrageCakania);

        }


        public void RefreshGui(OSPABA.Simulation simulation)
        {
            var s = ((MySimulation)simulation);

            Dispatcher.InvokeAsync(() =>
            {
                SimulationTimeRun.Text = TimeSpan.FromMinutes(Simulation.CurrentTime).ToString("hh\\:mm\\:ss");
            });

            foreach (var autobus in s.AgentAutobusov.Autobusy)
            {
                if (!double.IsPositiveInfinity(autobus.ZaciatokJazdyCas) && !autobus.KoniecJazd)
                {
                    var prejdenyCas = s.CurrentTime - autobus.ZaciatokJazdyCas;
                    var prec = (prejdenyCas * 100 / autobus.AktualnaZastavka.CasKDalsejZastavke);
                    autobus.PrejdenyCas = prec;
                }
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {

            Task.Run(() =>
            {
#if Live
                Simulation.SetSimSpeed(1 / 60d, 0.001);
                Simulation.OnRefreshUI(Action);
#endif

                Simulation.Simulate(10);
            });
        }
    }
}
