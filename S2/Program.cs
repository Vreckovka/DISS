﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Simulations.ConfidenceInterval;
using Simulations.Distributions;
using Simulations.UsedSimulations.S2;

namespace S2
{
    class Program
    {
        static void Main(string[] args)
        {
            double casCakania = 0;
            double pocetOdislo = 0;
            double volnyCasCasnikov = 0;
            double pocetVolnychCasnikov = 0;

            double volnyCasKucharov = 0;
            double pocetVolnychKucharov = 0;

            double pocetVolnychStolov_2 = 0;
            double pocetVolnychStolov_4 = 0;
            double pocetVolnychStolov_6 = 0;

            int count = 500;

            int pocetCasnikov = 4;
            int pocetKucharov = 16;
            bool chladenie = false;

            List<double> casCakaniaList = new List<double>();
            List<double> pocetOdisloList = new List<double>();

            List<double> pocetVolnychCasnikovList = new List<double>();
            List<double> volnyCasCasnikovList = new List<double>();

            List<double> pocetVolnychKucharovList = new List<double>();
            List<double> volnyCasKucharovList = new List<double>();
        
            List<double> pocetVolnychStolov_2List = new List<double>();
            List<double> pocetVolnychStolov_4List = new List<double>();
            List<double> pocetVolnychStolov_6List = new List<double>();

            ConfidenceInterval.SampleStandardDeviationData sampleStandardDeviation = new ConfidenceInterval.SampleStandardDeviationData();
            Stopwatch timer = new Stopwatch();         
            timer.Start();

            for (int i = 0; i < count; i++)
            {
                SimulationCore_S2 s2_SimulationCore =
                    new SimulationCore_S2();

                var sim = s2_SimulationCore.Simulate(new TimeSpan(11, 0, 0), new TimeSpan(20, 0, 0), pocetCasnikov, pocetKucharov, chladenie, true);

                pocetOdisloList.Add(sim[0]);
                pocetOdislo += sim[0];

                casCakaniaList.Add(sim[1]);
                casCakania += sim[1];

                volnyCasCasnikov += sim[2];
                volnyCasCasnikovList.Add(sim[2]);

                pocetVolnychCasnikov += sim[3];
                pocetVolnychCasnikovList.Add(sim[3]);


                volnyCasKucharov += sim[4];
                volnyCasKucharovList.Add(sim[4]);

                pocetVolnychKucharov += sim[5];
                pocetVolnychKucharovList.Add(sim[5]);


                pocetVolnychStolov_2 += sim[6];
                pocetVolnychStolov_2List.Add(sim[6]);

                pocetVolnychStolov_4 += sim[7];
                pocetVolnychStolov_4List.Add(sim[7]);

                pocetVolnychStolov_6 += sim[8];
                pocetVolnychStolov_6List.Add(sim[8]);

                sampleStandardDeviation.AddValue(sim[1]);

                if (i % 500 == 0)
                {
                    Console.Clear();
                    Console.WriteLine(i.ToString("N0"));
                    Console.WriteLine($"Pocet casnikov: {pocetCasnikov}\t Pocet kucharov: {pocetKucharov}\t Chladenie: {chladenie}\n");
                    Console.WriteLine($"Priemerny cas cakania:\t{casCakania / i:N5}\n" +
                                      $"Pocet odidenych:\t{pocetOdislo / i:N5}\n\n" +
                                      $"Volny cas casnikov:\t{volnyCasCasnikov / i:N5}\n" +
                                      $"Pocet volnych casnikov:\t{pocetVolnychCasnikov / i:N5}\n\n" +
                                      $"Volny cas kucharov:\t{volnyCasKucharov / i:N5}\n" +
                                      $"Pocet volnych kucharov:\t{pocetVolnychKucharov / i:N5}\n\n" +
                                      $"Pocet volnych stolov 2:\t{pocetVolnychStolov_2 / i:N5}\n" +
                                      $"Pocet volnych stolov 4:\t{pocetVolnychStolov_4 / i:N5}\n" +
                                      $"Pocet volnych stolov 6:\t{pocetVolnychStolov_6 / i:N5}"
                                      );

                   
                }
            }

            Console.Clear();
            Console.WriteLine(count.ToString("N0"));
            Console.WriteLine($"Pocet casnikov: {pocetCasnikov}\t Pocet kucharov: {pocetKucharov}\t Chladenie: {chladenie}\n");
            Console.WriteLine($"Priemerny cas cakania:\t{casCakania / count:N5}\t{ConfidenceInterval.ToStringInterval(0.95, casCakaniaList)}\n" +
                              $"Pocet odidenych:\t{pocetOdislo / count:N5}\t{ConfidenceInterval.ToStringInterval(0.95, pocetOdisloList)}\n\n" +
                              $"Volny cas casnikov:\t{volnyCasCasnikov / count:N5}\t{ConfidenceInterval.ToStringInterval(0.95, volnyCasCasnikovList)}\n" +
                              $"Pocet volnych casnikov:\t{pocetVolnychCasnikov / count:N5}\t\t{ConfidenceInterval.ToStringInterval(0.95, pocetVolnychCasnikovList)}\n\n" +
                              $"Volny cas kucharov:\t{volnyCasKucharov / count:N5}\t{ConfidenceInterval.ToStringInterval(0.95, volnyCasKucharovList)}\n" +
                              $"Pocet volnych kucharov:\t{pocetVolnychKucharov / count:N5}\t\t{ConfidenceInterval.ToStringInterval(0.95, pocetVolnychKucharovList)}\n\n" +
                              $"Pocet volnych stolov 2:\t{pocetVolnychStolov_2 / count:N5}\t\t{ConfidenceInterval.ToStringInterval(0.95, pocetVolnychStolov_2List)}\n" +
                              $"Pocet volnych stolov 4:\t{pocetVolnychStolov_4 / count:N5}\t\t{ConfidenceInterval.ToStringInterval(0.95, pocetVolnychStolov_4List)}\n" +
                              $"Pocet volnych stolov 6:\t{pocetVolnychStolov_6 / count:N5}\t\t{ConfidenceInterval.ToStringInterval(0.95, pocetVolnychStolov_6List)}"
            );

            Console.WriteLine(ConfidenceInterval.ToStringInterval(0.95, sampleStandardDeviation));
            timer.Stop();
            Console.WriteLine("Time elapsed: {0:hh\\:mm\\:ss}", timer.Elapsed);

            Console.ReadLine();
        }
    }
}
