using System;
using System.Collections.Generic;
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
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using DISS.Annotations;
using DISS.SimulationPages;
using Simulations.ConfidenceInterval;

namespace DISS.WindowsPages
{
    /// <summary>
    /// Interaction logic for SimulationRuns_Page.xaml
    /// </summary>
    public partial class Page_SimulationRuns : Page
    {
        private Page_S2 page_S1 = new Page_S2();

        public Page_SimulationRuns()
        {
            InitializeComponent();
            page_S1.SimulationModel.Simulation.RunFinished += Simulation_RunFinished; ; ;
            DataContext = page_S1;
          
        }

        private void Simulation_RunFinished(object sender, double[] e)
        {
            Dispatcher.Invoke(() =>
            {
                DataContext = null;
                DataContext = page_S1;
            });
        }

        private void Start_Click(object sender, RoutedEventArgs e)
        {
            if (!page_S1.SimulationRunning)
                page_S1.StartRuns(ConvertToInt("10"),
                    ConvertToInt(TextBox_NumberOfReplication.Text), 
                    ConvertToInt(TextBox_NumberOfWaiters.Text), 
                    ConvertToInt(TextBox_NumberOfCooks.Text), Convert.ToBoolean(CheckBox_Cooling.IsChecked));
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

    public class ConfidenceIntervalConverter : MarkupExtension, IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var str = ConfidenceInterval.ToStringInterval(0.95, (ConfidenceInterval.SampleStandardDeviationData)value);
            return str;
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
