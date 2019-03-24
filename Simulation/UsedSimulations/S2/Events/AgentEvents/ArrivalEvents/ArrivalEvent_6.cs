using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Simulations.Simulations.EventSimulation;
using Simulations.UsedSimulations.Other;

namespace Simulations.UsedSimulations.S2.Events.AgentEvents.ArrivalEvents
{
    class ArrivalEvent_6 : ArrivalEvent
    {
        public ArrivalEvent_6(Agent agent, TimeSpan occurrenceTime, SimulationCore simulationCore) : base(agent,
            occurrenceTime, simulationCore)
        {
        }

        public override void Execute()
        {
            var core = (S2_SimulationCore) SimulationCore;

            var nextArrival = new ArrivalEvent_6(new Agent_S2(6),
                OccurrenceTime + TimeSpan.FromSeconds(core.arrivalGenerator_6.GetNext()),
                core);

            if (nextArrival.OccurrenceTime <= SimulationCore.EndTime)
                SimulationCore.Calendar.Enqueue(nextArrival, nextArrival.OccurrenceTime);

            DefaultArrivalExec();
        }
    }
}
