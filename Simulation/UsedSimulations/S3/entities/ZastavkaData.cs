using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Simulations.UsedSimulations.S3.entities
{
    public class ZastavkaData
    {
        public Zastavka DalsiaZastavka { get; set; }
        public double CasKDalsejZastavke { get; set; }
        public double CasKuStadionu { get; set; }

        public KeyValuePair<Zastavka,ZastavkaData> DalsiaZastavkaData { get; set; }
    }
}
