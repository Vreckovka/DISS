using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Simulations.UsedSimulations.Other
{
    public class Cook
    {
        public int Id { get; set; }
        public bool Occupied { get; set; }

        private static int _count;

        public Cook()
        {
            Id = _count;
            _count++;
        }
    }
}
