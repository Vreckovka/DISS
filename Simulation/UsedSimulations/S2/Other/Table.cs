using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PropertyChanged;

namespace Simulations.UsedSimulations.Other
{
    [AddINotifyPropertyChangedInterface]
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
