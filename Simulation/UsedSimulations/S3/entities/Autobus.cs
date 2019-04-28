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
        public int IndexAktualnaZastavkaVLinke { get; set; }
        public int KapacitaOsob { get; set; }
        public bool StojiNaZastavke { get; set; }
        public List<Cestujuci> Cestujuci { get; set; }
        public Linka Linka { get; set; }
        public bool PlnyAutobus { get; set; }
        public int IndexStartZastavka { get; set; }
        public Zastavka AktualnaZastavka { get; set; }
        public int CelkovyPocetPrevezenych { get; set; }
        public int AktualnyPocetPrevezenych { get; set; }
        public Autobus(OSPABA.Simulation mySim) : base(mySim)
        {
            Cestujuci = new List<Cestujuci>();
        }

        public Autobus(int id, OSPABA.Simulation mySim) : base(id, mySim)
        {
            Cestujuci = new List<Cestujuci>();
        }

        public void Reset()
        {
            AktualnyPocetPrevezenych = 0;
            PlnyAutobus = false;
            Cestujuci.Clear();
            IndexAktualnaZastavkaVLinke = IndexStartZastavka;
        }
    }
}
