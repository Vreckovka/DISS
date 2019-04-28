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
    public class Zastavka : Entity
    {
        public string Meno { get; set; }
        public int CelkovyPocetCestujucich { get; set; }
        public int GlobalIndex { get; set; }
        public double CasKoncaGenerovania { get; set; }

        public Queue<Cestujuci> Cestujuci { get; set; }
        public Zastavka(OSPABA.Simulation mySim) : base(mySim)
        {
            Cestujuci = new Queue<Cestujuci>();
        }

        public Zastavka(int id, OSPABA.Simulation mySim) : base(id, mySim)
        {
        }
    }
}
