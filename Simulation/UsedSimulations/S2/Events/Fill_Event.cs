using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Simulations.UsedSimulations.Other;

namespace Simulations.UsedSimulations.S2.Events
{
    class Fill_Event : Event_S2
    {
        public Fill_Event(Agent_S2 agent, double occurrenceTime, SimulationCore_S2 simulationCore) : base(agent, occurrenceTime, simulationCore)
        {
        }

        public override void Execute()
        {
            
        }

        public override string ToString()
        {
            return $"Fill {OccurrenceTime}";
        }
    }
}
