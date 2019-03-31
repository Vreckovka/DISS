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
    class Arrival_Event : Event_S2
    {
        public Arrival_Event(Agent_S2 agent,
            double occurrenceTime,
            SimulationCore_S2 simulationCore) : base(agent, occurrenceTime, simulationCore)
        {
        }

        public override void Execute()
        {
            Arrival_Event nextArrival = null;

            switch (Agent.AgentCount)
            {
                case 1:
                    nextArrival = new Arrival_Event(new Agent_S2(1),
                        OccurrenceTime + SimulationCore.arrivalGenerator_1.GetNext(), SimulationCore);
                    break;
                case 2:
                    nextArrival = new Arrival_Event(new Agent_S2(2),
                        OccurrenceTime + SimulationCore.arrivalGenerator_2.GetNext(), SimulationCore);
                    break;
                case 3:
                    nextArrival = new Arrival_Event(new Agent_S2(3),
                        OccurrenceTime + SimulationCore.arrivalGenerator_3.GetNext(), SimulationCore);
                    break;
                case 4:
                    nextArrival = new Arrival_Event(new Agent_S2(4),
                        OccurrenceTime + SimulationCore.arrivalGenerator_4.GetNext(), SimulationCore);
                    break;
                case 5:
                    nextArrival = new Arrival_Event(new Agent_S2(5),
                        OccurrenceTime + SimulationCore.arrivalGenerator_5.GetNext(), SimulationCore);
                    break;
                case 6:
                    nextArrival = new Arrival_Event(new Agent_S2(6),
                        OccurrenceTime + SimulationCore.arrivalGenerator_6.GetNext(), SimulationCore);
                    break;
                default:
                    throw new ArgumentException("Non valid agent count");
            }

            if (nextArrival.OccurrenceTime <= SimulationCore.EndTime.TotalSeconds)
            {
                SimulationCore.Calendar.Enqueue(nextArrival, nextArrival.OccurrenceTime);
            }

            DefaultArrivalExec();
        }

        protected void DefaultArrivalExec()
        {
            var table = GetProperTable();
            Agent.ArrivalTime = OccurrenceTime;

            if (table == null)
            {
                SimulationCore.CountOfLeftAgents += Agent.AgentCount;
                return;
            }
            else
            {
                table.Occupied = true;
                Agent.Table = table;

                SimulationCore.CountOfStayedAgents += Agent.AgentCount;
                SimulationCore.AgentsWaitingForOrder.Enqueue(Agent);
                SimulationCore.CountOfWaitingAgents_Order += Agent.AgentCount;
                
                SimulationCore.CheckWaiters(OccurrenceTime);
                SimulationCore.ChangeTableStats(OccurrenceTime, table, false);
            }
        }

        protected Table GetProperTable()
        {
            return (from x in SimulationCore.Tables
                    where x.Occupied == false
                    where x.Capacity >= Agent.AgentCount
                    select x).FirstOrDefault();
        }

        public override string ToString()
        {
            return $"Agent: {Agent}\t\t  Prichod zakaznika  \t{OccurrenceTime}";
        }

    }
}
