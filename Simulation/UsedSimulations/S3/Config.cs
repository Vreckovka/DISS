using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Simulations.UsedSimulations.S3
{
    public static class Config
    {
        public const int PocetReplikacii = 1;
        public static double ZaciatokZapasu = new TimeSpan(13,0,0).TotalMinutes;
        public const int PocetAutobusov = 3;
        public static bool Cakanie = true;
    }
}
