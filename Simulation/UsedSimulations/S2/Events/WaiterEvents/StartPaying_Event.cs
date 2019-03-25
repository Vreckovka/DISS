using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Simulations.Simulations.EventSimulation;
using Simulations.UsedSimulations.Other;

namespace Simulations.UsedSimulations.S2.Events.WaiterEvents
{
    class StartPaying_Event : SimulationEvent
    {
        public Waiter Waiter { get; set; }

        public StartPaying_Event(Agent agent,
            TimeSpan occurrenceTime,
            SimulationCore simulationCore,
            Waiter waiter) : base(agent, occurrenceTime, simulationCore)
        {
            Waiter = waiter;
        }


        public override void Execute()
        {
            var core = (S2_SimulationCore)SimulationCore;
            ((Agent_S2)Agent).StartPaying = OccurrenceTime;

            var @event = new EndPaying_Event(Agent,
                            OccurrenceTime + TimeSpan.FromSeconds(core.payingGenerator.GetNext()),
                            core,
                            Waiter);

            core.Calendar.Enqueue(@event, @event.OccurrenceTime);

            Waiter.Occupied = true;

            
        }

        public override string ToString()
        {
            return $"Agent: {Agent}, Obsluha: {Waiter.Id}  Zaciatok platenia  \t{OccurrenceTime}";
        }
    }
}
