using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Simulations.UsedSimulations.S2;
using Simulations.UsedSimulations.S2.Events.ChefEvents;

namespace Simulations.UsedSimulations.Other
{
    public class Cook
    {
        public int Id { get; set; }
        public bool Occupied { get; set; }

        public S2_SimulationCore _core;
        private static int _count;

        public Cook(S2_SimulationCore core)
        {
            Id = _count;
            _count++;
            _core = core;
        }

        public void MakeProperEvent(TimeSpan OccurrenceTime)
        {
            if (_core.FoodsWaintingForCook.Count > 0)
            {
                Food nextFood = _core.FoodsWaintingForCook.Dequeue();

                var @event = new EndCooking_Event(nextFood.Agent,
                    OccurrenceTime + nextFood.Time,
                    _core,
                    nextFood,
                    this);

                _core.Calendar.Enqueue(@event, @event.OccurrenceTime);
            }
            else
            {
                this.Occupied = false;
            }
        }

        public override string ToString()
        {
            return $"{Id} {Occupied}";
        }
    }
}
