using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Timers;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using DISS.Annotations;
using DISS.SimulationPages;
using LiveCharts;
using LiveCharts.Configurations;
using LiveCharts.Defaults;
using LiveCharts.Geared;
using LiveCharts.Wpf;

namespace DISS.WindowsPages
{
    /// <summary>
    /// Interaction logic for Page_LiveSimulation.xaml
    /// </summary>
    public partial class Page_LiveSimulation : Page, INotifyPropertyChanged
    {
        private string PlayData =
            "m 17.34375,7.78125 0,16.90625 0,16.9375 L 30.875,33.21875 44.53125,24.6875 30.875,16.1875 17.34375,7.78125 z";

        private string PauseData =
            "M282.856,0H169.714c-31.228,0-56.571,25.344-56.571,56.571v678.857c0,31.228,25.344,56.571," +
            "56.571,56.571h113.143 " +
            "c31.256,0,56.572-25.315,56.572-56.571V56.571C339.428,25.344,314.112,0,282.856,0z " +
            "M622.285,0H509.143 c-31.256,0-56.572,25.344-56.572,56.571v678.857c0,31.228,25.316,56.571,56.572," +
            "56.571h113.143 c31.256,0,56.572-25.315,56.572-56.571V56.571C678.857,25.344,653.541,0,622.285,0z";


        public GearedValues<ObservableValue> ChartValues;
        private Page_S1 page_S1 = new Page_S1();
        System.Timers.Timer aTimer = new System.Timers.Timer();

        TimeSpan _simulationTime;
        public TimeSpan SimulationTime
        {
            get { return _simulationTime; }
            set
            {
                OnPropertyChanged(nameof(SimulationTime));
                _simulationTime = value;
            }
        }
        public Page_LiveSimulation()
        {
            InitializeComponent();
            DataContext = this;

            aTimer.Elapsed += new ElapsedEventHandler(OnTimedEvent);
            aTimer.Interval = 1000;

            ChartValues = new GearedValues<ObservableValue>();
            ChartValues.WithQuality(Quality.Low);

            Frame_Simulation.Content = page_S1;
            page_S1.simulationModel.Simulation.ReplicationFinished += SimulationModel_SimulationReplicationFinished;
            page_S1.simulationModel.Simulation.SimulationFinished += Simulation_SimulationFinished;
            Chart_Line.Values = ChartValues;
        }

        private void Simulation_SimulationFinished(object sender, double[] e)
        {
            aTimer.Enabled = false;
        }

        int acutalIteration;

        private void OnTimedEvent(object source, ElapsedEventArgs e)
        {
            SimulationTime = new TimeSpan(SimulationTime.Hours, SimulationTime.Minutes, SimulationTime.Seconds + 1);
        }
        public int ActualIteration
        {
            get { return acutalIteration; }
            set
            {
                OnPropertyChanged(nameof(ActualIteration));
                acutalIteration = value;
            }
        }

        List<ObservableValue> values = new List<ObservableValue>();
        int speed;
        private void SimulationModel_SimulationReplicationFinished(object sender, double[] e)
        {
            //ChartValues.Add(new ObservableValue(e[3]));
            if (values.Count >= (int)speed / 2)
            {
                values.AsGearedValues();
                ChartValues.AddRange(values);
                values.Clear();
            }
            else
            {
                values.Add(new ObservableValue(e[3]));
            }

            ActualIteration++;
        }

        /// <summary>
        /// Solving issue with tab control, Chart starts freezing after switchin tab
        /// </summary>
        public void CreateChart()
        {
            Grid_Chart.Children.Remove(Chart);

            Chart = new CartesianChart()
            {
                Background = null,
                Hoverable = false,
                DataTooltip = null,
                DisableAnimations = true,
                LegendLocation = LegendLocation.Right,
                Zoom = ZoomingOptions.X,
                Pan = PanningOptions.X,
                Margin = new Thickness(10),
                AxisX = new AxesCollection()
                {
                    new Axis()
                     {
                        Foreground = (Brush)Application.Current.Resources["DefaultWhiteBrush"],
                        Title = "Number of iteration",
                        Position = AxisPosition.LeftBottom,
                        FontSize = 15,
                        Separator = new LiveCharts.Wpf.Separator()
                        {
                           Style = (Style)Application.Current.Resources["CleanSeparator"],
                        }
                    }
                },
                AxisY = new AxesCollection()
                {
                    new Axis()
                    {
                        Foreground = (Brush)Application.Current.Resources["DefaultWhiteBrush"],
                        Title = "Time (minutes)",
                        FontSize = 15,
                        Separator = new LiveCharts.Wpf.Separator()
                        {
                            Style = (Style)Application.Current.Resources["CleanSeparator"],
                        }
                    }
                }
            };

            Grid.SetRow(Chart, 1);

            SeriesCollection series = new SeriesCollection()
            {
                new GLineSeries()
                {
                    Stroke = (Brush) Application.Current.Resources["DefaultRedBrush"],
                    Values = ChartValues,
                    Fill = Brushes.Transparent,
                    PointGeometry = null,
                    LineSmoothness = 0,
                    Title = "A-F-G-E",
                }
            };

            Chart.Series = series;
            Grid_Chart.Children.Add(Chart);

        }
        private Random GetRandom()
        {
            if (CheckBox_RandomSeed.IsChecked == true)
                return new Random();
            else
                return new Random(ConvertToInt(TextBox_Seed.Text));
        }
        private void StartSimulation_Click(object sender, MouseButtonEventArgs e)
        {
            if (Play_Button.Tag.Equals("Pause") && ConvertToInt(TextBox_NumberOfIteration.Text) != 0)
            {
                if (!page_S1.SimulationRunning)
                {
                    page_S1.ResumeSimulation();
                    aTimer.Enabled = true;

                    page_S1.StartSimulation(GetRandom(), ConvertToInt(TextBox_NumberOfIteration.Text));

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
                aTimer.Enabled = false;
                page_S1.PauseSimulation();
                Play_Button.Tag = "Pause";
                PlayButtonData.Data = Geometry.Parse(PlayData);
            }
        }

        private void StopSimulation_Click(object sender, MouseButtonEventArgs e)
        {
            if (page_S1.SimulationRunning)
            {
                aTimer.Enabled = false;
                page_S1.StopSimulation();
                Play_Button.Tag = "Pause";
                Play_Button.IsEnabled = false;
                PlayButtonData.Data = Geometry.Parse(PlayData);
            }
        }

        private  void RefreshSimulation_Click(object sender, MouseButtonEventArgs e)
        {
            if (page_S1.SimulationRunning)
            {
                page_S1.StopSimulation();
                page_S1.ResumeSimulation();
                aTimer.Enabled = true;
                 page_S1.StartSimulation(GetRandom(), ConvertToInt(TextBox_NumberOfIteration.Text));
            }
            else
            {
                Play_Button.IsEnabled = true;
                Play_Button.Tag = "Pause";
                PlayButtonData.Data = Geometry.Parse(PlayData);
            }

            SimulationTime = new TimeSpan(0, 0, 0);
            ActualIteration = 0;

            ChartValues.Clear();
        }

        private void TextBox_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key != Key.Left && e.Key != Key.Right)
            {
                int number = 0;
                try
                {
                    number = ConvertToInt(((TextBox)sender).Text);

                    if (number != 0)
                        ((TextBox)sender).Text = number.ToString("N0");
                    else
                        ((TextBox)sender).Text = "";

                    ((TextBox)sender).CaretIndex = TextBox_NumberOfIteration.Text.Length;

                }
                catch (OverflowException)
                {
                    var text = ((TextBox)sender).Text;
                    text = Regex.Replace(text, @"\s+", "");
                    Regex digitsOnly = new Regex(@"[^\d]");
                    text = digitsOnly.Replace(text, "");

                    number = ValidTextToInt(text);
                    ((TextBox)sender).Text = number.ToString("N0");
                    ((TextBox)sender).CaretIndex = ((TextBox)sender).Text.Length;
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


        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

        }

        private void Slider_SimulationSpeed_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            page_S1.simulationModel.SetSimulationSpeed(101 - (int)Slider_SimulationSpeed.Value);
            speed = (int)Slider_SimulationSpeed.Value;
        }
    }
}
