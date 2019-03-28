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
        public double WorkedTime { get; set; }
        public double LastEventTime { get; set; }
        public bool Occupied { get; set; }
        private static int _count;
        private SimulationCore_S2 _core;
        public Waiter(SimulationCore_S2 core)
        {
            Id = _count;
            _count++;
            _core = core;
        }

        public void MakeProperEvent(double OccurrenceTime)
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
                this.LastEventTime = @event.OccurrenceTime;
            }
            else if (_core.AgentsWaitingForDeliver.Count != 0)
            {
                var agent = _core.AgentsWaitingForDeliver.Dequeue();

                var @event = new StartDeliveringFood_Event(agent,
                    OccurrenceTime,
                    _core,
                    this
                );
               
                _core.Calendar.Enqueue(@event, @event.OccurrenceTime);

                this.Occupied = true;
                this.LastEventTime = @event.OccurrenceTime;
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
                this.LastEventTime = @event.OccurrenceTime;
            }           
        }

        public override string ToString()
        {
            return $"{Id} {Occupied} {WorkedTime}";
        }
    }
}
