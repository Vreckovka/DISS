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
    class EndCooking_Event : Event_S2
    {
        private Cook Cook;
        public EndCooking_Event(Agent_S2 agent,
            double occurrenceTime,
            SimulationCore_S2 simulationCore,
            Cook cook) : base(agent, occurrenceTime, simulationCore)
        {
            Cook = cook;
        }

        public override void Execute()
        {
            var core = SimulationCore;
            Agent.FoodLeft--;

            if (Agent.FoodLeft == 0)
            {
                core.AgentsWaitingForDeliver.Enqueue(Agent);

            }

            Cook.Occupied = false;
            Cook.WorkedTime += OccurrenceTime - Cook.LastEventTime;
            core.ChangeCooksStats(OccurrenceTime);

            core.FreeCooks.Enqueue(Cook);

            core.CheckCooks(OccurrenceTime);
            core.CheckWaiters(OccurrenceTime);
        }

        public override string ToString()
        {
            return $"Agent: {Agent}, Kuchar:  {Cook.Id}  Koniec varenia  \t{OccurrenceTime}";
        }
    }
}
