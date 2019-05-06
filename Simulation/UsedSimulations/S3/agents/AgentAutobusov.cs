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
            Autobusy = ((MySimulation) MySim).Configration.Autobusy;
            foreach (var autobus in Autobusy)
            {
                autobus.HardReset();
            }
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
