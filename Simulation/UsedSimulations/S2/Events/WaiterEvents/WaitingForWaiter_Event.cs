using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Simulations.Simulations.EventSimulation;

namespace Simulations.UsedSimulations.S2.Events.WaiterEvents
{
    class WaitingForWaiter_Event : SimulationEvent
    {
        public WaitingForWaiter_Event(Agent agent, 
            TimeSpan occurrenceTime, 
            SimulationCore simulationCore) : base(agent, occurrenceTime, simulationCore)
        {
        }

        public override void Execute()
        {
            var core = (S2_SimulationCore)SimulationCore;
            var freeWaiter = (from x in core.Waiters where x.Occupied == false select x).FirstOrDefault();

            if (freeWaiter != null)
            {
                var @event = new WaitingForPickFood_Event(Agent,
                    OccurrenceTime + TimeSpan.FromSeconds(core.waintingForOrderGenerator.GetNext()),
                    core,
                    freeWaiter);

                core.Calendar.Enqueue(@event, @event.OccurrenceTime);
            }
            else
            {
                core.AgentsWaitingForOrder.Enqueue(Agent);
            }
        }

        public override string ToString()
        {
            return $"Agent: {Agent}\t\t  Prichod obsluhy  \t{OccurrenceTime}";
        }
    }
}
