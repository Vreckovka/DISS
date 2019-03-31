using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Simulations.Simulations
{
    public abstract class BaseSimulation
    {
        protected EventWaitHandle waitHandle = new ManualResetEvent(true);
        public event EventHandler<double[]> SimulationFinished;
        public event EventHandler<double[]> RunFinished;
        protected bool pause;
        public int SimulationDelay { get; set; }

        protected virtual void OnRunFinished(double[] e)
        {
            RunFinished?.Invoke(this, e);
        }

        protected virtual void OnSimulationFinished(double[] e)
        {
            SimulationFinished?.Invoke(this, e);
        }

        public void OnPauseClick()
        {
            pause = true;
            waitHandle.Reset();
        }

        public void OnResumeClick()
        {
            pause = false;
            waitHandle.Set();
        }

        protected void ManageSimulationSpeed()
        {
            Thread.SpinWait((int)Math.Pow(SimulationDelay,3));
        }
    }
}
