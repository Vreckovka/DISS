﻿using System;
using System.CodeDom;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Timers;
using System.Windows.Controls;
using System.Windows;
using System.Windows.Interop;
using System.Windows.Navigation;
using System.Windows.Threading;
using DISS.WindowsPages;
using Simulations.UsedSimulations.S2;


namespace DISS
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private Page_LiveSimulation page_LiveSimulation = new Page_LiveSimulation();
        private Page_SimulationRuns page_SimulationRuns = new Page_SimulationRuns();
        private Page_Graphs page_Graphs = new Page_Graphs();

        public MainWindow()
        {
            InitializeComponent();
            Frame_Simulation.Content = page_LiveSimulation;
        }


        private void ListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (IsLoaded)
            {
                if (((ListView)sender).SelectedItem == ((ListView)sender).Items[0])
                {
                    page_LiveSimulation.CreateChart();
                    Frame_Simulation.Content = page_LiveSimulation;
                }
                else if (((ListView)sender).SelectedItem == ((ListView)sender).Items[1])
                {
                    Frame_Simulation.Content = page_SimulationRuns;
                }
                else
                {
                    Frame_Simulation.Content = page_Graphs;
                }
            }
        }
    }
}
