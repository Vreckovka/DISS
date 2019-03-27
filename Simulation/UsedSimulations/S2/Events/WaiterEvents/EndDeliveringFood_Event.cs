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
    class EndDeliveringFood_Event : SimulationEvent
    {
        public Waiter Waiter { get; set; }
        public EndDeliveringFood_Event(Agent agent,
            TimeSpan occurrenceTime,
            SimulationCore simulationCore,
            Waiter waiter) : base(agent, occurrenceTime, simulationCore)
        {
            Waiter = waiter;
           
        }

        public override void Execute()
        {
            var core = (S2_SimulationCore)SimulationCore;
            ((Agent_S2)Agent).DeliveredFood = OccurrenceTime;

            List<TimeSpan> eatinFood = new List<TimeSpan>();

            for (int i = 0; i < ((Agent_S2)Agent).AgentCount; i++)
            {
                eatinFood.Add(TimeSpan.FromSeconds(core.eatingFoodGenerator.GetNext()));
            }

            var last = (from x in eatinFood orderby x.TotalMilliseconds descending select x).First();

            var @event = new EndEatingFood_Event(Agent,
                OccurrenceTime + last,
                core);

            core.Calendar.Enqueue(@event, @event.OccurrenceTime);

            Waiter.Occupied = false;
            core.FreeWaiters.Enqueue(Waiter);

            core.CheckWaiters(OccurrenceTime);

        }

        public override string ToString()
        {
            return $"Agent: {Agent}, Obsluha: {Waiter.Id} Koniec Prinesenie jedla  \t{OccurrenceTime}";
        }
    }
}
