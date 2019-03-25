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
        public bool IsServiceOccupied { get; set; }

        TimeSpan _lastTime;
        public bool Cooling { get; set; }
        protected abstract void BeforeSimulation();
        public abstract double[] Simulate();
        public TimeSpan EndTime { get; set; }
        public SimulationCore(TimeSpan startTime, TimeSpan endTime, bool cooling)
        {
            SimulationTime = startTime;
            EndTime = endTime;
            Cooling = cooling;
        }
    }
}
