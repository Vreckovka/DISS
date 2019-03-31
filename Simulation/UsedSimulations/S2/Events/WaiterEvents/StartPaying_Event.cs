using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Simulations.Simulations.EventSimulation;
using Simulations.UsedSimulations.Other;

namespace Simulations.UsedSimulations.S2.Events.WaiterEvents
{
    class StartPaying_Event : Event_S2
    {
        public Waiter Waiter { get; set; }

        public StartPaying_Event(Agent_S2 agent,
            double occurrenceTime,
            SimulationCore_S2 simulationCore,
            Waiter waiter) : base(agent, occurrenceTime, simulationCore)
        {
            Waiter = waiter;
        }


        public override void Execute()
        {
            var core = SimulationCore;
            Agent.StartPaying = OccurrenceTime;

            if (SimulationCore.LiveSimulation)
                SimulationCore.WaitingTimeOfAgents += (Agent.StartPaying - Agent.EndEatingFood) * Agent.AgentCount;

            var @event = new EndPaying_Event(Agent,
                            OccurrenceTime + core.payingGenerator.GetNext(),
                            core,
                            Waiter);

            core.Calendar.Enqueue(@event, @event.OccurrenceTime);
        }

        public override string ToString()
        {
            return $"Agent: {Agent}, Obsluha: {Waiter.Id}  Zaciatok platenia  \t{OccurrenceTime}";
        }
    }
}
