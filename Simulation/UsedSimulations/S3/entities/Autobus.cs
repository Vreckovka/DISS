using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OSPABA;
using PropertyChanged;

namespace Simulations.UsedSimulations.S3.entities
{
    [AddINotifyPropertyChangedInterface]
    public class Autobus : Entity
    {
        public int PocetDveri { get; set; }
        public int KapacitaOsob { get; set; }
        public Queue<Cestujuci> Cestujuci { get; set; }
        public Linka Linka { get; set; }


        public bool StojiNaZastavke { get; set; }
        public bool KoniecProcesu { get; set; }
        public ZastavkaData AktualnaZastavka { get; set; }
        public ZastavkaData ZaciatocnaZastavka { get; set; }
        public bool NovaJazda { get; set; }

        public int CelkovyPocetPrevezenych { get; set; }
        public int AktualnyPocetPrevezenych { get; set; }

        public double JazdaStart { get; set; }
        public double JazdaEnd { get; set; }
        public double DobaJazdy { get; set; }
        public double PocetKoliecok { get; set; }
        public Autobus(OSPABA.Simulation mySim) : base(mySim)
        {
            Cestujuci = new Queue<Cestujuci>();
        }

        public Autobus(int id, OSPABA.Simulation mySim) : base(id, mySim)
        {
            Cestujuci = new Queue<Cestujuci>();
        }

        public void Reset()
        {
            AktualnyPocetPrevezenych = 0;
            KoniecProcesu = false;
            Cestujuci.Clear();
        }
    }
}
