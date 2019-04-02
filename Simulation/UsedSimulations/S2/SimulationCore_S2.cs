using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using LiveCharts;
using PriorityQueues;
using PropertyChanged;
using Simulations.Distributions;
using Simulations.Simulations.EventSimulation;
using Simulations.UsedSimulations.Other;
using Simulations.UsedSimulations.S2.Events;
using Simulations.UsedSimulations.S2.Events.AgentEvents.ArrivalEvents;

namespace Simulations.UsedSimulations.S2
{
    [AddINotifyPropertyChangedInterface]
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
        public ChartValues<double> WaitingTimes { get; set; }

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
        public List<Table> Tables { get; set; }
        public List<Waiter> Waiters { get; set; }
        public List<Cook> Cooks { get; set; }
        public Queue<Cook> FreeCooks { get; set; }
        public BinaryHeap<Waiter, double> FreeWaiters { get; set; }
        public Queue<Food> FoodsWaintingForCook { get; set; }
        public Queue<Agent_S2> AgentsWaitingForOrder { get; set; }
        public Queue<Agent_S2> AgentsWaitingForDeliver { get; set; }
        public Queue<Agent_S2> AgentsWaitingForPaying { get; set; }
        public int CountOfWaitingAgents_Order { get; set; }
        public int CountOfWaitingAgents_Deliver { get; set; }
        public int CountOfWaitingAgents_Pay { get; set; }
        public bool LiveSimulation { get; set; }

        public double[] SimulationResult { get; set; }

        #region Confidence intervals

        public ConfidenceInterval.ConfidenceInterval.SampleStandardDeviationData AvrageCountOfLeftAgents_ConfindenceInterval { get; set; }
        public ConfidenceInterval.ConfidenceInterval.SampleStandardDeviationData AvrageWaiting_ConfindenceInterval { get; set; }

        public ConfidenceInterval.ConfidenceInterval.SampleStandardDeviationData FreeTimeOfWaiters_ConfindenceInterval { get; set; }
        public ConfidenceInterval.ConfidenceInterval.SampleStandardDeviationData AvrageCountOfFreeWaiters_ConfindenceInterval { get; set; }

        public ConfidenceInterval.ConfidenceInterval.SampleStandardDeviationData FreeTimeOfCooks_ConfindenceInterval { get; set; }
        public ConfidenceInterval.ConfidenceInterval.SampleStandardDeviationData AvrageCountOfFreeCooks_ConfindenceInterval { get; set; }


        public ConfidenceInterval.ConfidenceInterval.SampleStandardDeviationData AvrageCountOfFreeTables_2_ConfindenceInterval { get; set; }
        public ConfidenceInterval.ConfidenceInterval.SampleStandardDeviationData AvrageCountOfFreeTables_4_ConfindenceInterval { get; set; }
        public ConfidenceInterval.ConfidenceInterval.SampleStandardDeviationData AvrageCountOfFreeTables_6_ConfindenceInterval { get; set; }


        #endregion

        #endregion

        #region Before Simulation Methods
        public override void BeforeSimulation(TimeSpan startTime, TimeSpan endTime, int numberOfWaiters, int numberOfCooks, bool cooling)
        {
            RefreshSimulation();

            StartTime = startTime;
            EndTime = endTime;
            SimulationTime = startTime.TotalSeconds;
            Cooling = cooling;

            FreeWaiters = new BinaryHeap<Waiter, double>(PriorityQueueType.Minimum);
            FreeCooks = new Queue<Cook>();
            Tables = new List<Table>();
            Waiters = new List<Waiter>();
            Cooks = new List<Cook>();


            FoodsWaintingForCook = new Queue<Food>();
            AgentsWaitingForOrder = new Queue<Agent_S2>();
            AgentsWaitingForDeliver = new Queue<Agent_S2>();
            AgentsWaitingForPaying = new Queue<Agent_S2>();

            CreateDistributions(new Random());

          

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

            CreateTables(10, 7, 6);

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

            WaitingTimes = new ChartValues<double>();


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
                new DiscreetEmpiricalDistributionData(185, 330, 0.15, random.Next()),
                new DiscreetEmpiricalDistributionData(331, 630, 0.5,random.Next()),
                new DiscreetEmpiricalDistributionData(631, 930, 0.35,random.Next()),
            };

            penneSaladGenerator = new DiscreetEmpiricalDistribution(penneSaladGeneratorData, random.Next());

            var wholeWheatSpaghettiGeneratorData = new List<DiscreetEmpiricalDistributionData>()
            {
                new DiscreetEmpiricalDistributionData(290, 356, 0.2,random.Next()),
                new DiscreetEmpiricalDistributionData(357, 540, 0.43,random.Next()),
                new DiscreetEmpiricalDistributionData(541, 600, 0.37,random.Next()),
            };


            wholeWheatSpaghettiGenerator = new DiscreetEmpiricalDistribution(wholeWheatSpaghettiGeneratorData, random.Next());
            richSaladGenerator = 180;

            waintingForOrderGenerator = new UniformContinuousDistribution(45, 120, random.Next());
            eatingFoodGenerator = new TriangularDistribution(random.Next(), 3 * 60, 30 * 60, 15 * 60);
            payingGenerator = new UniformContinuousDistribution(43, 97, random.Next());
            deliveringFoodGenerator = new UniformContinuousDistribution(23, 80, random.Next());

            pickFoodRandom = new Random(random.Next());
        }
        private void CreateTables(int tables_2, int tables_4, int tables_6)
        {
            for (int i = 0; i < tables_2; i++)
            {
                Tables.Add(new Table()
                {
                    Capacity = 2
                });
            }

            _countOfFreeTables_2 = tables_2;

            for (int i = 0; i < tables_4; i++)
            {
                Tables.Add(new Table()
                {
                    Capacity = 4
                });
            }
            _countOfFreeTables_4 = tables_4;

            for (int i = 0; i < tables_6; i++)
            {
                Tables.Add(new Table()
                {
                    Capacity = 6
                });
            }
            _countOfFreeTables_6 = tables_6;

            _lastChangeTimeTable_2 = StartTime.TotalSeconds;
            _lastChangeTimeTable_4 = StartTime.TotalSeconds;
            _lastChangeTimeTable_6 = StartTime.TotalSeconds;

            Tables = Tables.OrderBy(x => x.Capacity).ToList();
        }
        private void RefreshSimulation()
        {
            Waiter.Count = 0;
            Cook.Count = 0;
            _lastTime = 0;

            foreach (PropertyInfo prop in typeof(SimulationCore_S2).GetProperties())
            {
                if (prop.Name != "SimulationDelay" && prop.Name != "ActualReplication")
                {
                    if (prop.PropertyType.Name == "Double" || prop.PropertyType.Name == "Int32")
                        prop.SetValue(this, 0);
                }
            }
        }
        #endregion

        #region Simulation Methods
        public void CheckCooks(double OccurrenceTime)
        {
            if (FreeCooks.Count > 0)
            {
                if (FoodsWaintingForCook.Count > 0)
                {
                    ChangeCooksStats(OccurrenceTime);

                    var freeCook = FreeCooks.Dequeue();
                    freeCook.MakeProperEvent(OccurrenceTime);
                }
            }
        }
        public void CheckWaiters(double OccurrenceTime)
        {
            if (FreeWaiters.Count > 0)
            {
                if (AgentsWaitingForOrder.Count != 0 || AgentsWaitingForDeliver.Count != 0 || AgentsWaitingForPaying.Count != 0)
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
                    _avrageWeightOfFreeTables_6 += (occurenceTime - _lastChangeTimeTable_6);

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

        private void GetSamplesForConfidenceInterval(double[] stats)
        {
                AvrageCountOfLeftAgents_ConfindenceInterval.AddValue(stats[0]);

                AvrageWaiting_ConfindenceInterval.AddValue(stats[1]);

                FreeTimeOfWaiters_ConfindenceInterval.AddValue(stats[2]);

                AvrageCountOfFreeWaiters_ConfindenceInterval.AddValue(stats[3]);

                FreeTimeOfCooks_ConfindenceInterval.AddValue(stats[4]);

                AvrageCountOfFreeCooks_ConfindenceInterval.AddValue(stats[5]);

                AvrageCountOfFreeTables_2_ConfindenceInterval.AddValue(stats[6]);

                AvrageCountOfFreeTables_4_ConfindenceInterval.AddValue(stats[7]);

                AvrageCountOfFreeTables_6_ConfindenceInterval.AddValue(stats[8]);
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
            return  100 - (((workTime / Waiters.Count) * 100) / simTimeSeconds);
        }
        private double CalculateFreeTimeOfCooks()
        {
            double workTime = 0;
            foreach (var cook in Cooks)
            {
                workTime += cook.WorkedTime;
            }

            var simTimeSeconds = (EndTime - StartTime).TotalSeconds;
            return 100 - (((workTime / Cooks.Count) * 100) / simTimeSeconds);
        }
        private double CalculateAvrageWaitingTime()
        {
            if (LiveSimulation)
                return WaitingTimeOfAgents / CountOfStayedAgents;
            else
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

        protected override double[] CalculateStatistics()
        {
            SimulationResult = new double[]
            {
                CalculateAvrageCountOfLeftAgents(),
                CalculateAvrageWaitingTime(),
                CalculateFreeTimeOfWaiters(),
                CalculateAvrageCountOfFreeWaiters(),
                CalculateFreeTimeOfCooks(),
                CalculateAvrageCountOfFreeCooks(),
                CalculateAvrageCountOfFreeTables_2(),
                CalculateAvrageCountOfFreeTables_4(),
                CalculateAvrageCountOfFreeTables_6(),
                SimulationTime
            };

            return SimulationResult;
        }
        #endregion


        private double _lastTime;
        public override double[] Simulate(TimeSpan startTime, TimeSpan endTime, int numberOfWaiters, int numberOfCooks, bool cooling, bool run)
        {
            LiveSimulation = !run;

            if (run)
            {
                BeforeSimulation(startTime, endTime, numberOfWaiters, numberOfCooks, cooling);
            }

            int count = 0;
            double lastTime = 0;

            var diff = (Calendar.PeekPriority - SimulationTime);

            for (int i = 0; i < diff; i += 1)
            {
                Calendar.Enqueue(new Fill_Event(null, SimulationTime + i, this), SimulationTime + i);
            }


            while (Calendar.Count > 0)
            {
                SimulationEvent acutalEvent = Calendar.Dequeue();
                SimulationTime = acutalEvent.OccurrenceTime;

                acutalEvent.Execute();

                if (!Cooling)
                {
                    if (SimulationTime > EndTime.TotalSeconds)
                    {
                        if (!run)
                        {
                            SimulationResult = CalculateStatistics();
                            WaitingTimes.Add(SimulationResult[1]);
                        }

                        break;
                    }
                }

                if (!run)
                {

                    if (pause)
                    {
                        waitHandle.WaitOne();
                    }

                    ManageSimulationSpeed();

                    if (Calendar.Count == 0)
                        break;

                    diff = (Calendar.PeekPriority - SimulationTime);

                    if (!(acutalEvent is Fill_Event))
                    {
                        for (int i = 0; i < diff; i += 1)
                        {
                            Calendar.Enqueue(new Fill_Event(null, SimulationTime + i, this), SimulationTime + i);
                        }
                    }

                    SimulationResult = CalculateStatistics();

                    if (Math.Ceiling(lastTime) + 1 <= Math.Ceiling(SimulationTime))
                    {
                        if (count % 15 == 0)
                        {
                            WaitingTimes.Add(SimulationResult[1]);
                        }

                        lastTime = SimulationTime;
                    }

                    count++;
                }
            }

            CalculateStatistics();
            OnSimulationFinished(SimulationResult);
            return SimulationResult;
        }

        public ReplicationData ReplicationData { get; set; }
        public int ActualReplication { get; set; }

        public void CreateConfidenceIntervals()
        {
            AvrageWaiting_ConfindenceInterval =
                new ConfidenceInterval.ConfidenceInterval.SampleStandardDeviationData();

            AvrageCountOfLeftAgents_ConfindenceInterval =
                new ConfidenceInterval.ConfidenceInterval.SampleStandardDeviationData();

            FreeTimeOfWaiters_ConfindenceInterval =
                new ConfidenceInterval.ConfidenceInterval.SampleStandardDeviationData();

            AvrageCountOfFreeWaiters_ConfindenceInterval =
                new ConfidenceInterval.ConfidenceInterval.SampleStandardDeviationData();

            FreeTimeOfCooks_ConfindenceInterval =
                new ConfidenceInterval.ConfidenceInterval.SampleStandardDeviationData();

            AvrageCountOfFreeCooks_ConfindenceInterval =
                new ConfidenceInterval.ConfidenceInterval.SampleStandardDeviationData();

            AvrageCountOfFreeTables_2_ConfindenceInterval =
                new ConfidenceInterval.ConfidenceInterval.SampleStandardDeviationData();

            AvrageCountOfFreeTables_4_ConfindenceInterval =
                new ConfidenceInterval.ConfidenceInterval.SampleStandardDeviationData();

            AvrageCountOfFreeTables_6_ConfindenceInterval =
                new ConfidenceInterval.ConfidenceInterval.SampleStandardDeviationData();
        }
        public override double[] SimulateRuns(TimeSpan startTime, TimeSpan endTime, int numberOfWaiters, int numberOfCooks, bool cooling,
            int numberOfReplications)
        {
            ReplicationData = new ReplicationData();
            CreateConfidenceIntervals();

            double casCakania = 0;
            double pocetOdislo = 0;
            double volnyCasCasnikov = 0;
            double pocetVolnychCasnikov = 0;

            double volnyCasKucharov = 0;
            double pocetVolnychKucharov = 0;

            double pocetVolnychStolov_2 = 0;
            double pocetVolnychStolov_4 = 0;
            double pocetVolnychStolov_6 = 0;

            int pocetCasnikov = numberOfWaiters;
            int pocetKucharov = numberOfCooks;
            bool chladenie = cooling;

            for (int i = 0; i < numberOfReplications; i++)
            {
                var sim = Simulate(new TimeSpan(11, 0, 0), new TimeSpan(20, 0, 0), pocetCasnikov, pocetKucharov, chladenie, true);

                pocetOdislo += sim[0];
                casCakania += sim[1];
                volnyCasCasnikov += sim[2];
                pocetVolnychCasnikov += sim[3];
                volnyCasKucharov += sim[4];
                pocetVolnychKucharov += sim[5];
                pocetVolnychStolov_2 += sim[6];
                pocetVolnychStolov_4 += sim[7];
                pocetVolnychStolov_6 += sim[8];

                if (i % 10 == 0)
                {
                    ReplicationData.AvrageCountOfLeftAgents = pocetOdislo /i;
                    ReplicationData.AvrageWaitingTimeOfAgents = casCakania / i;

                    ReplicationData.AvrageFreeTimeOfWaiters = volnyCasCasnikov / i;
                    ReplicationData.AvrageCountOfFreeWaiters = pocetVolnychCasnikov / i;

                    ReplicationData.AvrageCountOfFreeCooks = volnyCasKucharov / i;
                    ReplicationData.AvrageFreeTimeOfCooks = pocetVolnychKucharov / i;

                    ReplicationData.AvrageFreeTables_2 = pocetVolnychStolov_2 / i;
                    ReplicationData.AvrageFreeTables_4 = pocetVolnychStolov_4 / i;
                    ReplicationData.AvrageFreeTables_6 = pocetVolnychStolov_6 / i;

                    ActualReplication = i;
                }

                GetSamplesForConfidenceInterval(new double[]
                {
                    sim[0] ,
                    sim[1] ,
                    sim[2] ,
                    sim[3],
                    sim[4] ,
                    sim[5] ,
                    sim[6],
                    sim[7],
                    sim[8],
                });
            }

            ReplicationData.AvrageCountOfLeftAgents = pocetOdislo / numberOfReplications;
            ReplicationData.AvrageWaitingTimeOfAgents = casCakania / numberOfReplications;

            ReplicationData.AvrageFreeTimeOfWaiters = volnyCasCasnikov / numberOfReplications;
            ReplicationData.AvrageCountOfFreeWaiters = pocetVolnychCasnikov / numberOfReplications;

            ReplicationData.AvrageCountOfFreeCooks = volnyCasKucharov / numberOfReplications;
            ReplicationData.AvrageFreeTimeOfCooks = pocetVolnychKucharov / numberOfReplications;

            ReplicationData.AvrageFreeTables_2 = pocetVolnychStolov_2 / numberOfReplications;
            ReplicationData.AvrageFreeTables_4 = pocetVolnychStolov_4 / numberOfReplications;
            ReplicationData.AvrageFreeTables_6 = pocetVolnychStolov_6 / numberOfReplications;

            ActualReplication = numberOfReplications;

            var result = new double[]
            {
                casCakania / numberOfReplications,
                pocetOdislo / numberOfReplications,
                volnyCasCasnikov / numberOfReplications,
                pocetVolnychCasnikov / numberOfReplications,
                volnyCasKucharov / numberOfReplications,
                pocetVolnychKucharov / numberOfReplications,
                pocetVolnychStolov_2 / numberOfReplications,
                pocetVolnychStolov_4 / numberOfReplications,
                pocetVolnychStolov_6 / numberOfReplications,
            };


            OnRunFinished(result);
            return result;
        }

        public SimulationCore_S2() : base()
        {
        }

    }

    [AddINotifyPropertyChangedInterface]
    public class ReplicationData
    {
        public double AvrageWaitingTimeOfAgents { get; set; }
        public double AvrageCountOfLeftAgents { get; set; }

        public double AvrageCountOfFreeWaiters { get; set; }
        public double AvrageFreeTimeOfWaiters { get; set; }


        public double AvrageCountOfFreeCooks { get; set; }
        public double AvrageFreeTimeOfCooks { get; set; }

        public double AvrageFreeTables_2 { get; set; }
        public double AvrageFreeTables_4 { get; set; }
        public double AvrageFreeTables_6 { get; set; }

    }
}
