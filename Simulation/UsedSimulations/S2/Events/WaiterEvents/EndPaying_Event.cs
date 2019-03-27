using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Simulations.Simulations.EventSimulation;
using Simulations.UsedSimulations.Other;

namespace Simulations.UsedSimulations.S2.Events.WaiterEvents
{
    class EndPaying_Event : SimulationEvent
    {
        public Waiter Waiter { get; set; }
        public EndPaying_Event(Agent agent,
            TimeSpan occurrenceTime,
            SimulationCore simulationCore,
            Waiter waiter) : base(agent, occurrenceTime, simulationCore)
        {
            Waiter = waiter;
        }

        public override void Execute()
        {
            var core = (S2_SimulationCore)SimulationCore;

            core.WaitingTimeOfAgents += (((Agent_S2)Agent).StartOrder - ((Agent_S2)Agent).ArrivalTime).TotalSeconds * ((Agent_S2)Agent).AgentCount;
            core.WaitingTimeOfAgents += (((Agent_S2)Agent).DeliveredFood - ((Agent_S2)Agent).EndOrder).TotalSeconds * ((Agent_S2)Agent).AgentCount;
            core.WaitingTimeOfAgents += (((Agent_S2)Agent).StartPaying - ((Agent_S2)Agent).EndEatingFood).TotalSeconds * ((Agent_S2)Agent).AgentCount;
                        
            core.CountOfPaiedAgents += ((Agent_S2)Agent).AgentCount;

            Waiter.Occupied = false;
            core.FreeWaiters.Enqueue(Waiter);

            ((Agent_S2)Agent).Table.Occupied = false;

            core.CheckWaiters(OccurrenceTime);
        }

        public override string ToString()
        {
            return $"Agent: {Agent}, Obsluha: {Waiter.Id}  Koniec platenia  \t{OccurrenceTime}";
        }
    }
}
