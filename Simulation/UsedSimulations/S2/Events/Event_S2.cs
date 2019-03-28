using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Simulations.Simulations.EventSimulation;
using Simulations.UsedSimulations.Other;

namespace Simulations.UsedSimulations.S2.Events
{
    abstract class Event_S2 : SimulationEvent
    {
        public new SimulationCore_S2 SimulationCore { get; set; }
        public new Agent_S2 Agent { get; set; }
        public Event_S2(Agent_S2 agent, double occurrenceTime, SimulationCore_S2 simulationCore) : base(agent, occurrenceTime, simulationCore)
        {
            SimulationCore = simulationCore;
            Agent = agent;
        }

        public abstract override void Execute();
    }
}
