using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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

            int count = 10000;

            int pocetCasnikov = 4;
            int pocetKucharov = 16;
            bool chladenie = false;

            Stopwatch timer = new Stopwatch();         
            timer.Start();

            for (int i = 0; i < count; i++)
            {
                SimulationCore_S2 s2_SimulationCore =
                    new SimulationCore_S2(new TimeSpan(11, 0, 0), new TimeSpan(20, 0, 0), pocetCasnikov, pocetKucharov, chladenie);

                var sim = s2_SimulationCore.Simulate();
                pocetOdislo += sim[0];
                casCakania += sim[1];

                volnyCasCasnikov += sim[2];
                pocetVolnychCasnikov += sim[3];

                volnyCasKucharov += sim[4];
                pocetVolnychKucharov += sim[5];

                pocetVolnychStolov_2 += sim[6];
                pocetVolnychStolov_4 += sim[7];
                pocetVolnychStolov_6 += sim[8];

                if (i % 500 == 0)
                {
                    Console.Clear();
                    Console.WriteLine(i);
                    Console.WriteLine($"Pocet casnikov: {pocetCasnikov}\t Pocet kucharov: {pocetKucharov}\t Chladenie: {chladenie}\n");
                    Console.WriteLine($"Priemerny cas cakania:\t{casCakania / i}\n" +
                                      $"Pocet odidenych:\t{pocetOdislo / i}\n\n" +
                                      $"Volny cas casnikov:\t{volnyCasCasnikov / i}\n" +
                                      $"Pocet volnych casnikov:\t{pocetVolnychCasnikov / i}\n\n" +
                                      $"Volny cas kucharov:\t{volnyCasKucharov / i}\n" +
                                      $"Pocet volnych kucharov:\t{pocetVolnychKucharov / i}\n\n" +
                                      $"Pocet volnych stolov 2:\t{pocetVolnychStolov_2 / i}\n" +
                                      $"Pocet volnych stolov 4:\t{pocetVolnychStolov_4 / i}\n" +
                                      $"Pocet volnych stolov 6:\t{pocetVolnychStolov_6 / i}"
                                      );
                }
            }


            Console.Clear();
            Console.WriteLine(count);
            Console.WriteLine($"Pocet casnikov: {pocetCasnikov}\t Pocet kucharov: {pocetKucharov}\t Chladenie: {chladenie}\n");
            Console.WriteLine($"Priemerny cas cakania:\t{casCakania / count}\n" +
                              $"Pocet odidenych:\t{pocetOdislo / count}\n\n" +
                              $"Volny cas casnikov:\t{volnyCasCasnikov / count}\n" +
                              $"Pocet volnych casnikov:\t{pocetVolnychCasnikov / count}\n\n" +
                              $"Volny cas kucharov:\t{volnyCasKucharov / count}\n" +
                              $"Pocet volnych kucharov:\t{pocetVolnychKucharov / count}\n\n" +
                              $"Pocet volnych stolov 2:\t{pocetVolnychStolov_2 / count}\n" +
                              $"Pocet volnych stolov 4:\t{pocetVolnychStolov_4 / count}\n" +
                              $"Pocet volnych stolov 6:\t{pocetVolnychStolov_6 / count}"
            );
            timer.Stop();
            Console.WriteLine("Time elapsed: {0:hh\\:mm\\:ss}", timer.Elapsed);

            Console.ReadLine();
        }
    }
}
