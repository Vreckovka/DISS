using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using DISS.Annotations;
using OxyPlot;
using PropertyChanged;
using Simulations.UsedSimulations.S2;

namespace DISS.SimulationModels
{
    [AddINotifyPropertyChangedInterface]
    public class SimulationModel_2 : SimulationModel
    {
        public SimulationCore_S2 Simulation { get; set; }
        public SimulationModel_2()
        {
            Simulation = new SimulationCore_S2();
        }

        public override void SetSimulationSpeed(int speed)
        {
            Simulation.SimulationDelay = speed;
        }

        public override void ResumeSimulation()
        {
            Simulation.OnResumeClick();
        }

        public override void SimulationFinished()
        {
            //throw new NotImplementedException();
        }

        public override double[] StartSimulation(Random random, int numberOfWaiters, int numberOfCooks,bool cooling)
        {
            return Simulation.Simulate(new TimeSpan(11, 0, 0), new TimeSpan(20, 0, 0), numberOfWaiters, numberOfCooks, cooling, false);
        }

        public override void StartRuns(int runsCount, int replicationCount, int numberOfWaiter, int numberOfCooks, bool cooling)
        {
            Simulation.SimulateRuns(new TimeSpan(11, 0, 0), new TimeSpan(20, 0, 0), numberOfWaiter, numberOfCooks,
                cooling, replicationCount);
        }

        public override void PauseSimulation()
        {
            Simulation.OnPauseClick();
        }
    }
}
