using System;
using System.Linq;
using Simulations.Simulations.EventSimulation;
using Simulations.UsedSimulations.Other;
using Simulations.UsedSimulations.S2.Events.WaiterEvents;

namespace Simulations.UsedSimulations.S2.Events.AgentEvents.ArrivalEvents
{
    class ArrivalEvent_1 : ArrivalEvent
    {
        public ArrivalEvent_1(Agent agent,
            TimeSpan occurrenceTime,
            SimulationCore simulationCore) : base(agent, occurrenceTime, simulationCore)
        {
        }


        public override void Execute()
        {
            var core = (S2_SimulationCore)SimulationCore;

            var nextArrival = new ArrivalEvent_1(new Agent_S2(1),
                OccurrenceTime + TimeSpan.FromSeconds(core.arrivalGenerator_1.GetNext()),
                core);

            SimulationCore.Calendar.Enqueue(nextArrival, nextArrival.OccurrenceTime);

            var table = GetProperTable();

            if (table == null)
            {
                core.CountOfLeftAgents++;
            }
            else
            {
                table.Occupied = true;
                ((Agent_S2)Agent).Table = table;
            }

            DefaultArrivalExec();
        }


    }
}
