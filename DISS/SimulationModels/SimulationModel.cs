using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using DISS.Annotations;
using PropertyChanged;
using Simulation;

namespace DISS.SimulationModels
{
    public abstract class SimulationModel 
    {
        public abstract void SetSimulationSpeed(int speed);
        public abstract void ResumeSimulation();
        public abstract void SimulationFinished();
        public abstract double[] StartSimulation(Random random, int numberOfWaiters, int numberOfCooks, bool cooling);
        public abstract void StartRuns(int runsCount,int replicationCount, int numberOfWaiter, int numberOfCooks, bool cooling);
        public abstract void PauseSimulation();
    }
}
