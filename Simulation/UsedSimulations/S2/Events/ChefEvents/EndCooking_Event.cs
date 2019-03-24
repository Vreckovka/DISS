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
        public Cook Cook { get; set; }
        public Food Food { get; set; }
        public EndCooking_Event(Agent agent,
            TimeSpan occurrenceTime,
            SimulationCore simulationCore,
            Cook cook) : base(agent, occurrenceTime, simulationCore)
        {
            Cook = cook;
            Cook.Occupied = true;
        }

        public override void Execute()
        {
            var core = (S2_SimulationCore)SimulationCore;
            
            if (core.FoodsWaintingToCook.Count > 0)
            {
               Food nextFood = core.FoodsWaintingToCook.Dequeue();

                var @event = new StartsCooking_Event(nextFood.Agent,
                    OccurrenceTime + nextFood.Time,
                    core);

                core.Calendar.Enqueue(@event, @event.OccurrenceTime);
            }

            var freeWaiter = (from x in core.Waiters where x.Occupied == false select x).FirstOrDefault();

            var @event1 = new DeliveringFood_Event(Agent,
                OccurrenceTime + TimeSpan.FromSeconds(core.deliveringFoodGenerator.GetNext()),
                core,
                freeWaiter
                );

            core.Calendar.Enqueue(@event1, @event1.OccurrenceTime);

            Cook.Occupied = false;
        }

        public override string ToString()
        {
            return $"Agent: {Agent}, Kuchar:  {Cook.Id}  Koniec varenia  \t{OccurrenceTime}";
        }
    }
}
