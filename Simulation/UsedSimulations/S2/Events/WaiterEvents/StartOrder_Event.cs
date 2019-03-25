using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Simulations.Simulations.EventSimulation;
using Simulations.UsedSimulations.Other;

namespace Simulations.UsedSimulations.S2.Events.WaiterEvents
{
    class StartOrder_Event : SimulationEvent
    {
        private Waiter Waiter;
        public StartOrder_Event(Agent agent,
            TimeSpan occurrenceTime,
            SimulationCore simulationCore,
            Waiter waiter) : base(agent, occurrenceTime, simulationCore)
        {
            Waiter = waiter;
            ((Agent_S2)Agent).StartOrder = OccurrenceTime;
        }

        public override void Execute()
        {
            var core = (S2_SimulationCore)SimulationCore;

            var @event = new EndOrderFood_Event(Agent,
                OccurrenceTime + TimeSpan.FromSeconds(core.waintingForOrderGenerator.GetNext()),
                core,
                Waiter);

            core.Calendar.Enqueue(@event, @event.OccurrenceTime);

            Waiter.Occupied = true;
        }

        public override string ToString()
        {
            return $"Agent: {Agent}, Obsluha: {Waiter.Id}  Prichod obsluhy  \t{OccurrenceTime}";
        }
    }
}
