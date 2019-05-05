using System;
using OSPABA;
using simulation;
using agents;
using Simulations.UsedSimulations.S3;

namespace continualAssistants
{
    //meta! id="49"
    public class JazdaNaZastavkuProces : Process
    {
        public JazdaNaZastavkuProces(int id, OSPABA.Simulation mySim, CommonAgent myAgent) :
            base(id, mySim, myAgent)
        {
        }

        override public void PrepareReplication()
        {
            base.PrepareReplication();
            // Setup component for the next replication
        }

        //meta! sender="AgentAutobusov", id="50", type="Start"
        public void ProcessStart(MessageForm message)
        {
            MyMessage sprava = (MyMessage)message.CreateCopy();

            sprava.Autobus = ((MyMessage)message).Autobus;
            sprava.Code = Mc.JazdaNaZastavku;

            if (sprava.Autobus.AktualnaZastavka == sprava.Autobus.Linka.Zastavky[0])
            {
                sprava.Autobus.JazdaStart = MySim.CurrentTime;
            }

            sprava.Autobus.ZaciatokJazdyCas = MySim.CurrentTime;
         
            Hold(sprava.Autobus.AktualnaZastavka.CasKDalsejZastavke, sprava);
        }

        //meta! userInfo="Process messages defined in code", id="0"
        public void ProcessDefault(MessageForm message)
        {
            switch (message.Code)
            {
                case Mc.JazdaNaZastavku:
                    AssistantFinished(message);

                    var sprava = new MyMessage((MyMessage)message);

                    sprava.Autobus = ((MyMessage)message).Autobus;
                    sprava.Code = Mc.PrichodNaZastavku;
                    sprava.Addressee = MySim.FindAgent(SimId.AgentZastavok);


                    sprava.Autobus.AktualnaZastavka = sprava.Autobus.AktualnaZastavka.DalsiaZastavka;


                    //Console.WriteLine(MySim.CurrentTime + " " + sprava.Autobus.AktualnaZastavka.Zastavka + " " + sprava.Autobus.AktualnaZastavka.Zastavka.Cestujuci.Count);
                    if (sprava.Autobus.NovaJazda)
                    {
                        sprava.Autobus.KoniecProcesu = false;
                        sprava.Autobus.NovaJazda = false;
                    }

                    if (sprava.Autobus.AktualnaZastavka.Konecna)
                    {
                        sprava.Autobus.JazdaEnd = MySim.CurrentTime;
                        sprava.Autobus.PocetKoliecok++;
                        sprava.Autobus.DobaJazdy += sprava.Autobus.JazdaEnd - sprava.Autobus.JazdaStart;
                    }

                    Notice(sprava);
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
        public new AgentAutobusov MyAgent
        {
            get
            {
                return (AgentAutobusov)base.MyAgent;
            }
        }
    }
}
