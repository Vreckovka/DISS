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
        public double SimulationTime { get; set; }
        public FibonacciHeap<SimulationEvent, double> Calendar { get; set; } = new FibonacciHeap<SimulationEvent, double>(PriorityQueueType.Minimum);
        public bool Cooling { get; set; }
        protected abstract void BeforeSimulation();
        protected abstract void AfterSimulation();
        public abstract double[] Simulate();
        protected double[] SimulationData { get; set; }
        public TimeSpan EndTime { get; set; }
        public TimeSpan StartTime { get; set; }
        public SimulationCore(TimeSpan startTime, TimeSpan endTime, bool cooling)
        {
            SimulationTime = startTime.TotalSeconds;
            StartTime = startTime;
            EndTime = endTime;
            Cooling = cooling;
        }
    }
}
