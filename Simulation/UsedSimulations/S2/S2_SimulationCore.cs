﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PriorityQueues;
using Simulations.Distributions;
using Simulations.Simulations.EventSimulation;
using Simulations.UsedSimulations.Other;
using Simulations.UsedSimulations.S2.Events.AgentEvents.ArrivalEvents;

namespace Simulations.UsedSimulations.S2
{
    public class S2_SimulationCore : SimulationCore
    {
        #region Generators

        #region Arrivals distributions

        public ExponentialDistribution arrivalGenerator_1;
        public ExponentialDistribution arrivalGenerator_2;
        public ExponentialDistribution arrivalGenerator_3;
        public ExponentialDistribution arrivalGenerator_4;
        public ExponentialDistribution arrivalGenerator_5;
        public ExponentialDistribution arrivalGenerator_6;

        #endregion

        #region Food distributions
        public UniformDiscreetDistribution cezarSaladGenerator;
        public DiscreetEmpiricalDistribution3 penneSaladGenerator;
        public DiscreetEmpiricalDistribution3 wholeWheatSpaghettiGenerator;
        public int richSaladGenerator;
        public UniformContinuousDistribution deliveringFoodGenerator;
        #endregion
        public UniformContinuousDistribution waintingForOrderGenerator;
        public TriangularDistribution eatingFoodGenerator;
        public UniformContinuousDistribution payingGenerator;
        public Random pickFoodRandom;
        #endregion

        #region Statistics
        public int CountOfLeftAgents { get; set; }
        public int CountOfStayedAgents { get; set; }
        public int CountOfPaiedAgents { get; set; }
        public double WaitingTimeOfAgents { get; set; }
        #endregion

        public List<Table> Tables { get; set; } = new List<Table>();
        public List<Waiter> Waiters { get; set; } = new List<Waiter>();
        public List<Cook> Cooks { get; set; } = new List<Cook>();
        public Queue<Food> FoodsWaintingForCook { get; set; } = new Queue<Food>();
        public Queue<Agent> AgentsWaitingForOrder { get; set; } = new Queue<Agent>();
        public Queue<KeyValuePair<Agent, Food>> FoodsWaitingForDeliver { get; set; } = new Queue<KeyValuePair<Agent, Food>>();
        public Queue<Agent> AgentsWaitingForPaying { get; set; } = new Queue<Agent>();

        private void CreateDistributions(Random random)
        {
            arrivalGenerator_1 = new ExponentialDistribution(1.0 / ((60.0 / 10) * 60), random.Next());
            arrivalGenerator_2 = new ExponentialDistribution(1.0 / ((60.0 / 8) * 60), random.Next());
            arrivalGenerator_3 = new ExponentialDistribution(1.0 / ((60.0 / 6) * 60), random.Next());
            arrivalGenerator_4 = new ExponentialDistribution(1.0 / ((60.0 / 5) * 60), random.Next());
            arrivalGenerator_5 = new ExponentialDistribution(1.0 / ((60.0 / 3) * 60), random.Next());
            arrivalGenerator_6 = new ExponentialDistribution(1.0 / ((60.0 / 4) * 60), random.Next());


            cezarSaladGenerator = new UniformDiscreetDistribution(380, 440, random.Next());
            penneSaladGenerator =
                new DiscreetEmpiricalDistribution3(185, 330, 0.15, 331, 630, 0.5, 631, 930, random.Next());
            wholeWheatSpaghettiGenerator =
                new DiscreetEmpiricalDistribution3(290, 356, 0.2, 357, 540, 0.43, 541, 600, random.Next());

            richSaladGenerator = 180;

            waintingForOrderGenerator = new UniformContinuousDistribution(45, 120, random.Next());
            eatingFoodGenerator = new TriangularDistribution(random.Next(), 3 * 60, 30 * 60, 15 * 60);
            payingGenerator = new UniformContinuousDistribution(43, 97, random.Next());
            deliveringFoodGenerator = new UniformContinuousDistribution(23, 80, random.Next());

            pickFoodRandom = new Random(random.Next());

        }
        private void CreateTables()
        {
            for (int i = 0; i < 10; i++)
            {
                Tables.Add(new Table()
                {
                    Capacity = 2
                });
            }

            for (int i = 0; i < 7; i++)
            {
                Tables.Add(new Table()
                {
                    Capacity = 4
                });
            }

            for (int i = 0; i < 6; i++)
            {
                Tables.Add(new Table()
                {
                    Capacity = 6
                });
            }
        }
        protected override void BeforeSimulation()
        {
            CreateDistributions(new Random());
            CreateTables();

            Calendar.Enqueue(new ArrivalEvent_1(new Agent_S2(1), SimulationTime, this), SimulationTime);
            Calendar.Enqueue(new ArrivalEvent_2(new Agent_S2(2), SimulationTime, this), SimulationTime);
            Calendar.Enqueue(new ArrivalEvent_3(new Agent_S2(3), SimulationTime, this), SimulationTime);
            Calendar.Enqueue(new ArrivalEvent_4(new Agent_S2(4), SimulationTime, this), SimulationTime);
            Calendar.Enqueue(new ArrivalEvent_5(new Agent_S2(5), SimulationTime, this), SimulationTime);
            Calendar.Enqueue(new ArrivalEvent_6(new Agent_S2(6), SimulationTime, this), SimulationTime);
        }


        public override double[] Simulate()
        {
            BeforeSimulation();

            while (SimulationTime <= EndTime && Calendar.Count > 0)
            {
                SimulationEvent acutalEvent = Calendar.Dequeue();

                if (acutalEvent.OccurrenceTime > EndTime)
                    break;

                Console.WriteLine(acutalEvent);

                SimulationTime = acutalEvent.OccurrenceTime;
                acutalEvent.Execute();
            }

            return new double[]
            {
               (double) CountOfLeftAgents / (CountOfStayedAgents + CountOfLeftAgents),
               (double) WaitingTimeOfAgents / CountOfStayedAgents,
               CountOfStayedAgents,
               CountOfLeftAgents,
               CountOfPaiedAgents
            };
        }

        public S2_SimulationCore(TimeSpan startTime,
            TimeSpan endTime,
            int numberOfWaiters,
            int numberOfCooks) : base(startTime, endTime)
        {

            for (int i = 0; i < numberOfWaiters; i++)
            {
                Waiters.Add(new Waiter(this));
            }
            for (int i = 0; i < numberOfCooks; i++)
            {
                Cooks.Add(new Cook(this));
            }
        }
    }
}
