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
using System.Collections.ObjectModel;

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
            new VystupovanieProces(SimId.VystupovanieProces, MySim, this);

            AddOwnMessage(Mc.InitJazda);
            AddOwnMessage(Mc.JazdaNaZastavku);
            AddOwnMessage(Mc.KoniecJazdy);
            AddOwnMessage(Mc.CestujuciVystupil);
        }
        //meta! tag="end"

        public void VytvorAutobusy()
        {
            var agent = (AgentOkolia)MySim.FindAgent(SimId.AgentOkolia);
            var linky = (from x in agent.Linky select x.Autobusy).ToList();

            Autobusy = new List<Autobus>();

            foreach (ObservableCollection<Autobus> linka in linky)
            {
                foreach (var autobus in linka)
                {
                    autobus.HardReset();
                    Autobusy.Add(autobus);
                }
            }
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
