using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Simulations.Simulations.EventSimulation;
using Simulations.UsedSimulations.Other;

namespace Simulations.UsedSimulations.S2.Events.ChefEvents
{
    class StartsCooking_Event : SimulationEvent
    {
        public StartsCooking_Event(Agent agent, TimeSpan occurrenceTime, SimulationCore simulationCore) : base(agent, occurrenceTime, simulationCore)
        {
        }

        public override void Execute()
        {
            var core = (S2_SimulationCore)SimulationCore;

            Food food = GetFood();
            
            var freeCook = (from x in core.Cooks where x.Occupied == false select x).FirstOrDefault();

            if (freeCook == null)
            {
                core.FoodsWaintingToCook.Enqueue(food);
            }
            else
            {
                food.Cook = freeCook;

                var @event = new EndCooking_Event(Agent,
                    food.Time + OccurrenceTime,
                    core,
                    freeCook
                    );

                @event.Food = food;

                core.Calendar.Enqueue(@event, @event.OccurrenceTime);
            }
        }

        private Food GetFood()
        {
            var core = (S2_SimulationCore)SimulationCore;
            var prb = core.pickFoodRandom.NextDouble();

            if (prb < 0.3)
            {
                return new Food()
                {
                    FoodType = FoodType.CezarSalad,
                    Time = TimeSpan.FromSeconds(core.cezarSaladGenerator.GetNext()),
                    Agent = Agent
                };
            }

            else if (prb < 0.65)
            {
                return new Food()
                {
                    FoodType = FoodType.PenneSalad,
                    Time = TimeSpan.FromSeconds(core.penneSaladGenerator.GetNext()),
                    Agent = Agent
                };
            }
            else if (prb < 0.85)
            {
                return new Food()
                {
                    FoodType = FoodType.WholeWheatSpaghetti,
                    Time = TimeSpan.FromSeconds(core.wholeWheatSpaghettiGenerator.GetNext()),
                    Agent = Agent
                };
            }
            else
            {
                return new Food()
                {
                    FoodType = FoodType.RichSalad,
                    Time = TimeSpan.FromSeconds(core.richSaladGenerator),
                    Agent = Agent
                };
            }
        }

        public override string ToString()
        {
            return $"Agent: {Agent}\t\t  Zaciatok varenia  \t{OccurrenceTime}";
        }
    }
}
