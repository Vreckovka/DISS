using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PriorityQueues;

namespace Simulations.Simulations.EventSimulation
{
    public abstract class SimulationCore
    {
        public TimeSpan SimulationTime { get; set; }
        public FibonacciHeap<SimulationEvent, TimeSpan> Calendar { get; set; } = new FibonacciHeap<SimulationEvent, TimeSpan>(PriorityQueueType.Minimum);
        public Queue<Agent> AgentQueue { get; set; } = new Queue<Agent>();
        public bool IsServiceOccupied { get; set; }

        TimeSpan _lastTime;

        protected abstract void BeforeSimulation();
        public abstract void Simulate();
        public TimeSpan EndTime { get; set; }
        public SimulationCore(TimeSpan startTime, TimeSpan endTime)
        {
            SimulationTime = startTime;
            EndTime = endTime;
        }
    }
}
