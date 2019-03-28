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
    class EndDeliveringFood_Event : Event_S2
    {
        public Waiter Waiter { get; set; }
        public EndDeliveringFood_Event(Agent_S2 agent,
            double occurrenceTime,
            SimulationCore_S2 simulationCore,
            Waiter waiter) : base(agent, occurrenceTime, simulationCore)
        {
            Waiter = waiter;
        }

        public override void Execute()
        {
            var core = SimulationCore;
            Agent.DeliveredFood = OccurrenceTime;

            double last = 0;

            for (int i = 0; i < Agent.AgentCount; i++)
            {
                var food = core.eatingFoodGenerator.GetNext();

                if (food > last)
                    last = food;
            }

            var @event = new EndEatingFood_Event(Agent,
                OccurrenceTime + last,
                core);

            core.Calendar.Enqueue(@event, @event.OccurrenceTime);

            Waiter.Occupied = false;
            Waiter.WorkedTime += OccurrenceTime - Waiter.LastEventTime;
            core.ChangeWaitersStats(OccurrenceTime);

            core.FreeWaiters.Enqueue(Waiter, Waiter.WorkedTime);
            core.CheckWaiters(OccurrenceTime);

        }

        public override string ToString()
        {
            return $"Agent: {Agent}, Obsluha: {Waiter.Id} Koniec Prinesenie jedla  \t{OccurrenceTime}";
        }
    }
}
