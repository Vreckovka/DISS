﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OSPRNG;
using Simulations.Distributions;

namespace Simulations.UsedSimulations.S3.entities
{
    public class ZastavkaData
    {
        public Zastavka Zastavka { get; set; }
        public ZastavkaData DalsiaZastavka { get; set; }
        public double CasKDalsejZastavke { get; set; }
        public double CasKuStadionu { get; set; }
        public double CasKoncaGenerovania { get; set; }
        public double CasZaciatkuGenerovania { get; set; }


        //public int Cislo { get; set; } = 5;
        public ExponentialDistribution Generator { get; set; }
        public bool Konecna { get; set; }

        public override string ToString()
        {
            return Zastavka.Meno;
        }
    }
}
