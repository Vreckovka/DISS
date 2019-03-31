using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Globalization;
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
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using DISS.Annotations;
using DISS.SimulationPages;
using LiveCharts;
using LiveCharts.Configurations;
using LiveCharts.Defaults;
using LiveCharts.Wpf;
using OxyPlot;
using PropertyChanged;

namespace DISS.WindowsPages
{

    /// <summary>
    /// Interaction logic for Page_LiveSimulation.xaml
    /// </summary>
    [AddINotifyPropertyChangedInterface]
    public partial class Page_LiveSimulation : Page
    {
        public Func<double, string> XAxisFormatter { get; set; }
        public Page_S2 Page_S2 { get; set; } = new Page_S2();

        public Page_LiveSimulation()
        {
            InitializeComponent();
            DataContext = this;

            Frame_Simulation.Content = Page_S2;
            Page_S2.SimulationModel.Simulation.SimulationFinished += Simulation_SimulationFinished;

            //Chart_Line.Values = ChartValues;
            XAxisFormatter = XAxisLabelFormatter;
        }

        private void Simulation_SimulationFinished(object sender, double[] e)
        {
        }

        List<double> values = new List<double>();
        double oldVal;
        double deltas;
        int deltaCount;
        bool _chartScrolled;
        private double everyNIteration = 1;

        /// <summary>
        /// Solving issue with tab control, Chart starts freezing after switchin tab
        /// </summary>
        public void CreateChart()
        {
            //Grid_Chart.Children.Remove(Chart);

            //YAxis = new Axis()
            //{
            //    Foreground = (Brush)Application.Current.Resources["DefaultWhiteBrush"],
            //    Title = "Waiting time (seconds)",
            //    FontSize = 15,
            //    Separator = new LiveCharts.Wpf.Separator()
            //    {
            //        Style = (Style)Application.Current.Resources["CleanSeparator"],
            //    }
            //};

            //Chart = new CartesianChart()
            //{
            //    Background = null,
            //    Hoverable = false,
            //    DataTooltip = null,
            //    DisableAnimations = true,
            //    LegendLocation = LegendLocation.Right,
            //    Zoom = ZoomingOptions.X,
            //    Pan = PanningOptions.X,
            //    Margin = new Thickness(10),
            //    AxisX = new AxesCollection()
            //    {
            //        new Axis()
            //         {
            //            Foreground = (Brush)Application.Current.Resources["DefaultWhiteBrush"],
            //            Title = "Simulation time",
            //            Position = AxisPosition.LeftBottom,
            //            FontSize = 15,
            //            Separator = new LiveCharts.Wpf.Separator()
            //            {
            //               Style = (Style)Application.Current.Resources["CleanSeparator"],
            //            },
            //          LabelFormatter = XAxisLabelFormatter
            //        }
            //    },
            //    AxisY = new AxesCollection()
            //    {
            //       YAxis
            //    }
            //};

            //Grid.SetRow(Chart, 1);

            //SeriesCollection series = new SeriesCollection()
            //{
            //    new LineSeries()
            //    {
            //        Stroke = (Brush)Application.Current.Resources["DefaultRedBrush"],
            //        Values = ChartValues,
            //        Fill = Brushes.Transparent,
            //        PointGeometry = null,
            //        LineSmoothness = 1,
            //        Title = "Avrage waiting time of customer",
            //    }
            //};

            //Chart.Series = series;
            //Grid_Chart.Children.Add(Chart);

        }

        private string XAxisLabelFormatter(double a)
        {
            return TimeSpan.FromSeconds((a * 15)  + 39600).ToString();
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
            if (Play_Button.Tag.Equals("Pause") && ConvertToInt(TextBox_NumberOfCooks.Text) != 0)
            {
                if (!Page_S2.SimulationRunning)
                {
                    Page_S2.ResumeSimulation();
                    Page_S2.StartSimulation(GetRandom(), ConvertToInt(TextBox_NumberOfWaiters.Text), ConvertToInt(TextBox_NumberOfCooks.Text), Convert.ToBoolean(CheckBox_Cooling.IsChecked));

                }
                else
                {
                    Page_S2.ResumeSimulation();
                }

                Play_Button.Tag = "Play";
            }
            else
            {
                Page_S2.PauseSimulation();
                Play_Button.Tag = "Pause";
            }
        }

        private void StopSimulation_Click(object sender, MouseButtonEventArgs e)
        {
            if (Page_S2.SimulationRunning)
            {
                Page_S2.StopSimulation();
                Play_Button.Tag = "Pause";
                Play_Button.IsEnabled = false;
            }
        }

        private void RefreshSimulation_Click(object sender, MouseButtonEventArgs e)
        {
            if (Page_S2.SimulationRunning)
            {
                Page_S2.StopSimulation();
                Page_S2.ResumeSimulation();
                Page_S2.StartSimulation(GetRandom(), ConvertToInt(TextBox_NumberOfWaiters.Text), ConvertToInt(TextBox_NumberOfCooks.Text), Convert.ToBoolean(CheckBox_Cooling.IsChecked));

                Play_Button.Tag = "Play";
            }
            else
            {
                Play_Button.IsEnabled = true;
                Play_Button.Tag = "Pause";
            }

            values.Clear();
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

                    ((TextBox)sender).CaretIndex = TextBox_NumberOfCooks.Text.Length;

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

        private void Slider_SimulationSpeed_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            if (Page_S2.SimulationModel != null)
                Page_S2.SimulationModel.SetSimulationSpeed(100 - (int)Slider_SimulationSpeed.Value);
        }

        private void Chart_MouseWheel(object sender, MouseWheelEventArgs e)
        {
            _chartScrolled = true;
            ResetChart();
        }
    }

    public class SimulationTimeConverter : MarkupExtension, IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return TimeSpan.FromSeconds((double) value).ToString("hh\\:mm\\:ss");
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            return this;
        }
    }
}
