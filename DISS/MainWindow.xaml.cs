using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
using DISS.SimulationPages;
using LiveCharts;
using LiveCharts.Defaults;
using LiveCharts.Wpf;
using LiveCharts.Geared;


namespace DISS
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private string PlayData =
            "m 17.34375,7.78125 0,16.90625 0,16.9375 L 30.875,33.21875 44.53125,24.6875 30.875,16.1875 17.34375,7.78125 z";

        private string PauseData =
            "M282.856,0H169.714c-31.228,0-56.571,25.344-56.571,56.571v678.857c0,31.228,25.344,56.571," +
            "56.571,56.571h113.143 " +
            "c31.256,0,56.572-25.315,56.572-56.571V56.571C339.428,25.344,314.112,0,282.856,0z " +
            "M622.285,0H509.143 c-31.256,0-56.572,25.344-56.572,56.571v678.857c0,31.228,25.316,56.571,56.572," +
            "56.571h113.143 c31.256,0,56.572-25.315,56.572-56.571V56.571C678.857,25.344,653.541,0,622.285,0z";


        public GearedValues<ObservableValue> ChartValues { get; set; }

        private Page_S1 page_S1 = new Page_S1();
        public MainWindow()
        {
            InitializeComponent();
            DataContext = this;

            ChartValues = new GearedValues<ObservableValue>();

            Frame_Simulation.Content = page_S1;
            page_S1.SimulationReplicationFinished += Page_S1_SimulationReplicationFinished;
            ChartValues.WithQuality(Quality.Low);

        }

        private void Page_S1_SimulationReplicationFinished(object sender, string[] e)
        {
            ChartValues.Add(new ObservableValue(Convert.ToDouble(e[0].Replace('.', ','))));

            Dispatcher.Invoke(() =>
            {
                Chart_Line.Values = ChartValues;
            });
        }

        private void StartSimulation_Click(object sender, MouseButtonEventArgs e)
        {
            if (Play_Button.Tag.Equals("Pause"))
            {
                page_S1.StartSimulation();
                Play_Button.Tag = "Play";
                page_S1.Started = true;
                PlayButtonData.Data = Geometry.Parse(PauseData);
               
            }
            else
            {
                page_S1.PauseSimulation();
                Play_Button.Tag = "Pause";
                PlayButtonData.Data = Geometry.Parse(PlayData);
            }
        }

        private void StopSimulation_Click(object sender, MouseButtonEventArgs e)
        {
            if (page_S1.Started)
            {
                page_S1.StopSimulation();
                Play_Button.Tag = "Pause";
                Play_Button.IsEnabled = false;
                PlayButtonData.Data = Geometry.Parse(PlayData);
            }
        }

        private void RefreshSimulation_Click(object sender, MouseButtonEventArgs e)
        {
            if (page_S1.Started)
            {
                page_S1.StopSimulation();
                ChartValues.Clear();
                Play_Button.Tag = "Pause";
                PlayButtonData.Data = Geometry.Parse(PlayData);
            }

            Play_Button.IsEnabled = true;
        }
    }
}
