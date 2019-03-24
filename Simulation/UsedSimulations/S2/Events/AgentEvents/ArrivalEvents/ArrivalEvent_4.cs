using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Simulations.Simulations.EventSimulation;
using Simulations.UsedSimulations.Other;

namespace Simulations.UsedSimulations.S2.Events.AgentEvents.ArrivalEvents
{
    class ArrivalEvent_4 : ArrivalEvent
    {
        public ArrivalEvent_4(Agent agent, TimeSpan occurrenceTime, SimulationCore simulationCore) : base(agent, occurrenceTime, simulationCore)
        {
        }

        public override void Execute()
        {
            var core = (S2_SimulationCore)SimulationCore;

            var nextArrival = new ArrivalEvent_4(new Agent_S2(4),
                OccurrenceTime + TimeSpan.FromSeconds(core.arrivalGenerator_4.GetNext()),
                core);

            SimulationCore.Calendar.Enqueue(nextArrival, nextArrival.OccurrenceTime);

            DefaultArrivalExec();
        }
    }
}
