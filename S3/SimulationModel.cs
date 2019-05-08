using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Threading;
using PropertyChanged;
using simulation;
using S3.Pages;

namespace S3
{
    [AddINotifyPropertyChangedInterface]
    public class SimulationModel
    {
        public MySimulation Simulation { get; set; }
        public string SimTime { get; set; }
        public int Replication { get; set; }

        public SimulationModel()
        {
            Simulation = new MySimulation(MyConfiguration.Configuration);
           
            Simulation.OnReplicationDidFinish(Vypis);
            Simulation.OnSimulationDidFinish(Vypis);
            
           
        }

        private void Vypis(OSPABA.Simulation s)
        {
            //Console.Clear();
            //Console.WriteLine("Replikacia: " + (s.CurrentReplication + 1));
            //Console.WriteLine($"Cas ukoncenia: {(int)TimeSpan.FromMinutes(((MySimulation)s).AvrageFinishedTime).TotalHours}:" +
            //                  $"{TimeSpan.FromMinutes(((MySimulation)s).AvrageFinishedTime):mm}:" +
            //                  $"{TimeSpan.FromMinutes(((MySimulation)s).AvrageFinishedTime):ss}");
            //Console.WriteLine("Pocet ludi: " + ((MySimulation)s).AvragePocetLudi);

            Replication = s.CurrentReplication + 1;
        }

        public void RefreshGui(OSPABA.Simulation simulation)
        {
            var s = ((MySimulation)simulation);

            SimTime = TimeSpan.FromMinutes(Simulation.CurrentTime).ToString();

            foreach (var autobus in s.AgentAutobusov.Autobusy)
            {
                if (!double.IsPositiveInfinity(autobus.StatZaciatokJazdyCas) && !autobus.KoniecJazd)
                {
                    var prejdenyCas = s.CurrentTime - autobus.StatZaciatokJazdyCas;
                    var prec = (prejdenyCas * 100 / autobus.AktualnaZastavka.CasKDalsejZastavke);
                    autobus.PrejdenaTrasaPerc = prec;
                }
            }
        }
    }
}
