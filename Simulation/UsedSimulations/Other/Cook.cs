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
        public double WorkedTime { get; set; }
        public double LastEventTime { get; set; }

        public SimulationCore_S2 _core;
        private static int _count;

        public Cook(SimulationCore_S2 core)
        {
            Id = _count;
            _count++;
            _core = core;
        }

        public void MakeProperEvent(double OccurrenceTime)
        {
            if (_core.FoodsWaintingForCook.Count > 0)
            {
                Food nextFood = _core.FoodsWaintingForCook.Dequeue();

                var @event = new StartCooking_Event(nextFood.Agent,
                    OccurrenceTime,
                    _core,
                    this,
                    nextFood);

                _core.Calendar.Enqueue(@event, @event.OccurrenceTime);

                this.Occupied = true;
                this.LastEventTime = @event.OccurrenceTime;
            }
        }

        public override string ToString()
        {
            return $"{Id} {Occupied} {WorkedTime}";
        }
    }
}
