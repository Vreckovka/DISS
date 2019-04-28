using System.Collections.Generic;
using OSPABA;
using simulation;
using managers;
using continualAssistants;
using PropertyChanged;
using Simulations.UsedSimulations.S3;
using Simulations.UsedSimulations.S3.entities;

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
            ZacniJazdy();
            // Setup component for the next replication
        }

        //meta! userInfo="Generated code: do not modify", tag="begin"
        private void Init()
        {
            new ManagerAutobusov(SimId.ManagerAutobusov, MySim, this);
            new InitJazdaProces(SimId.InitJazdaProces, MySim, this);
            new JazdaNaZastavkuProces(SimId.JazdaNaZastavkuProces, MySim, this);

            AddOwnMessage(Mc.InitJazda);
            AddOwnMessage(Mc.JazdaNaZastavku);
            AddOwnMessage(Mc.KoniecJazdy);
        }
        //meta! tag="end"

        public void VytvorAutobusy()
        {
            Autobusy = new List<Autobus>();
            var agentOkolia = (AgentOkolia)MySim.FindAgent(SimId.AgentOkolia);

            Autobusy.Add(new Autobus(1,MySim)
            {
                KapacitaOsob = 186,
                PocetDveri = 4,
                Linka = agentOkolia.Linky[0],
                IndexStartZastavka = 0
            });

            Autobusy.Add(new Autobus(2, MySim)
            {
                KapacitaOsob = 186,
                PocetDveri = 4,
                Linka = agentOkolia.Linky[0],
                IndexStartZastavka = 0
            });

            Autobusy.Add(new Autobus(3, MySim)
            {
                KapacitaOsob = 186,
                PocetDveri = 4,
                Linka = agentOkolia.Linky[1],
                IndexStartZastavka = 0
            });

            Autobusy.Add(new Autobus(4, MySim)
            {
                KapacitaOsob = 186,
                PocetDveri = 4,
                Linka = agentOkolia.Linky[1],
                IndexStartZastavka = 0
            });

            Autobusy.Add(new Autobus(5, MySim)
            {
                KapacitaOsob = 186,
                PocetDveri = 4,
                Linka = agentOkolia.Linky[2],
                IndexStartZastavka = 0
            });

            Autobusy.Add(new Autobus(6, MySim)
            {
                KapacitaOsob = 186,
                PocetDveri = 4,
                Linka = agentOkolia.Linky[2],
                IndexStartZastavka = 0
            });
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
