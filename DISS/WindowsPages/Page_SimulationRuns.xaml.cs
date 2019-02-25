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
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using DISS.Annotations;
using DISS.SimulationPages;

namespace DISS.WindowsPages
{
    /// <summary>
    /// Interaction logic for SimulationRuns_Page.xaml
    /// </summary>
    public partial class Page_SimulationRuns : Page, INotifyPropertyChanged
    {
        private Page_S1 page_S1 = new Page_S1();
        System.Timers.Timer aTimer = new System.Timers.Timer();
        TimeSpan _simulationTime;
        double[] _runData;
        double[] _avrageData;

        public TimeSpan SimulationTime
        {
            get { return _simulationTime; }
            set
            {
                OnPropertyChanged(nameof(SimulationTime));
                _simulationTime = value;
            }
        }

        public double[] RunData
        {
            get { return _runData; }
            set
            {
                OnPropertyChanged(nameof(RunData));
                _runData = value;
            }
        }

        public double[] AvrageData
        {
            get { return _avrageData; }
            set
            {
                OnPropertyChanged(nameof(AvrageData));
                _avrageData = value;
            }
        }

        public Page_SimulationRuns()
        {
            InitializeComponent();

            DataContext = this;

            aTimer.Elapsed += new ElapsedEventHandler(OnTimedEvent);
            aTimer.Interval = 1000;
            page_S1.simulationModel.Simulation.RunFinished += Simulation_RunFinished; ;

        }

        private void Simulation_RunFinished(object sender, double[] e)
        {
            Dispatcher.Invoke(() =>
            {
                DataGrid_SimulationRuns.Items.Add(new string[]
                {
                    DataGrid_SimulationRuns.Items.Count.ToString(),
                    "A-B-C-D-E: " + (e[0]) + "\n" +
                    "A-F-H-D-E: " + (e[1]) + "\n" +
                    "A-F-G-E: " + (e[2]),
                    "Probability: " + (e[3])

                });

                if (RunData != null)
                    RunData = new double[]
                    {
                    (RunData[0] + e[0]),
                    (RunData[1] + e[1]),
                    (RunData[2] + e[2]),
                    (RunData[3] + e[3])
                    };
                else
                    RunData = new double[]
                    {
                       e[0],
                       e[1],
                       e[2],
                       e[3]
                    };

                if (AvrageData != null)
                    AvrageData = new double[]
                    {
                        (RunData[0] / DataGrid_SimulationRuns.Items.Count),
                        (RunData[1] / DataGrid_SimulationRuns.Items.Count),
                        (RunData[2] / DataGrid_SimulationRuns.Items.Count),
                        (RunData[3] / DataGrid_SimulationRuns.Items.Count)
                    };
                else
                    AvrageData = new double[]
                    {
                        RunData[0],
                        RunData[0],
                        RunData[0],
                        RunData[0]
                    };


                if (DataGrid_SimulationRuns.Items.Count > 0)
                {
                    var border = VisualTreeHelper.GetChild(DataGrid_SimulationRuns, 0) as Decorator;
                    if (border != null)
                    {
                        var scroll = border.Child as ScrollViewer;
                        if (scroll != null) scroll.ScrollToEnd();
                    }
                }
            });
        }

        private void Start_Click(object sender, RoutedEventArgs e)
        {
            page_S1.StartRuns(ConvertToInt(TextBox_NumberOfRuns.Text),
                ConvertToInt(TextBox_NumberOfReplication.Text));
            }

        private void OnTimedEvent(object source, ElapsedEventArgs e)
        {
            SimulationTime = new TimeSpan(SimulationTime.Hours, SimulationTime.Minutes, SimulationTime.Seconds + 1);
        }

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
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

                    ((TextBox)sender).CaretIndex = ((TextBox)sender).Text.Length;

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

        private void Stop_Click(object sender, RoutedEventArgs e)
        {
            aTimer.Enabled = false;
            page_S1.StopSimulation();
        }
    }
}
