using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Simulations.Simulations.EventSimulation;
using Simulations.UsedSimulations.Other;

namespace Simulations.UsedSimulations.S2.Events.WaiterEvents
{
    class StartDeliveringFood_Event : SimulationEvent
    {
        private Waiter Waiter;
        public StartDeliveringFood_Event(Agent agent, 
            TimeSpan occurrenceTime, 
            SimulationCore simulationCore,
            Waiter waiter) : base(agent, occurrenceTime, simulationCore)
        {
            Waiter = waiter;
        }

        public override void Execute()
        {
            var core = (S2_SimulationCore)SimulationCore;
            
            var @event = new EndDeliveringFood_Event(Agent,
                OccurrenceTime + TimeSpan.FromSeconds(core.deliveringFoodGenerator.GetNext()),
                core,
                Waiter
            );

            core.Calendar.Enqueue(@event, @event.OccurrenceTime);

            Waiter.Occupied = true;
        }

        public override string ToString()
        {
            return $"Agent: {Agent}, Obsluha: {Waiter.Id} Zaciatok Prinesenie jedla  \t{OccurrenceTime}";
        }
    }
}
