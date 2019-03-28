using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Simulations.Simulations.EventSimulation;
using Simulations.UsedSimulations.Other;

namespace Simulations.UsedSimulations.S2.Events.WaiterEvents
{
    class EndPaying_Event : Event_S2
    {
        public Waiter Waiter { get; set; }
        public EndPaying_Event(Agent_S2 agent,
            double occurrenceTime,
            SimulationCore_S2 simulationCore,
            Waiter waiter) : base(agent, occurrenceTime, simulationCore)
        {
            Waiter = waiter;
        }

        public override void Execute()
        {
            var core = SimulationCore;

            core.WaitingTimeOfAgents += (Agent.StartOrder - Agent.ArrivalTime) * Agent.AgentCount;
            core.WaitingTimeOfAgents += (Agent.DeliveredFood - Agent.EndOrder) * Agent.AgentCount;
            core.WaitingTimeOfAgents += (Agent.StartPaying - Agent.EndEatingFood) * Agent.AgentCount;
                        
            core.CountOfPaiedAgents += Agent.AgentCount;

            Waiter.Occupied = false;
            Waiter.WorkedTime += OccurrenceTime - Waiter.LastEventTime;
            core.ChangeWaitersStats(OccurrenceTime);

            core.FreeWaiters.Enqueue(Waiter, Waiter.WorkedTime);

            Agent.Table.Occupied = false;
            core.ChangeTableStats(OccurrenceTime, Agent.Table, true);

            core.CheckWaiters(OccurrenceTime);
        }

        public override string ToString()
        {
            return $"Agent: {Agent}, Obsluha: {Waiter.Id}  Koniec platenia  \t{OccurrenceTime}";
        }
    }
}
