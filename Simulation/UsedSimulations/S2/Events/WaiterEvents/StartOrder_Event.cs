using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Simulations.Simulations.EventSimulation;
using Simulations.UsedSimulations.Other;

namespace Simulations.UsedSimulations.S2.Events.WaiterEvents
{
    class StartOrder_Event : Event_S2
    {
        private Waiter Waiter;
        public StartOrder_Event(Agent_S2 agent,
            double occurrenceTime,
            SimulationCore_S2 simulationCore,
            Waiter waiter) : base(agent, occurrenceTime, simulationCore)
        {
            Waiter = waiter;
        }

        public override void Execute()
        {
            var core = SimulationCore;
            Agent.StartOrder = OccurrenceTime;

            if (SimulationCore.LiveSimulation)
                SimulationCore.WaitingTimeOfAgents += (Agent.StartOrder - Agent.ArrivalTime) * Agent.AgentCount;

            var @event = new EndOrderFood_Event(Agent,
                OccurrenceTime + core.waintingForOrderGenerator.GetNext(),
                core,
                Waiter);

            core.Calendar.Enqueue(@event, @event.OccurrenceTime);
        }

        public override string ToString()
        {
            return $"Agent: {Agent}, Obsluha: {Waiter.Id}  Prichod obsluhy  \t{OccurrenceTime}";
        }
    }
}
