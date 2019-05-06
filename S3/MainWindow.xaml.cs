#define Live1

using System;
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
using S3.Pages;


namespace S3
{

    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    ///
    [AddINotifyPropertyChangedInterface]
    public partial class MainWindow : Window
    {
        
        public MySimulation Simulation { get; set; }

        public static Configuration_Page _conf_Page;
        public static LiveSimulation_Page _live_Page;
        public static Replication_Page _rep_Page;

        //private Configuration_Page _conf_Page;

        public MainWindow()
        {
            InitializeComponent();

            _conf_Page = new Configuration_Page();
            _live_Page = new LiveSimulation_Page();
            _rep_Page = new Replication_Page();

            Asd.Content = _conf_Page;
        }

        private void ConfClick(object sender, MouseButtonEventArgs e)
        {
            Asd.Content = _conf_Page;
        }

        private void LiveClick(object sender, MouseButtonEventArgs e)
        {
            Asd.Content = _live_Page;
        }

        private void RepClick(object sender, MouseButtonEventArgs e)
        {
            Asd.Content = _rep_Page;
        }
    }
}
