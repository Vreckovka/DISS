using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Simulations.Simulations.EventSimulation;
using Simulations.UsedSimulations.Other;

namespace Simulations.UsedSimulations.S2.Events.ChefEvents
{
    class StartCooking_Event : SimulationEvent
    {
        Cook Cook;
        private Food Food;
        public StartCooking_Event(Agent agent, 
            TimeSpan occurrenceTime, 
            SimulationCore simulationCore,
            Cook cook,
            Food food) : base(agent, occurrenceTime, simulationCore)
        {
            Cook = cook;
            Food = food;
        }

        public override void Execute()
        {
            var core = (S2_SimulationCore)SimulationCore;

            var @event = new EndCooking_Event(Food.Agent,
                OccurrenceTime + Food.Time,
                core,
                Food,
                Cook);

            core.Calendar.Enqueue(@event, @event.OccurrenceTime);

            Cook.Occupied = true;
        }

        public override string ToString()
        {
            return $"Agent: {Agent}, Kuchar:  {Cook.Id}  Zaciatok varenia  \t{OccurrenceTime}";
        }
    }
}
