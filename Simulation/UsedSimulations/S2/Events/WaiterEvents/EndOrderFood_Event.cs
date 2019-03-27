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
            
        }

        public override void Execute()
        {
            var core = (S2_SimulationCore)SimulationCore;
            ((Agent_S2)Agent).EndOrder = OccurrenceTime;

            List<Food> foods = new List<Food>();

            for (int i = 0; i < ((Agent_S2)Agent).AgentCount; i++)
            {
                foods.Add(GetFood());
            }

            ((Agent_S2) Agent).FoodLeft = ((Agent_S2) Agent).AgentCount;

            foreach (var food in foods)
            {
                core.FoodsWaintingForCook.Enqueue(food);
                core.CheckCooks(OccurrenceTime);
            }

            Waiter.Occupied = false;
            core.FreeWaiters.Enqueue(Waiter);

            core.CheckWaiters(OccurrenceTime);

            
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
            return $"Agent: {Agent}, Obsluha: {Waiter.Id} koniec Vyber jedla   \t{OccurrenceTime}";
        }
    }
}
