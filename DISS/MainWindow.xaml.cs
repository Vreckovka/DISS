using System;
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
using DISS.SimulationModels;


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


        public GearedValues<ObservableValue> ChartValues = new GearedValues<ObservableValue>();
        private static Random random = new Random();
        private Page_S1 page_S1 = new Page_S1(random);

        public MainWindow()
        {
            InitializeComponent();
            DataContext = page_S1;
            ChartValues.WithQuality(Quality.Low);

            Frame_Simulation.Content = page_S1;
            page_S1.simulationModel.SimulationReplicationFinished += SimulationModel_SimulationReplicationFinished;
        }

        private void SimulationModel_SimulationReplicationFinished(object sender, string[] e)
        {
            ChartValues.Add(new ObservableValue(Convert.ToDouble(e[3].Replace('.', ','))));

            try
            {
                Dispatcher.Invoke(() =>
                 {
                     Chart_Line.Values = ChartValues;
                 });
            }
            catch (Exception)
            {
            }
        }

        private void StartSimulation_Click(object sender, MouseButtonEventArgs e)
        {
            if (Play_Button.Tag.Equals("Pause") && ConvertToInt(TextBox_NumberOfIteration.Text) != 0)
            {
               
                if (!page_S1.SimulationRunning)
                {
                    page_S1.ResumeSimulation();
                    page_S1.StartSimulation(random, ConvertToInt(TextBox_NumberOfIteration.Text));
                }
                else
                {
                    page_S1.ResumeSimulation();
                }

                Play_Button.Tag = "Play";
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
            if (page_S1.SimulationRunning)
            {
                page_S1.StopSimulation();
                Play_Button.Tag = "Pause";
                Play_Button.IsEnabled = false;
                PlayButtonData.Data = Geometry.Parse(PlayData);
            }
        }

        private void RefreshSimulation_Click(object sender, MouseButtonEventArgs e)
        {
            if (page_S1.SimulationRunning)
            {
                page_S1.StopSimulation();
                page_S1.ResumeSimulation();
                page_S1.StartSimulation(random, ConvertToInt(TextBox_NumberOfIteration.Text));

            }
            else
            {
                Play_Button.IsEnabled = true;
                Play_Button.Tag = "Pause";
                PlayButtonData.Data = Geometry.Parse(PlayData);
            }

            ChartValues.Clear();
        }

        private void TextBox_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key != Key.Left && e.Key != Key.Right)
            {
                int number = 0;
               try
                {
                    number = ConvertToInt(TextBox_NumberOfIteration.Text);

                    if (number != 0)
                        TextBox_NumberOfIteration.Text = number.ToString("N0");
                    else
                        TextBox_NumberOfIteration.Text = "";

                    TextBox_NumberOfIteration.CaretIndex = TextBox_NumberOfIteration.Text.Length;

                }
                catch (OverflowException)
                {
                    var text = TextBox_NumberOfIteration.Text;
                    text = Regex.Replace(text, @"\s+", "");
                    Regex digitsOnly = new Regex(@"[^\d]");
                    text = digitsOnly.Replace(text, "");

                    number = ValidTextToInt(text);
                    TextBox_NumberOfIteration.Text = number.ToString("N0");
                    TextBox_NumberOfIteration.CaretIndex = TextBox_NumberOfIteration.Text.Length;
                }
            }
        }

        private int ConvertToInt(string text)
        {
            text = Regex.Replace(text, @"\s+", "");
            Regex digitsOnly = new Regex(@"[^\d]");
            text = digitsOnly.Replace(text, "");

            if (text != "")
                return Convert.ToInt32(text);
            else
                return 0;
        }

        private int ValidTextToInt(string text)
        {
            int number;
            while (true)
            {
                if (!Int32.TryParse(text, out number))
                {
                    text = text.Remove(text.Length - 1);
                }
                else
                    return number;
            }
        }

        private void ResetButt_Click(object sender, RoutedEventArgs e)
        {
            Chart.AxisX[0].MinValue = double.NaN;
            Chart.AxisX[0].MaxValue = double.NaN;
            Chart.AxisY[0].MinValue = double.NaN;
            Chart.AxisY[0].MaxValue = double.NaN;
        }
    }
}
