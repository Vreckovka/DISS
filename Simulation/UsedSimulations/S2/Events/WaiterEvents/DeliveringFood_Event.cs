using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Simulations.Simulations.EventSimulation;
using Simulations.UsedSimulations.Other;
using Simulations.UsedSimulations.S2.Events.AgentEvents;
using Simulations.UsedSimulations.S2.Events.ChefEvents;

namespace Simulations.UsedSimulations.S2.Events.WaiterEvents
{
    class DeliveringFood_Event : SimulationEvent
    {
        public Waiter Waiter { get; set; }
        public DeliveringFood_Event(Agent agent, 
            TimeSpan occurrenceTime, 
            SimulationCore simulationCore,
            Waiter waiter) : base(agent, occurrenceTime, simulationCore)
        {
            Waiter = waiter;
            waiter.Occupied = true;
        }

        public override void Execute()
        {
            var core = (S2_SimulationCore)SimulationCore;

            var @event = new EndEatingFood_Event(Agent,
                OccurrenceTime + TimeSpan.FromSeconds(core.eatingFoodGenerator.GetNext()),
                core);

            core.Calendar.Enqueue(@event, @event.OccurrenceTime);

            Waiter.Occupied = false;
        }

        public override string ToString()
        {
            return $"Agent: {Agent}, Obsluha: {Waiter.Id}  Prinesenie jedla  \t{OccurrenceTime}";
        }
    }
}
