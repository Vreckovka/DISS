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
    class EndOrderFood_Event : Event_S2
    {
        public Waiter Waiter { get; set; }
        public EndOrderFood_Event(Agent_S2 agent,
            double occurrenceTime,
            SimulationCore_S2 simulationCore,
            Waiter waiter) : base(agent, occurrenceTime, simulationCore)
        {
            Waiter = waiter;
            
        }

        public override void Execute()
        {
            Agent.EndOrder = OccurrenceTime;

            for (int i = 0; i < Agent.AgentCount; i++)
            {
                var food = GetFood();
                SimulationCore.FoodsWaintingForCook.Enqueue(food);
                SimulationCore.CheckCooks(OccurrenceTime);
            }

            Waiter.Occupied = false;
            Waiter.WorkedTime += (OccurrenceTime - Waiter.LastEventTime);
            SimulationCore.ChangeWaitersStats(OccurrenceTime);

            SimulationCore.FreeWaiters.Enqueue(Waiter, Waiter.WorkedTime);
            SimulationCore.CheckWaiters(OccurrenceTime);
        }

        private Food GetFood()
        {
            var core = SimulationCore;
            var prb = core.pickFoodRandom.NextDouble();

            if (prb < 0.3)
            {
                return new Food()
                {
                    Time =core.cezarSaladGenerator.GetNext(),
                    Agent = Agent,
                };
            }

            else if (prb < 0.65)
            {
                return new Food()
                {
                    Time = core.penneSaladGenerator.GetNext(),
                    Agent = Agent
                };
            }
            else if (prb < 0.85)
            {
                return new Food()
                {
                    Time = core.wholeWheatSpaghettiGenerator.GetNext(),
                    Agent = Agent
                };
            }
            else
            {
                return new Food()
                {
                    Time = core.richSaladGenerator,
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
