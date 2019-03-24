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
        public GearedValues<double> ChartValues;
        private Page_S1 page_S1 = new Page_S1();
        System.Timers.Timer aTimer = new System.Timers.Timer();
        private int _removeNIteration;
        Func<double, string> _XAxisFormatter;
        TimeSpan _simulationTime;

        #region Properties
        public TimeSpan SimulationTime
        {
            get { return _simulationTime; }
            set
            {
                _simulationTime = value;
                OnPropertyChanged(nameof(SimulationTime));
            }
        }

        public Func<double, string> XAxisFormatter
        {
            get { return _XAxisFormatter; }
            set
            {
                _XAxisFormatter = value;
                OnPropertyChanged(nameof(XAxisFormatter));
            }
        }

        public Page_S1 Simulation
        {
            get { return page_S1; }
            set
            {
                OnPropertyChanged(nameof(Simulation));
                page_S1 = value;
            }
        }

        public int ActualIteration
        {
            get { return acutalIteration; }
            set
            {
                acutalIteration = value;
                OnPropertyChanged(nameof(ActualIteration));
            }

        }

        #endregion

        public Page_LiveSimulation()
        {
            InitializeComponent();
            DataContext = this;

            aTimer.Elapsed += new ElapsedEventHandler(OnTimedEvent);
            aTimer.Interval = 1000;

            ChartValues = new GearedValues<double>();
            ChartValues.WithQuality(Quality.Low);

            Frame_Simulation.Content = page_S1;
            page_S1.SimulationModel.McSimulation.ReplicationFinished += SimulationModel_SimulationReplicationFinished;
            page_S1.SimulationModel.McSimulation.SimulationFinished += Simulation_SimulationFinished;
            Chart_Line.Values = ChartValues;

            _removeNIteration = ConvertToInt(TextBox_RemoveNIteration.Text);
            XAxisFormatter = XAxisLabelFormatter;
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

        List<double> values = new List<double>();
        double oldVal;
        double deltas;
        int deltaCount;
        bool _chartScrolled;
        private double everyNIteration = 500;

        private void SimulationModel_SimulationReplicationFinished(object sender, double[] e)
        {
            if (acutalIteration % 100000 == 0 && !_chartScrolled)
            {
                if (ActualIteration > _removeNIteration && deltaCount == 0)
                {
                    oldVal = e[3];
                    deltaCount++;
                }
                else if (ActualIteration > 0)
                {
                    Dispatcher.Invoke(() =>
                    {
                        double delta = Math.Abs(oldVal - e[3]) * 0.04;

                        deltas += delta;
                        var a = (deltas / deltaCount);

                        YAxis.MaxValue = e[3] * (1 + a);
                        YAxis.MinValue = e[3] * (1 - a);

                        oldVal = e[3];
                        deltaCount++;
                    });
                }

            }
            //Remove noise
            if (ActualIteration > _removeNIteration)
            {
                if (values.Count >= 50)
                {
                    values.AsGearedValues();
                    ChartValues.AddRange(values);
                    values.Clear();
                }
                else
                {
                    if (ActualIteration % everyNIteration == 0)
                    {
                        values.Add(e[3]);
                    }
                }
            }

            ActualIteration++;
        }

        /// <summary>
        /// Solving issue with tab control, Chart starts freezing after switchin tab
        /// </summary>
        public void CreateChart()
        {
            Grid_Chart.Children.Remove(Chart);

            YAxis = new Axis()
            {
                Foreground = (Brush)Application.Current.Resources["DefaultWhiteBrush"],
                Title = "Time (minutes)",
                FontSize = 15,
                Separator = new LiveCharts.Wpf.Separator()
                {
                    Style = (Style)Application.Current.Resources["CleanSeparator"],
                }
            };

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
                        },
                      LabelFormatter = XAxisLabelFormatter
                    }
                },
                AxisY = new AxesCollection()
                {
                   YAxis
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
                    Title = "A-B-C-D-E",
                }
            };

            Chart.Series = series;
            Grid_Chart.Children.Add(Chart);

        }

        private string XAxisLabelFormatter(double a)
        {
            return (a * everyNIteration).ToString("N0");
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
                    page_S1.StartSimulation(GetRandom(), ConvertToInt(TextBox_NumberOfIteration.Text));

                }
                else
                {
                    page_S1.ResumeSimulation();
                }

                aTimer.Enabled = true;
                Play_Button.Tag = "Play";
            }
            else
            {
                aTimer.Enabled = false;
                page_S1.PauseSimulation();
                Play_Button.Tag = "Pause";
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
            }
        }

        private void RefreshSimulation_Click(object sender, MouseButtonEventArgs e)
        {
            if (page_S1.SimulationRunning)
            {
                page_S1.StopSimulation();
                page_S1.ResumeSimulation();
                aTimer.Enabled = true;
                page_S1.StartSimulation(GetRandom(), ConvertToInt(TextBox_NumberOfIteration.Text));

                Play_Button.Tag = "Play";
            }
            else
            {
                Play_Button.IsEnabled = true;
                Play_Button.Tag = "Pause";
            }


            ChartValues.Clear();
            values.Clear();

            SimulationTime = new TimeSpan(0, 0, 0);
            ActualIteration = 0;
            ResetChart();

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

                _removeNIteration = ConvertToInt(TextBox_RemoveNIteration.Text);
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
            while (true)
            {
                if (!Int32.TryParse(text, out var number))
                {
                    text = text.Remove(text.Length - 1);
                }
                else
                    return number;
            }
        }

        private void ResetButt_Click(object sender, RoutedEventArgs e)
        {
            _chartScrolled = false;
            ResetChart();
        }

        private void ResetChart()
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
            page_S1.SimulationModel.SetSimulationSpeed(100 - (int)Slider_SimulationSpeed.Value);
        }

        private void Chart_MouseWheel(object sender, MouseWheelEventArgs e)
        {
            _chartScrolled = true;
            ResetChart();
        }
    }
}
