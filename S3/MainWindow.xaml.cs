﻿using System;
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
        public MySimulation Simulation { get; set; }
        Thread thread;
        public MainWindow()
        {
            InitializeComponent();
            Simulation = new MySimulation();

            DataContext = Simulation;
            double pocet = 0;
            double count = 0;

            Simulation.OnReplicationDidFinish((s)
                =>
            {
                pocet += Simulation.AgentOkolia.CelkovyPocetCestujucich;
                count++;
                Console.WriteLine(pocet / count);
                // Console.Clear();
            });

            Simulation.OnSimulationDidFinish((s)
                =>
            {
                Console.WriteLine("Koniec.");
                Console.ReadKey();
            });

            thread = new Thread(() =>
            {
                while (true)
                {
                    Thread.Sleep(10);
                    Dispatcher.Invoke(() =>
                    {
                        if (!double.IsInfinity(Simulation.CurrentTime))
                            SimulationTimeRun.Text = TimeSpan.FromMinutes(Simulation.CurrentTime).ToString();

                    });
                }
            });

            thread.IsBackground = true;
        }



        private void Button_Click(object sender, RoutedEventArgs e)
        {
            thread.Start();
            Task.Run(() =>
            {
                Simulation.SetSimSpeed(1/30d, 0.01);
                Simulation.Simulate(Config.PocetReplikacii,115);
            });
        }
    }
}
