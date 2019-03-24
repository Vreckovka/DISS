using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Simulations.Simulations.EventSimulation;
using Simulations.UsedSimulations.S2;
using Simulations.UsedSimulations.S2.Events.WaiterEvents;

namespace Simulations.UsedSimulations.Other
{
    public class Waiter
    {
        public int Id { get; set; }
        public Table Table { get; set; }
        public bool Occupied { get; set; }
        private static int _count;
        private S2_SimulationCore _core;
        public Waiter(S2_SimulationCore core)
        {
            Id = _count;
            _count++;
            _core = core;
        }

        public bool MakeProperEvent(TimeSpan OccurrenceTime)
        {
            if (_core.AgentsWaitingForOrder.Count != 0)
            {
                var agent = _core.AgentsWaitingForOrder.Dequeue();
                var @event = new StartOrder_Event(agent,
                    OccurrenceTime,
                    _core,
                    this
                );

                _core.Calendar.Enqueue(@event, @event.OccurrenceTime);
                return true;
            }
            else if (_core.FoodsWaitingForDeliver.Count != 0)
            {
                var food = _core.FoodsWaitingForDeliver.Dequeue();

                var @event = new DeliveringFood_Event(food.Key,
                    OccurrenceTime + TimeSpan.FromSeconds(_core.deliveringFoodGenerator.GetNext()),
                    _core,
                    this
                );

                _core.Calendar.Enqueue(@event, @event.OccurrenceTime);
                return true;
            }
            else if (_core.AgentsWaitingForPaying.Count != 0)
            {
                var agent = _core.AgentsWaitingForPaying.Dequeue();

                var @event = new StartPaying_Event(agent,
                    OccurrenceTime,
                    _core,
                    this);

                _core.Calendar.Enqueue(@event, @event.OccurrenceTime);
                return true;
            }

            return false;
        }
    }
}
