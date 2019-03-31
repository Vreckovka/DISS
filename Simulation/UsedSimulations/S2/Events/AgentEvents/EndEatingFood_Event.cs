using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Simulations.Simulations.EventSimulation;
using Simulations.UsedSimulations.Other;
using Simulations.UsedSimulations.S2.Events.WaiterEvents;

namespace Simulations.UsedSimulations.S2.Events.AgentEvents
{
    class EndEatingFood_Event : Event_S2
    {
        public EndEatingFood_Event(Agent_S2 agent, double occurrenceTime, SimulationCore_S2 simulationCore) : base(agent, occurrenceTime, simulationCore)
        {

        }

        public override void Execute()
        {
            Agent.EndEatingFood = OccurrenceTime;

            SimulationCore.AgentsWaitingForPaying.Enqueue(Agent);
            SimulationCore.CountOfWaitingAgents_Pay += Agent.AgentCount;

            SimulationCore.CheckWaiters(OccurrenceTime);
        }

        public override string ToString()
        {
            return $"Agent: {Agent}\t\t  Koniec jedenia  \t{OccurrenceTime}";
        }
    }
}
