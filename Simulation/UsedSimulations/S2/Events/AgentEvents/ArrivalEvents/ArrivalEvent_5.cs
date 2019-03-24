using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Simulations.Simulations.EventSimulation;

namespace Simulations.UsedSimulations.S2.Events.AgentEvents.ArrivalEvents
{
    class ArrivalEvent_5 : SimulationEvent
    {
        public ArrivalEvent_5(Agent agent, TimeSpan occurrenceTime, SimulationCore simulationCore) : base(agent, occurrenceTime, simulationCore)
        {
        }

        public override void Execute()
        {
            throw new NotImplementedException();
        }

        public override string ToString()
        {
            return "Agent " + Agent.ID + " Prichod zakaznika(5) - " + OccurrenceTime;
        }
    }
}
