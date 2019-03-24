using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Simulations.Simulations.EventSimulation;
using Simulations.UsedSimulations.S2.Events.WaiterEvents;

namespace Simulations.UsedSimulations.S2.Events.AgentEvents
{
    class EndEatingFood_Event : SimulationEvent
    {
        public EndEatingFood_Event(Agent agent, TimeSpan occurrenceTime, SimulationCore simulationCore) : base(agent, occurrenceTime, simulationCore)
        {
        }

        public override void Execute()
        {
           
            var core = (S2_SimulationCore)SimulationCore;

            var freeWaiter = (from x in core.Waiters where x.Occupied == false select x).FirstOrDefault();

            if (freeWaiter != null)
            {
                var @event = new StartPaying_Event(Agent,
                    OccurrenceTime,
                    core,
                    freeWaiter);

                core.Calendar.Enqueue(@event, @event.OccurrenceTime);
            }
        }

        public override string ToString()
        {
            return $"Agent: {Agent}\t\t  Koniec jedenia  \t{OccurrenceTime}";
        }
    }
}
