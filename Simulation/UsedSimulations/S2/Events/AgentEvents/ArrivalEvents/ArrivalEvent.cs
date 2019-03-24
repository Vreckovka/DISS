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
        public ArrivalEvent(Agent agent,
            TimeSpan occurrenceTime,
            SimulationCore simulationCore) : base(agent, occurrenceTime, simulationCore)
        {
            ((Agent_S2)Agent).ArrivalTime = OccurrenceTime;
        }

        public abstract override void Execute();

        protected void DefaultArrivalExec()
        {
            var core = (S2_SimulationCore)SimulationCore;
            var table = GetProperTable();

            if (table == null)
            {
                core.CountOfLeftAgents += ((Agent_S2)Agent).AgentCount;
                //Console.WriteLine($"Agent {Agent} ------------ ZAKAZNIK ODISIEL ---------------");
                return;
            }
            else
            {
                table.Occupied = true;
                ((Agent_S2)Agent).Table = table;
                core.CountOfStayedAgents += ((Agent_S2)Agent).AgentCount;
            }

            core.AgentsWaitingForOrder.Enqueue(Agent);

            var freeWaiter = (from x in core.Waiters where x.Occupied == false select x).FirstOrDefault();

            if (freeWaiter != null)
                freeWaiter.Occupied = freeWaiter.MakeProperEvent(OccurrenceTime);
        }

        protected Table GetProperTable()
        {
            var core = (S2_SimulationCore)SimulationCore;

            var table = (from x in core.Tables
                         orderby x.Capacity
                         where x.Occupied == false
                         where x.Capacity >= ((Agent_S2)Agent).AgentCount
                         select x).FirstOrDefault();


            if (table != null)
            {
                return table;
            }

            return null;
        }

        public override string ToString()
        {
            return $"Agent: {Agent}\t\t  Prichod zakaznika  \t{OccurrenceTime}";
        }

    }
}
