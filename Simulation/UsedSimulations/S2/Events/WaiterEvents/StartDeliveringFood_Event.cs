using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Simulations.Simulations.EventSimulation;
using Simulations.UsedSimulations.Other;

namespace Simulations.UsedSimulations.S2.Events.WaiterEvents
{
    class StartDeliveringFood_Event : Event_S2
    {
        private Waiter Waiter;
        public StartDeliveringFood_Event(Agent_S2 agent,
            double occurrenceTime, 
            SimulationCore_S2 simulationCore,
            Waiter waiter) : base(agent, occurrenceTime, simulationCore)
        {
            Waiter = waiter;
        }

        public override void Execute()
        {
            var core = SimulationCore;
            
            var @event = new EndDeliveringFood_Event(Agent,
                OccurrenceTime + core.deliveringFoodGenerator.GetNext(),
                core,
                Waiter
            );

            core.Calendar.Enqueue(@event, @event.OccurrenceTime);
        }

        public override string ToString()
        {
            return $"Agent: {Agent}, Obsluha: {Waiter.Id} Zaciatok Prinesenie jedla  \t{OccurrenceTime}";
        }
    }
}
