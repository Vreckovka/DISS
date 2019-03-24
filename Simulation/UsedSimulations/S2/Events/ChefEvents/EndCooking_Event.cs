using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Simulations.Simulations.EventSimulation;
using Simulations.UsedSimulations.Other;
using Simulations.UsedSimulations.S2.Events.WaiterEvents;

namespace Simulations.UsedSimulations.S2.Events.ChefEvents
{
    class EndCooking_Event : SimulationEvent
    {
        private Food Food;
        private Cook Cook;
        public EndCooking_Event(Agent agent,
            TimeSpan occurrenceTime,
            SimulationCore simulationCore,
            Food food,
            Cook cook) : base(agent, occurrenceTime, simulationCore)
        {
            Food = food;
            Cook = cook;
            Cook.Occupied = true;
           
        }

        public override void Execute()
        {
            var core = (S2_SimulationCore)SimulationCore;

            Cook.MakeProperEvent(OccurrenceTime);

            if (Food.LastFood)
            {
                core.FoodsWaitingForDeliver.Enqueue(new KeyValuePair<Agent, Food>(Agent, Food));

                var freeWaiter = (from x in core.Waiters where x.Occupied == false select x).FirstOrDefault();

                if (freeWaiter != null)
                {
                   freeWaiter.MakeProperEvent(OccurrenceTime);
                }
            }
        }

        public override string ToString()
        {
            return $"Agent: {Agent}, Kuchar:  {Cook.Id}  Koniec varenia  \t{OccurrenceTime}";
        }
    }
}
