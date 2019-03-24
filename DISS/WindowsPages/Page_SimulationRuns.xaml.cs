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

        public Page_S1 Page_S1
        {
            get { return page_S1; }
            set
            {
                OnPropertyChanged(nameof(Page_S1));
                page_S1 = value;
            }
        }

        public Page_SimulationRuns()
        {
            InitializeComponent();

            page_S1.SimulationModel.McSimulation.RunFinished += Simulation_RunFinished; ;

            DataContext = page_S1;

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
                    "A-F-G-E: " + (e[2])  + "\n" +
                    "Probability: " + (e[3])

                });

                if (DataGrid_SimulationRuns.Items.Count > 0)
                {
                    var border = VisualTreeHelper.GetChild(DataGrid_SimulationRuns, 0) as Decorator;
                    var scroll = border?.Child as ScrollViewer;
                    scroll?.ScrollToEnd();
                }
            });
        }

        private void Start_Click(object sender, RoutedEventArgs e)
        {
            if (!page_S1.SimulationRunning)
                page_S1.StartRuns(ConvertToInt(TextBox_NumberOfRuns.Text),
                    ConvertToInt(TextBox_NumberOfReplication.Text));
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

        private void Stop_Click(object sender, RoutedEventArgs e)
        {
            page_S1.StopSimulation();
        }
    }
}
