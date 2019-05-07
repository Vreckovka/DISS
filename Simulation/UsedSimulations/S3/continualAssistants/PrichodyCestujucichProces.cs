using System;
using System.Threading;
using OSPABA;
using simulation;
using agents;
using Simulations.Distributions;
using Simulations.UsedSimulations.S3.entities;

namespace continualAssistants
{
    //meta! id="66"
    public class PrichodyCestujucichProces : Process
    {

        private int cestujuciIndex;
        public PrichodyCestujucichProces(int id, OSPABA.Simulation mySim, CommonAgent myAgent) :
            base(id, mySim, myAgent)
        {

        }

        override public void PrepareReplication()
        {
            base.PrepareReplication();
            // VytvorGeneratori();
            // Setup component for the next replication
        }

        //meta! sender="AgentOkolia", id="67", type="Start"
        public void ProcessStart(MessageForm message)
        {
            var sprava = (MyMessage)message.CreateCopy();
            sprava.Code = Mc.PrichodCestujuceho;
            sprava.ZastavkaData = ((MyMessage)message).ZastavkaData;

            Hold(sprava.ZastavkaData.Generator.GetNext(), sprava);
        }

        //meta! userInfo="Process messages defined in code", id="0"
        public void ProcessDefault(MessageForm message)
        {
            switch (message.Code)
            {
                case Mc.PrichodCestujuceho:
                    var sprava = (MyMessage)message;

                    if (MySim.CurrentTime <= sprava.ZastavkaData.CasKoncaGenerovania
                        && sprava.ZastavkaData.Zastavka.PocetVygenerovanych <
                        sprava.ZastavkaData.Zastavka.MaxPocetVygenerovanych)
                    {
                        MyAgent.CelkovyPocetCestujucich++;


                        var novaSprava = (MyMessage)message.CreateCopy();

                        var cestujuci = new Cestujuci(cestujuciIndex++, MySim);

                        cestujuci.CasZacatiaCakania = MySim.CurrentTime;
                        


                        novaSprava.ZastavkaData = sprava.ZastavkaData;
                        novaSprava.ZastavkaData.Zastavka.Cestujuci.Enqueue(cestujuci);
                        novaSprava.ZastavkaData.Zastavka.PocetCestujucich++;
                        novaSprava.ZastavkaData.Zastavka.PocetVygenerovanych++;

                        novaSprava.ZastavkaData.Zastavka.PocetCestujucich = novaSprava.ZastavkaData.Zastavka.Cestujuci.Count;

                        //  if (novaSprava.ZastavkaData.Zastavka.Meno == "AB")
                        //      Console.WriteLine((MySim.CurrentTime * 60) + " " + novaSprava.ZastavkaData.Zastavka);

                        if (novaSprava.ZastavkaData.Zastavka.CakajuciAutobus != null &&
                            novaSprava.ZastavkaData.Zastavka.CakajuciAutobus.CakalNavyse)
                        {

                            var nastupovanieSprava = new MyMessage(MySim);
                            nastupovanieSprava.Autobus = novaSprava.ZastavkaData.Zastavka.CakajuciAutobus;
                            nastupovanieSprava.Addressee = this;
                            nastupovanieSprava.Code = Mc.CestujuciNastupil;

                            var agentAuto = MySim.FindAgent(SimId.AgentZastavok);
                            var asd = (NastupovanieProces)agentAuto.FindAssistant(SimId.NastupovanieProces);


                            var cestujuciNastupuje = nastupovanieSprava.Autobus.AktualnaZastavka.Zastavka.Cestujuci.Dequeue();
                            cestujuciNastupuje.CasCakania = (MySim.CurrentTime * 60) - (cestujuciNastupuje.CasZacatiaCakania * 60);
                            cestujuciNastupuje.Linka = nastupovanieSprava.Autobus.Linka;

                            nastupovanieSprava.Autobus.Cestujuci.Enqueue(cestujuciNastupuje);
                            nastupovanieSprava.Autobus.CelkovyPocetPrevezenych++;
                            nastupovanieSprava.Autobus.AktualnyPocetPrevezenych++;
                            nastupovanieSprava.Autobus.AktualnaZastavka.Zastavka.PocetCestujucich =
                                nastupovanieSprava.Autobus.AktualnaZastavka.Zastavka.Cestujuci.Count;

                            nastupovanieSprava.Autobus.PocetDveriObsadene++;

                            //TODO: Generator
                            Hold(asd.triangularDistribution.GetNext(), nastupovanieSprava);
                        }


                        Hold(novaSprava.ZastavkaData.Generator.GetNext(), novaSprava);
                    }
                    else
                    {
                        AssistantFinished(message);
                    }

                    break;

                case Mc.CestujuciNastupil:
                    {
                        var agentAuto = MySim.FindAgent(SimId.AgentZastavok);
                        var asd = agentAuto.FindAssistant(SimId.NastupovanieProces);

                        ((MyMessage)message).Addressee = asd;
                        ((MyMessage)message).Code = Mc.CestujuciNastupil;

                        Notice(((MyMessage)message));
                    }

                    break;
            }
        }


        //meta! userInfo="Generated code: do not modify", tag="begin"
        override public void ProcessMessage(MessageForm message)
        {
            switch (message.Code)
            {
                case Mc.Start:
                    ProcessStart(message);
                    break;

                default:
                    ProcessDefault(message);
                    break;
            }
        }
        //meta! tag="end"
        public new AgentOkolia MyAgent
        {
            get
            {
                return (AgentOkolia)base.MyAgent;
            }
        }
    }
}
