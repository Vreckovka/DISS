using System;
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
    public class SimulationCore_S2 : SimulationCore
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
        public DiscreetEmpiricalDistribution penneSaladGenerator;
        public DiscreetEmpiricalDistribution wholeWheatSpaghettiGenerator;
        public int richSaladGenerator;
        public UniformContinuousDistribution deliveringFoodGenerator;
        #endregion
        public UniformContinuousDistribution waintingForOrderGenerator;
        public TriangularDistribution eatingFoodGenerator;
        public UniformContinuousDistribution payingGenerator;
        public Random pickFoodRandom;
        #endregion

        #region Statistics
        #region Agent Statistics
        public int CountOfLeftAgents { get; set; }
        public int CountOfStayedAgents { get; set; }
        public int CountOfPaiedAgents { get; set; }
        public double WaitingTimeOfAgents { get; set; }
        #endregion

        #region Waiter Statistics

        private double _lastChangeTimeFreeWaiters;
        private double _avrageSumFreeWaiters;
        private double _avrageWeightOfFreeWaiters;

        #endregion

        #region Cooks Statistics

        private double _lastChangeTimeFreeCooks;
        private double _avrageSumFreeCooks;
        private double _avrageWeightOfFreeCooks;

        #endregion

        #region Table Statistics

        private double _lastChangeTimeTable_2;
        private double _lastChangeTimeTable_4;
        private double _lastChangeTimeTable_6;

        private double _avrageSumTables_2;
        private double _avrageWeightOfFreeTables_2;

        private double _avrageSumTables_4;
        private double _avrageWeightOfFreeTables_4;

        private double _avrageSumTables_6;
        private double _avrageWeightOfFreeTables_6;

        private int _countOfFreeTables_2;
        private int _countOfFreeTables_4;
        private int _countOfFreeTables_6;
        #endregion
        #endregion

        #region Properties
        public List<Table> Tables { get; set; } = new List<Table>();
        public List<Waiter> Waiters { get; set; } = new List<Waiter>();
        public List<Cook> Cooks { get; set; } = new List<Cook>();
        public Queue<Cook> FreeCooks { get; set; }
        public FibonacciHeap<Waiter, double> FreeWaiters { get; set; }
        public Queue<Food> FoodsWaintingForCook { get; set; } = new Queue<Food>();
        public Queue<Agent_S2> AgentsWaitingForOrder { get; set; } = new Queue<Agent_S2>();
        public Queue<Agent_S2> AgentsWaitingForDeliver { get; set; } = new Queue<Agent_S2>();
        public Queue<Agent_S2> AgentsWaitingForPaying { get; set; } = new Queue<Agent_S2>();
        #endregion

        #region Before Simulation Methods
        protected override void BeforeSimulation()
        {
            CreateDistributions(new Random());
            CreateTables();

            _lastChangeTimeFreeWaiters = StartTime.TotalSeconds;
            _lastChangeTimeFreeCooks = StartTime.TotalSeconds;

            var time_1 = arrivalGenerator_1.GetNext();
            var time_2 = arrivalGenerator_2.GetNext();
            var time_3 = arrivalGenerator_3.GetNext();
            var time_4 = arrivalGenerator_4.GetNext();
            var time_5 = arrivalGenerator_5.GetNext();
            var time_6 = arrivalGenerator_6.GetNext();


            Calendar.Enqueue(new Arrival_Event(new Agent_S2(1), time_1 + SimulationTime, this), time_1 + SimulationTime);
            Calendar.Enqueue(new Arrival_Event(new Agent_S2(2), time_2 + SimulationTime, this), time_2 + SimulationTime);
            Calendar.Enqueue(new Arrival_Event(new Agent_S2(3), time_3 + SimulationTime, this), time_3 + SimulationTime);
            Calendar.Enqueue(new Arrival_Event(new Agent_S2(4), time_4 + SimulationTime, this), time_4 + SimulationTime);
            Calendar.Enqueue(new Arrival_Event(new Agent_S2(5), time_5 + SimulationTime, this), time_5 + SimulationTime);
            Calendar.Enqueue(new Arrival_Event(new Agent_S2(6), time_6 + SimulationTime, this), time_6 + SimulationTime);
        }
        private void CreateDistributions(Random random)
        {
            arrivalGenerator_1 = new ExponentialDistribution(1.0 / ((60.0 / 10) * 60), random.Next());
            arrivalGenerator_2 = new ExponentialDistribution(1.0 / ((60.0 / 8) * 60), random.Next());
            arrivalGenerator_3 = new ExponentialDistribution(1.0 / ((60.0 / 6) * 60), random.Next());
            arrivalGenerator_4 = new ExponentialDistribution(1.0 / ((60.0 / 5) * 60), random.Next());
            arrivalGenerator_5 = new ExponentialDistribution(1.0 / ((60.0 / 3) * 60), random.Next());
            arrivalGenerator_6 = new ExponentialDistribution(1.0 / ((60.0 / 4) * 60), random.Next());


            cezarSaladGenerator = new UniformDiscreetDistribution(380, 440, random.Next());

            var penneSaladGeneratorData = new List<DiscreetEmpiricalDistributionData>()
            {
                new DiscreetEmpiricalDistributionData(185, 330, 0.15),
                new DiscreetEmpiricalDistributionData(331, 630, 0.5),
                new DiscreetEmpiricalDistributionData(631, 930, 0.35),
            };

            penneSaladGenerator = new DiscreetEmpiricalDistribution(penneSaladGeneratorData, random.Next());

            var wholeWheatSpaghettiGeneratorData = new List<DiscreetEmpiricalDistributionData>()
            {
                new DiscreetEmpiricalDistributionData(290, 356, 0.2),
                new DiscreetEmpiricalDistributionData(357, 540, 0.43),
                new DiscreetEmpiricalDistributionData(541, 600, 0.37),
            };


            wholeWheatSpaghettiGenerator = new DiscreetEmpiricalDistribution(wholeWheatSpaghettiGeneratorData, random.Next());
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

            _countOfFreeTables_2 = 10;

            for (int i = 0; i < 7; i++)
            {
                Tables.Add(new Table()
                {
                    Capacity = 4
                });
            }
            _countOfFreeTables_4 = 7;

            for (int i = 0; i < 6; i++)
            {
                Tables.Add(new Table()
                {
                    Capacity = 6
                });
            }
            _countOfFreeTables_6 = 6;

            _lastChangeTimeTable_2 = StartTime.TotalSeconds;
            _lastChangeTimeTable_4 = StartTime.TotalSeconds;
            _lastChangeTimeTable_6 = StartTime.TotalSeconds;
        }
        #endregion

        #region Simulation Methods
        public bool CheckCooksEvents()
        {
            if (FoodsWaintingForCook.Count > 0)
            {
                return true;
            }
            return false;
        }
        public void CheckCooks(double OccurrenceTime)
        {
            if (FreeCooks.Count > 0)
            {
                if (CheckCooksEvents())
                {
                    ChangeCooksStats(OccurrenceTime);

                    var freeCook = FreeCooks.Dequeue();
                    freeCook.MakeProperEvent(OccurrenceTime);
                }
            }
        }

        public bool CheckWaiterEvents()
        {
            if (AgentsWaitingForOrder.Count != 0 || AgentsWaitingForDeliver.Count != 0 || AgentsWaitingForPaying.Count != 0)
            {
                return true;
            }
            return false;
        }
        public void CheckWaiters(double OccurrenceTime)
        {
            if (FreeWaiters.Count > 0)
            {
                if (CheckWaiterEvents())
                {
                    ChangeWaitersStats(OccurrenceTime);

                    var freeWaiter = FreeWaiters.Dequeue();
                    freeWaiter.MakeProperEvent(OccurrenceTime);
                }
            }
        }
        #endregion

        #region Statistics Methods
        public void ChangeTableStats(double occurenceTime, Table table, bool left)
        {
            switch (table.Capacity)
            {
                case 2:
                    _avrageSumTables_2 += (occurenceTime - _lastChangeTimeTable_2) * _countOfFreeTables_2;
                    _avrageWeightOfFreeTables_2 += occurenceTime - _lastChangeTimeTable_2;

                    if (left)
                    {
                        _countOfFreeTables_2++;
                    }
                    else
                    {
                        _countOfFreeTables_2--;
                    }

                    _lastChangeTimeTable_2 = occurenceTime;
                    return;
                case 4:
                    _avrageSumTables_4 += (occurenceTime - _lastChangeTimeTable_4) * _countOfFreeTables_4;
                    _avrageWeightOfFreeTables_4 += occurenceTime - _lastChangeTimeTable_4;

                    if (left)
                    {
                        _countOfFreeTables_4++;
                    }
                    else
                    {
                        _countOfFreeTables_4--;
                    }
                    _lastChangeTimeTable_4 = occurenceTime;
                    return;
                case 6:
                    _avrageSumTables_6 += (occurenceTime - _lastChangeTimeTable_6) * _countOfFreeTables_6;
                    _avrageWeightOfFreeTables_6 += occurenceTime - _lastChangeTimeTable_6;

                    if (left)
                    {
                        _countOfFreeTables_6++;
                    }
                    else
                    {
                        _countOfFreeTables_6--;
                    }
                    _lastChangeTimeTable_6 = occurenceTime;
                    return;
            }
        }
        public void ChangeWaitersStats(double occurenceTime)
        {
            _avrageSumFreeWaiters += (occurenceTime - _lastChangeTimeFreeWaiters) * FreeWaiters.Count;
            _avrageWeightOfFreeWaiters += occurenceTime - _lastChangeTimeFreeWaiters;

            _lastChangeTimeFreeWaiters = occurenceTime;
        }
        public void ChangeCooksStats(double occurenceTime)
        {
            _avrageSumFreeCooks += (occurenceTime - _lastChangeTimeFreeCooks) * FreeCooks.Count;
            _avrageWeightOfFreeCooks += occurenceTime - _lastChangeTimeFreeCooks;

            _lastChangeTimeFreeCooks = occurenceTime;
        }
        #endregion

        #region Calculate Statistic methods

        private double CalculateFreeTimeOfWaiters()
        {
            double workTime = 0;
            foreach (var waiter in Waiters)
            {
                workTime += waiter.WorkedTime;
            }

            var simTimeSeconds = (EndTime - StartTime).TotalSeconds;
            return ((workTime / Waiters.Count) * 100) / simTimeSeconds;
        }
        private double CalculateFreeTimeOfCooks()
        {
            double workTime = 0;
            foreach (var cook in Cooks)
            {
                workTime += cook.WorkedTime;
            }

            var simTimeSeconds = (EndTime - StartTime).TotalSeconds;
            return ((workTime / Cooks.Count) * 100) / simTimeSeconds;
        }
        private double CalculateAvrageWaitingTime()
        {
            return WaitingTimeOfAgents / CountOfPaiedAgents;
        }
        private double CalculateAvrageCountOfLeftAgents()
        {
            return ((double)CountOfLeftAgents / (CountOfStayedAgents + CountOfLeftAgents)) * 100;
        }
        private double CalculateAvrageCountOfFreeWaiters()
        {
            return _avrageSumFreeWaiters / _avrageWeightOfFreeWaiters;
        }
        private double CalculateAvrageCountOfFreeCooks()
        {
            return _avrageSumFreeCooks / _avrageWeightOfFreeCooks;
        }
        private double CalculateAvrageCountOfFreeTables_2()
        {
            return _avrageSumTables_2 / _avrageWeightOfFreeTables_2;
        }
        private double CalculateAvrageCountOfFreeTables_4()
        {
            return _avrageSumTables_4 / _avrageWeightOfFreeTables_4;
        }
        private double CalculateAvrageCountOfFreeTables_6()
        {
            return _avrageSumTables_6 / _avrageWeightOfFreeTables_6;
        }
        #endregion
        protected override void AfterSimulation()
        {
            SimulationData = new double[]
            {
                CalculateAvrageCountOfLeftAgents(),
                CalculateAvrageWaitingTime(),
                CalculateFreeTimeOfWaiters(),
                CalculateAvrageCountOfFreeWaiters(),
                CalculateFreeTimeOfCooks(),
                CalculateAvrageCountOfFreeCooks(),
                CalculateAvrageCountOfFreeTables_2(),
                CalculateAvrageCountOfFreeTables_4(),
                CalculateAvrageCountOfFreeTables_6()
            };
        }

        public override double[] Simulate()
        {
            BeforeSimulation();

            while (Calendar.Count > 0)
            {
                SimulationEvent acutalEvent = Calendar.Dequeue();
                SimulationTime = acutalEvent.OccurrenceTime;

                acutalEvent.Execute();

                if (!Cooling)
                {
                    if (SimulationTime >= EndTime.TotalSeconds)
                        break;
                }
            }

            AfterSimulation();
            return SimulationData;
        }

        public SimulationCore_S2(TimeSpan startTime,
            TimeSpan endTime,
            int numberOfWaiters,
            int numberOfCooks,
            bool cooling) : base(startTime, endTime, cooling)
        {
            FreeWaiters = new FibonacciHeap<Waiter, double>(PriorityQueueType.Minimum);
            FreeCooks = new Queue<Cook>();

            for (int i = 0; i < numberOfWaiters; i++)
            {
                var waiter = new Waiter(this);

                FreeWaiters.Enqueue(waiter, 0);
                Waiters.Add(waiter);
            }
            for (int i = 0; i < numberOfCooks; i++)
            {
                var cook = new Cook(this);

                FreeCooks.Enqueue(cook);
                Cooks.Add(cook);
            }
        }
    }
}
