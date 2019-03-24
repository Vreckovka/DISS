using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Simulations.Simulations.EventSimulation;
using Simulations.UsedSimulations.Other;
using Simulations.UsedSimulations.S2.Events.WaiterEvents;

namespace Simulations.UsedSimulations.S2.Events.AgentEvents
{
    class EndEatingFood_Event : SimulationEvent
    {
        public EndEatingFood_Event(Agent agent, TimeSpan occurrenceTime, SimulationCore simulationCore) : base(agent, occurrenceTime, simulationCore)
        {
            ((Agent_S2)Agent).EndEatingFood = OccurrenceTime;
        }

        public override void Execute()
        {
            var core = (S2_SimulationCore)SimulationCore;

            core.AgentsWaitingForPaying.Enqueue(Agent);

            var freeWaiter = (from x in core.Waiters where x.Occupied == false select x).FirstOrDefault();

            if (freeWaiter != null)
                freeWaiter.Occupied = freeWaiter.MakeProperEvent(OccurrenceTime);
        }

        public override string ToString()
        {
            return $"Agent: {Agent}\t\t  Koniec jedenia  \t{OccurrenceTime}";
        }
    }
}
