using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OSPABA;

namespace Simulations.UsedSimulations.S3.entities
{
    public class Cestujuci : Entity
    {
        public double CasZacatiaCakania { get; set; }
        public double CasCakania { get; set; }
        public bool PrisielNaCas { get; set; }
        public Cestujuci(OSPABA.Simulation mySim) : base(mySim)
        {
        }

        public Cestujuci(int id, OSPABA.Simulation mySim) : base(id, mySim)
        {
        }
    }
}
