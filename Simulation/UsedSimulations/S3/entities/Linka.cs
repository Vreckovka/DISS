using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OSPABA;
using PropertyChanged;

namespace Simulations.UsedSimulations.S3.entities
{
    [AddINotifyPropertyChangedInterface]
    public class Linka : Entity
    {
        public string Meno { get; set; }
        public List<ZastavkaData> Zastavky { get; set; }
        public ObservableCollection<Autobus> Autobusy { get; set; } = new ObservableCollection<Autobus>();

        public Linka(OSPABA.Simulation mySim) : base(mySim)
        {
        }

        public Linka(int id, OSPABA.Simulation mySim) : base(id, mySim)
        {
        }
    }
}
