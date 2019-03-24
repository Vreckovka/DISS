using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Simulations.Simulations.EventSimulation
{
    public abstract class SimulationEvent
    {
        public TimeSpan OccurrenceTime { get; set; }
        public abstract void Execute();
        public Agent Agent { get; set; }
        public SimulationCore SimulationCore { get; set; }
        public SimulationEvent(Agent agent, TimeSpan occurrenceTime, SimulationCore simulationCore)
        {
            Agent = agent;
            OccurrenceTime = occurrenceTime;
            SimulationCore = simulationCore;
        }
    }
}
