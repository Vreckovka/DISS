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

namespace S3
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MySimulation Simulation { get; set; }
        public MainWindow()
        {
            InitializeComponent();
            Simulation = new MySimulation();

            DataContext = Simulation;
           
            Simulation.OnReplicationDidFinish((s)
                =>
            {

                Console.WriteLine("KONIEC REPLIKACIE");
                Console.Clear();
            });

            Simulation.OnSimulationDidFinish((s)
                =>
            {
                Console.WriteLine("Koniec.");
                Console.ReadKey();
            });



            //List<Zastavka> zastavkas = new List<Zastavka>();


            //zastavkas.Add(new Zastavka(Simulation)
            //{
            //    Meno = "AHOJK"
            //});

            //zastavkas.Add(new Zastavka(Simulation)
            //{
            //    Meno = "ASD"
            //});

            //Thread thread = new Thread(() =>
            //{
            //    int i = 0;
            //    while (true)
            //    {
            //        zastavkas[0].Meno = i.ToString();
            //        zastavkas[1].Meno = i.ToString();
            //        i++;

            //        Thread.Sleep(100);
            //    }
            //});

            //thread.Start();
           
            //Zastavky.ItemsSource = zastavkas;


            Task.Run(() =>
            {
                while (true)
                {
                    Thread.Sleep(250);
                    Dispatcher.Invoke(() =>
                    {
                        A.Items.Refresh();
                        B.Items.Refresh();
                        C.Items.Refresh();
                        Autobusy.Items.Refresh();
                        SimulationTimeRun.Text = TimeSpan.FromMinutes(Simulation.CurrentTime).ToString();
                    });
                }
            });
        }



        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Task.Run(() =>
            {
                Simulation.Simulate(Config.PocetReplikacii, Config.PocetReplikacii); 
            });
        }
    }
}
