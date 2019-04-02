using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PropertyChanged;
using Simulations.Simulations.EventSimulation;
using Simulations.UsedSimulations.S2;
using Simulations.UsedSimulations.S2.Events.WaiterEvents;

namespace Simulations.UsedSimulations.Other
{
    [AddINotifyPropertyChangedInterface]
    public class Waiter
    {
        public int Id { get; set; }
        public double WorkedTime { get; set; }
        [DoNotNotify]
        public double LastEventTime { get; set; }
        public bool Occupied { get; set; }
        public static int Count;
        private SimulationCore_S2 _core;
        public Waiter(SimulationCore_S2 core)
        {
            Id = Count;
            Count++;
            _core = core;
        }

        public void MakeProperEvent(double occurrenceTime)
        {
            if (_core.AgentsWaitingForOrder.Count != 0)
            {
                var agent = _core.AgentsWaitingForOrder.Dequeue();
                _core.CountOfWaitingAgents_Order -= agent.AgentCount;

                var @event = new StartOrder_Event(agent,
                    occurrenceTime,
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
                _core.CountOfWaitingAgents_Deliver -= agent.AgentCount;

                var @event = new StartDeliveringFood_Event(agent,
                    occurrenceTime,
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
                _core.CountOfWaitingAgents_Pay -= agent.AgentCount;

                var @event = new StartPaying_Event(agent,
                    occurrenceTime,
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
