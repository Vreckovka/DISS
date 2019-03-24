using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Simulations.Simulations.EventSimulation;
using Simulations.UsedSimulations.Other;
using Simulations.UsedSimulations.S2.Events.WaiterEvents;

namespace Simulations.UsedSimulations.S2.Events.AgentEvents.ArrivalEvents
{
    abstract class ArrivalEvent : SimulationEvent
    {
        public ArrivalEvent(Agent agent, TimeSpan occurrenceTime, SimulationCore simulationCore) : base(agent, occurrenceTime, simulationCore)
        {
        }

        public  abstract override void Execute();

        protected void DefaultArrivalExec()
        {
            var core = (S2_SimulationCore)SimulationCore;
            var freeWaiter = (from x in core.Waiters where x.Occupied == false select x).FirstOrDefault();

            if (freeWaiter != null)
                SimulationCore.Calendar.Enqueue(new WaitingForWaiter_Event(Agent,
                        OccurrenceTime,
                        SimulationCore),
                    OccurrenceTime);
            else
            {
                core.AgentQueue.Enqueue(Agent);
            }
        }

        protected Table GetProperTable()
        {
            var core = (S2_SimulationCore)SimulationCore;
            var tables = (from x in core.Tables orderby x.Capacity where x.Occupied == false select x).ToList();

            var agentCount = ((Agent_S2) Agent).AgentCount;
            foreach (var table in tables)
            {
                if (table.Capacity >= agentCount)
                {
                    return table;
                }
            }

            return null;
        }

        public override string ToString()
        {
            return $"Agent: {Agent}\t\t  Prichod zakaznika  \t{OccurrenceTime}";
        }

    }
}
