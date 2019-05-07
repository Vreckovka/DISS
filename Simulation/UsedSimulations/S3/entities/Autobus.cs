using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OSPABA;
using PropertyChanged;

namespace Simulations.UsedSimulations.S3.entities
{
    [TypeConverter(typeof(EnumDescriptionTypeConverter))]
    public enum AutobusyTyp
    {
        Autobus,
        Microbus
    }

    [TypeConverter(typeof(EnumDescriptionTypeConverter))]
    public enum VelkostAutobusu
    {
        [Description("Autobus, 186, 4")]
        K186D4,
        [Description("Autobus, 107, 3")]
        K107D3,
        [Description("Microbus, 8, 1")]
        K8D1,
    }

    [AddINotifyPropertyChangedInterface]
    public class Autobus : Entity 
    {
        public int PocetDveri { get; private set; }
        public int KapacitaOsob { get; private set; }
        public Queue<Cestujuci> Cestujuci { get; set; }
        public Linka Linka { get; set; }
        public int PocetDveriObsadene { get; set; }

        public bool CakalNavyse { get; set; }
        public bool StojiNaZastavke { get; set; }
        public bool KoniecProcesu { get; set; }
        public ZastavkaData AktualnaZastavka { get; set; }
        public bool NovaJazda { get; set; }
        public double CasZaciatkuJazdy { get; set; }
        public AutobusyTyp Typ { get; private set; }


        private VelkostAutobusu _VelkostAutobusu;
        public VelkostAutobusu VelkostAutobusu
        {
            get
            {
               return _VelkostAutobusu;
            }
            set
            {
                _VelkostAutobusu = value;
                SetVelkostAutobusu(value);
            }
        }

        public bool KoniecJazd { get; set; }


        public double StatZaciatokJazdyCas { get; set; } = double.MaxValue;
        public double PrejdenaTrasaPerc { get; set; }

        
        public int CelkovyPocetPrevezenych { get; set; }
        public int AktualnyPocetPrevezenych { get; set; }


        public double JazdaStart { get; set; }
        public double JazdaEnd { get; set; }
        public double DobaJazdy { get; set; }
        public double PocetKoliecok { get; set; }
        public Autobus(OSPABA.Simulation mySim,
            VelkostAutobusu velkostAutobusu,
            Linka linka) : base(mySim)
        {
            Cestujuci = new Queue<Cestujuci>();
            VelkostAutobusu = velkostAutobusu;
            Linka = linka;

            SetVelkostAutobusu(velkostAutobusu);

            AktualnaZastavka = linka.Zastavky.Last();
        }


        private void SetVelkostAutobusu(VelkostAutobusu velkostAutobusu)
        {
            switch (velkostAutobusu)
            {
                case VelkostAutobusu.K186D4:
                    PocetDveri = 4;
                    KapacitaOsob = 186;
                    Typ = AutobusyTyp.Autobus;
                    break;
                case VelkostAutobusu.K107D3:
                    PocetDveri = 3;
                    KapacitaOsob = 107;
                    Typ = AutobusyTyp.Autobus;
                    break;
                case VelkostAutobusu.K8D1:
                    PocetDveri = 1;
                    KapacitaOsob = 8;
                    Typ = AutobusyTyp.Microbus;
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(velkostAutobusu), velkostAutobusu, null);
            }
        }

        public Autobus(int id, OSPABA.Simulation mySim) : base(id, mySim)
        {
            Cestujuci = new Queue<Cestujuci>();
        }

        public void Reset()
        {
            AktualnyPocetPrevezenych = 0;
        }

        public void HardReset()
        {
            Reset();
            KoniecJazd = false;
            KoniecProcesu = false;
            PocetKoliecok = 0;
            StatZaciatokJazdyCas = double.MaxValue;
            PrejdenaTrasaPerc = 0;
            StojiNaZastavke = false;
            CelkovyPocetPrevezenych = 0;
        }
    }
}
