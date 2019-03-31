using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PriorityQueues;
using PropertyChanged;

namespace Simulations.Simulations.EventSimulation
{
    [AddINotifyPropertyChangedInterface]
    public abstract class SimulationCore : BaseSimulation
    {
        public double SimulationTime { get; set; }
        public BinaryHeap<SimulationEvent, double> Calendar { get; set; } = new BinaryHeap<SimulationEvent, double>(PriorityQueueType.Minimum);
        public bool Cooling { get; set; }
        public abstract void BeforeSimulation(TimeSpan startTime, TimeSpan endTime, int numberOfWaiters, int numberOfCooks, bool cooling);
        protected abstract double[] CalculateStatistics();
        public abstract double[] Simulate(TimeSpan startTime,TimeSpan endTime, int numberOfWaiters, int numberOfCooks, bool cooling, bool run);
        public abstract double[] SimulateRuns(TimeSpan startTime, TimeSpan endTime, int numberOfWaiters, int numberOfCooks, bool cooling, int numberOfReplications);
        protected double[] SimulationResult { get; set; }
        public TimeSpan EndTime { get; set; }
        public TimeSpan StartTime { get; set; }
        public SimulationCore()
        {
        }
    }
}
