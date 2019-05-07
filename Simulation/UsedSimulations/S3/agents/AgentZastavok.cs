using System;
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
    //meta! id="28"
    [AddINotifyPropertyChangedInterface]
    public class AgentZastavok : Agent
    {
    

        public AgentZastavok(int id, OSPABA.Simulation mySim, Agent parent) :
            base(id, mySim, parent)
        {
            Init();
        }

        override public void PrepareReplication()
        {

            base.PrepareReplication();
          

            // Setup component for the next replication
        }

        //meta! userInfo="Generated code: do not modify", tag="begin"
        private void Init()
        {
            new ManagerZastavok(SimId.ManagerZastavok, MySim, this);
            new NastupovanieProces(SimId.NastupovanieProces, MySim, this);

            AddOwnMessage(Mc.PrichodNaZastavku);
            AddOwnMessage(Mc.CestujuciNastupil);
        }
       
        //meta! tag="end"
    }
}
