using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Simulations.UsedSimulations.Other
{
    public class Table
    {
        public int Capacity { get; set; }
        public bool Occupied { get; set; }

        public override string ToString()
        {
            return $"{Capacity} {Occupied}";
        }
    }
}
