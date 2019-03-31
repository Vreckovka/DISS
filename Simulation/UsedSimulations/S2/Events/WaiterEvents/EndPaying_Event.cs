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
            if (!SimulationCore.LiveSimulation)
            {
                SimulationCore.WaitingTimeOfAgents += (Agent.StartOrder - Agent.ArrivalTime) * Agent.AgentCount;
                SimulationCore.WaitingTimeOfAgents += (Agent.DeliveredFood - Agent.EndOrder) * Agent.AgentCount;
                SimulationCore.WaitingTimeOfAgents += (Agent.StartPaying - Agent.EndEatingFood) * Agent.AgentCount;
            }

            SimulationCore.CountOfPaiedAgents += Agent.AgentCount;


            Waiter.Occupied = false;
            Waiter.WorkedTime += (OccurrenceTime - Waiter.LastEventTime);
            SimulationCore.ChangeWaitersStats(OccurrenceTime);

            Agent.Table.Occupied = false;
            SimulationCore.ChangeTableStats(OccurrenceTime, Agent.Table, true);

            SimulationCore.FreeWaiters.Enqueue(Waiter, Waiter.WorkedTime);
            SimulationCore.CheckWaiters(OccurrenceTime);
        }

        public override string ToString()
        {
            return $"Agent: {Agent}, Obsluha: {Waiter.Id}  Koniec platenia  \t{OccurrenceTime}";
        }
    }
}
