using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Simulations.UsedSimulations.Other
{
    public class Waiter
    {
        public int Id { get; set; }
        public Table Table { get; set; }
        public bool Occupied { get; set; }

        private static int _count;

        public Waiter()
        {
            Id = _count;
            _count++;
        }
    }
}
