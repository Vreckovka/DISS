using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Simulations.Simulations.EventSimulation;
using Simulations.UsedSimulations.Other;
using Simulations.UsedSimulations.S2.Events.ChefEvents;

namespace Simulations.UsedSimulations.S2.Events.WaiterEvents
{
    class EndOrderFood_Event : SimulationEvent
    {
        public Waiter Waiter { get; set; }
        public EndOrderFood_Event(Agent agent,
            TimeSpan occurrenceTime,
            SimulationCore simulationCore,
            Waiter waiter) : base(agent, occurrenceTime, simulationCore)
        {
            Waiter = waiter;
            Waiter.Occupied = true;
            ((Agent_S2)Agent).EndOrder = OccurrenceTime;
        }

        public override void Execute()
        {
            var core = (S2_SimulationCore)SimulationCore;

            List<Food> foods = new List<Food>();

            for (int i = 0; i < ((Agent_S2)Agent).AgentCount; i++)
            {
                foods.Add(GetFood());
            }

            (from x in foods orderby x.Time descending select x).First().LastFood = true;

            foreach (var food in foods)
            {
                var freeCook = (from x in core.Cooks where x.Occupied == false select x).FirstOrDefault();

                if (freeCook == null)
                {
                    core.FoodsWaintingForCook.Enqueue(food);
                }
                else
                {
                    food.Cook = freeCook;
                    var @event = new EndCooking_Event(Agent,
                        food.Time + OccurrenceTime,
                        core,
                        food,
                        freeCook
                    );
                    core.Calendar.Enqueue(@event, @event.OccurrenceTime);
                }
            }

            Waiter.Occupied = false;
            Waiter.MakeProperEvent(OccurrenceTime);
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
                    Agent = Agent,
                    
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
            return $"Agent: {Agent}, Obsluha: {Waiter.Id}  Vyber jedla   \t{OccurrenceTime}";
        }
    }
}
