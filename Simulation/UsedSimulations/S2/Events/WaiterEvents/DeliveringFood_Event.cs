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
            ((Agent_S2)Agent).DeliveredFood = OccurrenceTime;
        }

        public override void Execute()
        {
            var core = (S2_SimulationCore)SimulationCore;

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

            Waiter.Occupied = Waiter.MakeProperEvent(OccurrenceTime);
        }

        public override string ToString()
        {
            return $"Agent: {Agent}, Obsluha: {Waiter.Id}  Prinesenie jedla  \t{OccurrenceTime}";
        }
    }
}
