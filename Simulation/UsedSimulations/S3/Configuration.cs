using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Simulations.UsedSimulations.S3.entities;

namespace Simulations.UsedSimulations.S3
{
    public class Configuration
    {
        public double ZaciatokZapasu { get; set; }
        public bool Cakanie { get; set; }
        public List<Autobus> Autobusy { get; set; } = new List<Autobus>();

        public Configuration(int zaciatokZapasu, bool cakanie)
        {
            ZaciatokZapasu = zaciatokZapasu;
            Cakanie = cakanie;
        }
    }
}

