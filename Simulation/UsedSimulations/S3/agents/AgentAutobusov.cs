using System;
using System.Collections.Generic;
using OSPABA;
using simulation;
using managers;
using continualAssistants;
using PropertyChanged;
using Simulations.UsedSimulations.S3;
using Simulations.UsedSimulations.S3.entities;
using System.Linq;

namespace agents
{
    [AddINotifyPropertyChangedInterface]
    //meta! id="14"
    public class AgentAutobusov : Agent
    {
        public List<Autobus> Autobusy { get; set; }
        public AgentAutobusov(int id, OSPABA.Simulation mySim, Agent parent) :
            base(id, mySim, parent)
        {
            Init();
        }

        override public void PrepareReplication()
        {
            base.PrepareReplication();

            VytvorAutobusy();
            PriradAutobusy();
            ZacniJazdy();
            // Setup component for the next replication
        }

        //meta! userInfo="Generated code: do not modify", tag="begin"
        private void Init()
        {
            new ManagerAutobusov(SimId.ManagerAutobusov, MySim, this);
            new InitJazdaProces(SimId.InitJazdaProces, MySim, this);
            new JazdaNaZastavkuProces(SimId.JazdaNaZastavkuProces, MySim, this);
            new VystupovanieProces(SimId.VystupovanieProces, MySim, this);

            AddOwnMessage(Mc.InitJazda);
            AddOwnMessage(Mc.JazdaNaZastavku);
            AddOwnMessage(Mc.KoniecJazdy);
            AddOwnMessage(Mc.CestujuciVystupil);
        }
        //meta! tag="end"

        public void VytvorAutobusy()
        {
            Autobusy = new List<Autobus>();
            var agentOkolia = (AgentOkolia)MySim.FindAgent(SimId.AgentOkolia);


            Autobusy.Add(new Autobus(MySim)
            {
                KapacitaOsob = 186,
                PocetDveri = 4,
                Linka = agentOkolia.Linky[0],
                AktualnaZastavka = agentOkolia.Linky[0].Zastavky[0],
                ZacitokJazdyCas = new TimeSpan(11, 00, 0).TotalMinutes,
                Typ = AutobusyTyp.Autobus
            });

            Autobusy.Add(new Autobus(MySim)
            {
                KapacitaOsob = 8,
                PocetDveri = 1,
                Linka = agentOkolia.Linky[0],
                AktualnaZastavka = agentOkolia.Linky[0].Zastavky[1],
                ZacitokJazdyCas = new TimeSpan(11, 25, 0).TotalMinutes,
                Typ = AutobusyTyp.Microbus
            });

            Autobusy.Add(new Autobus(MySim)
            {
                KapacitaOsob = 186,
                PocetDveri = 4,
                Linka = agentOkolia.Linky[1],
                AktualnaZastavka = agentOkolia.Linky[1].Zastavky.Last(),
                ZacitokJazdyCas = new TimeSpan(11, 50, 0).TotalMinutes,
                Typ = AutobusyTyp.Autobus
            });

            Autobusy.Add(new Autobus(MySim)
            {
                KapacitaOsob = 186,
                PocetDveri = 4,
                Linka = agentOkolia.Linky[2],
                AktualnaZastavka = agentOkolia.Linky[2].Zastavky.Last(),
                ZacitokJazdyCas = new TimeSpan(12, 15, 0).TotalMinutes,
                Typ = AutobusyTyp.Autobus
            });
        }

        public void PriradAutobusy()
        {
            var agentOkolia = (AgentOkolia)MySim.FindAgent(SimId.AgentOkolia);

            agentOkolia.Linky[0].Autobusy = (from x in Autobusy where x.Linka.Meno == "A" select x).ToList();
            agentOkolia.Linky[1].Autobusy = (from x in Autobusy where x.Linka.Meno == "B" select x).ToList();
            agentOkolia.Linky[2].Autobusy = (from x in Autobusy where x.Linka.Meno == "C" select x).ToList();
        }

        public void ZacniJazdy()
        {
            for (int i = 0; i < Autobusy.Count; i++)
            {
                var sprava = new MyMessage(MySim);
                sprava.Addressee = FindAssistant(SimId.InitJazdaProces);
                sprava.Autobus = Autobusy[i];
                MyManager.StartContinualAssistant(sprava);
            }
        }
    }
}
