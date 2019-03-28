using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Simulations.Simulations.EventSimulation;
using Simulations.UsedSimulations.Other;

namespace Simulations.UsedSimulations.S2.Events.ChefEvents
{
    class StartCooking_Event : Event_S2
    {
        Cook Cook;
        private Food Food;
        public StartCooking_Event(Agent_S2 agent,
            double occurrenceTime, 
            SimulationCore_S2 simulationCore,
            Cook cook,
            Food food) : base(agent, occurrenceTime, simulationCore)
        {
            Cook = cook;
            Food = food;
        }

        public override void Execute()
        {
            var core = SimulationCore;

            var @event = new EndCooking_Event(Food.Agent,
                OccurrenceTime + Food.Time,
                core,
                Cook);

            core.Calendar.Enqueue(@event, @event.OccurrenceTime);

        }

        public override string ToString()
        {
            return $"Agent: {Agent}, Kuchar:  {Cook.Id}  Zaciatok varenia  \t{OccurrenceTime}";
        }
    }
}
