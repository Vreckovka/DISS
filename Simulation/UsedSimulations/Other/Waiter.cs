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

       

        public void MakeProperEvent(TimeSpan OccurrenceTime)
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
                this.Occupied = true;
            }
            else if (_core.AgentsWaitingForDeliver.Count != 0)
            {
                var agent = _core.AgentsWaitingForDeliver.Dequeue();

                var @event = new StartDeliveringFood_Event(agent,
                    OccurrenceTime,
                    _core,
                    this
                );
                this.Occupied = true;
                _core.Calendar.Enqueue(@event, @event.OccurrenceTime);
            }
            else if (_core.AgentsWaitingForPaying.Count != 0)
            {
                var agent = _core.AgentsWaitingForPaying.Dequeue();

                var @event = new StartPaying_Event(agent,
                    OccurrenceTime,
                    _core,
                    this);

                _core.Calendar.Enqueue(@event, @event.OccurrenceTime);
                this.Occupied = true;
            }           
        }

        public override string ToString()
        {
            return $"{Id} {Occupied}";
        }
    }
}
