using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PropertyChanged;
using Simulations.UsedSimulations.S2;
using Simulations.UsedSimulations.S2.Events.ChefEvents;

namespace Simulations.UsedSimulations.Other
{
    [AddINotifyPropertyChangedInterface]
    public class Cook
    {
        public int Id { get; set; }
        public bool Occupied { get; set; }
        public double WorkedTime { get; set; }
        public double LastEventTime { get; set; }

        public SimulationCore_S2 _core;
        public static int Count;

        public Cook(SimulationCore_S2 core)
        {
            Id = Count;
            Count++;
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
