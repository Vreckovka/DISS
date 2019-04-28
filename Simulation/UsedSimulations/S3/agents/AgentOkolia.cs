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
    [AddINotifyPropertyChangedInterface]
    //meta! id="8"
    public class AgentOkolia : Agent
    {
        public List<Linka> Linky { get; set; }
        public List<Zastavka> Zastavky { get; set; } 
        public AgentOkolia(int id, OSPABA.Simulation mySim, Agent parent) :
			base(id, mySim, parent)
		{
			Init();
		}

		override public void PrepareReplication()
		{
			base.PrepareReplication();

            StartGenerovanieCestujucich();
       
            // Setup component for the next replication
        }

      
        //meta! userInfo="Generated code: do not modify", tag="begin"
        private void Init()
		{
            //VytvorZastavky();
            //VypisZastavky();
            
            new ManagerOkolia(SimId.ManagerOkolia, MySim, this);
			new PrichodyCestujucichProces(SimId.PrichodyCestujucichProces, MySim, this);
            new PrichodyCestujucichNaZastavkuProces(SimId.PrichodyCestujucichNaZastavkuProces, MySim, this);
           
            AddOwnMessage(Mc.CestujuciDovezeny);
            AddOwnMessage(Mc.PrichodCestujuceho);
            AddOwnMessage(Mc.ZacniGenerovatCestujucich);
        }

        public void StartGenerovanieCestujucich()
        {
            foreach (var linka in Linky)
            {
                foreach (var zastavka in linka.Zastavky)
                {
                    var sprava = new MyMessage(MySim);
                    zastavka.Key.CasKoncaGenerovania =
                        (long) (Config.ZaciatokZapasu - ((10 + zastavka.Value.CasKuStadionu)));

                    sprava.Param = (long)(Config.ZaciatokZapasu - ((75 + zastavka.Value.CasKuStadionu)));
                    sprava.IndexZastavky = zastavka.Key.GlobalIndex;
                    sprava.Addressee = FindAssistant(SimId.PrichodyCestujucichNaZastavkuProces);
                    MyManager.StartContinualAssistant(sprava);
                }
            }

        }

        //meta! tag="end"

        public void VytvorZastavky()
        {

            Zastavky = new List<Zastavka>()
            {
                new Zastavka(MySim)
                {
                    Meno = "K1",
                    CelkovyPocetCestujucich = 260,
                    GlobalIndex = 0

                },
                new Zastavka(MySim)
                {
                    Meno = "K2",
                    CelkovyPocetCestujucich = 210,
                    GlobalIndex = 1
                },
                new Zastavka(MySim)
                {
                    Meno = "K3",
                    CelkovyPocetCestujucich = 220,
                    GlobalIndex = 2
                },
                new Zastavka(MySim)
                {
                    Meno = "Stadion",
                    GlobalIndex = 3
                },
            new Zastavka(MySim)
              {
                  Meno = "AA",
                  CelkovyPocetCestujucich = 123,
                 GlobalIndex = 4
              },
              new Zastavka(MySim)
              {
                  Meno = "AB",
                  CelkovyPocetCestujucich = 92,
                  GlobalIndex = 5
              },new Zastavka(MySim)
              {
                  Meno = "AC",
                  CelkovyPocetCestujucich = 241,
                  GlobalIndex = 6
              }
              ,new Zastavka(MySim)
              {
                  Meno = "AD",
                  CelkovyPocetCestujucich = 123,
                  GlobalIndex = 7
              },
              new Zastavka(MySim)
              {
                  Meno = "AE",
                  CelkovyPocetCestujucich = 215,
                  GlobalIndex = 8
              },
              new Zastavka(MySim)
              {
                  Meno = "AF",
                  CelkovyPocetCestujucich = 245,
                  GlobalIndex = 9
              },
              new Zastavka(MySim)
              {
                  Meno = "AG",
                  CelkovyPocetCestujucich = 137,
                  GlobalIndex = 10
              },
              new Zastavka(MySim)
              {
                  Meno = "AH",
                  CelkovyPocetCestujucich = 132,
                  GlobalIndex = 11
              },
              new Zastavka(MySim)
              {
                  Meno = "AI",
                  CelkovyPocetCestujucich = 164,
                  GlobalIndex = 12
              },
              new Zastavka(MySim)
              {
                  Meno = "AJ",
                  CelkovyPocetCestujucich = 124,
                  GlobalIndex = 13
              },
              new Zastavka(MySim)
              {
                  Meno = "AK",
                  CelkovyPocetCestujucich = 213,
                  GlobalIndex = 14
              },
              new Zastavka(MySim)
              {
                  Meno = "AL",
                  CelkovyPocetCestujucich = 185,
                  GlobalIndex = 15
              },


              new Zastavka(MySim)
              {
                  Meno = "BA",
                  CelkovyPocetCestujucich = 79,
                  GlobalIndex = 16
              },
              new Zastavka(MySim)
              {
                  Meno = "BB",
                  CelkovyPocetCestujucich = 69,
                  GlobalIndex = 17
              },
              new Zastavka(MySim)
              {
                  Meno = "BC",
                  CelkovyPocetCestujucich = 43,
                  GlobalIndex = 18
              },
              new Zastavka(MySim)
              {
                  Meno = "BD",
                  CelkovyPocetCestujucich = 127,
                  GlobalIndex = 19
              },
              new Zastavka(MySim)
              {
                  Meno = "BE",
                  CelkovyPocetCestujucich = 30,
                  GlobalIndex = 20
              },
              new Zastavka(MySim)
              {
                  Meno = "BF",
                  CelkovyPocetCestujucich = 69,
                  GlobalIndex = 21
              },
              new Zastavka(MySim)
              {
                  Meno = "BG",
                  CelkovyPocetCestujucich = 162,
                  GlobalIndex = 22
              },
              new Zastavka(MySim)
              {
                  Meno = "BH",
                  CelkovyPocetCestujucich = 90,
                  GlobalIndex = 23
              },
              new Zastavka(MySim)
              {
                  Meno = "BI",
                  CelkovyPocetCestujucich = 148,
                  GlobalIndex = 24
              },
              new Zastavka(MySim)
              {
                  Meno = "BJ",
                  CelkovyPocetCestujucich = 171,
                  GlobalIndex = 25
              },





              new Zastavka(MySim)
              {
                  Meno = "CA",
                  CelkovyPocetCestujucich = 240,
                  GlobalIndex = 26
              },
              new Zastavka(MySim)
              {
                  Meno = "CB",
                  CelkovyPocetCestujucich = 310,
                  GlobalIndex = 27
              },
              new Zastavka(MySim)
              {
                  Meno = "CC",
                  CelkovyPocetCestujucich = 131,
                  GlobalIndex = 28
              },
              new Zastavka(MySim)
              {
                  Meno = "CD",
                  CelkovyPocetCestujucich = 190,
                  GlobalIndex = 29
              },
              new Zastavka(MySim)
              {
                  Meno = "CE",
                  CelkovyPocetCestujucich = 132,
                  GlobalIndex = 30
              },
              new Zastavka(MySim)
              {
                  Meno = "CF",
                  CelkovyPocetCestujucich = 128,
                  GlobalIndex = 31
              },
              new Zastavka(MySim)
              {
                  Meno = "CG",
                  CelkovyPocetCestujucich = 70,
                  GlobalIndex = 32
              },
            };

            //Linka A
            Linky = new List<Linka>()
            {
                new Linka(MySim)
                {

                Meno = "A",
                Zastavky = new List<KeyValuePair<Zastavka, ZastavkaData>>()
                {
                    new KeyValuePair<Zastavka, ZastavkaData>(
                        Zastavky[4],
                    new ZastavkaData()
                    {
                        DalsiaZastavka = Zastavky[5],
                        CasKDalsejZastavke = 3.2,
                    }
                    ),
                    new KeyValuePair<Zastavka, ZastavkaData>(
                        Zastavky[5],
                        new ZastavkaData()
                        {
                            DalsiaZastavka = Zastavky[6],
                            CasKDalsejZastavke = 2.3
                        }
                    ),
                    new KeyValuePair<Zastavka, ZastavkaData>(
                        Zastavky[6],
                        new ZastavkaData()
                        {
                            DalsiaZastavka = Zastavky[7],
                            CasKDalsejZastavke = 2.1
                        }
                    ),
                    new KeyValuePair<Zastavka, ZastavkaData>(
                        Zastavky[7],
                        new ZastavkaData()
                        {
                            DalsiaZastavka = Zastavky[0],
                            CasKDalsejZastavke = 1.2
                        }
                    ),
                    new KeyValuePair<Zastavka, ZastavkaData>(
                        Zastavky[0],
                        new ZastavkaData()
                        {
                            DalsiaZastavka = Zastavky[8],
                            CasKDalsejZastavke = 5.4
                        }
                    ),
                    new KeyValuePair<Zastavka, ZastavkaData>(
                        Zastavky[8],
                        new ZastavkaData()
                        {
                            DalsiaZastavka = Zastavky[9],
                            CasKDalsejZastavke = 2.9
                        }
                    ),
                    new KeyValuePair<Zastavka, ZastavkaData>(
                        Zastavky[9],
                        new ZastavkaData()
                        {
                            DalsiaZastavka = Zastavky[10],
                            CasKDalsejZastavke = 3.4
                        }
                    ),
                    new KeyValuePair<Zastavka, ZastavkaData>(
                        Zastavky[10],
                        new ZastavkaData()
                        {
                            DalsiaZastavka = Zastavky[2],
                            CasKDalsejZastavke = 1.8
                        }
                    ),
                    new KeyValuePair<Zastavka, ZastavkaData>(
                        Zastavky[2],
                        new ZastavkaData()
                        {
                            DalsiaZastavka = Zastavky[11],
                            CasKDalsejZastavke = 4.0
                        }
                    ),
                    new KeyValuePair<Zastavka, ZastavkaData>(
                        Zastavky[11],
                        new ZastavkaData()
                        {
                            DalsiaZastavka = Zastavky[12],
                            CasKDalsejZastavke = 1.6
                        }
                    ),
                    new KeyValuePair<Zastavka, ZastavkaData>(
                        Zastavky[12],
                        new ZastavkaData()
                        {
                            DalsiaZastavka = Zastavky[13],
                            CasKDalsejZastavke = 4.6
                        }
                    ),
                    new KeyValuePair<Zastavka, ZastavkaData>(
                        Zastavky[13],
                        new ZastavkaData()
                        {
                            DalsiaZastavka = Zastavky[14],
                            CasKDalsejZastavke = 3.4
                        }
                    ),
                    new KeyValuePair<Zastavka, ZastavkaData>(
                        Zastavky[14],
                        new ZastavkaData()
                        {
                            DalsiaZastavka = Zastavky[15],
                            CasKDalsejZastavke = 1.2
                        }
                    ),
                    new KeyValuePair<Zastavka, ZastavkaData>(
                        Zastavky[15],
                        new ZastavkaData()
                        {
                            DalsiaZastavka = Zastavky[3],
                            CasKDalsejZastavke = 0.9
                        }
                    ),
                }
                },
                 new Linka(MySim)
                {

                Meno = "B",
                Zastavky = new List<KeyValuePair<Zastavka, ZastavkaData>>()
                {
                    new KeyValuePair<Zastavka, ZastavkaData>(
                        Zastavky[16],
                    new ZastavkaData()
                    {
                        DalsiaZastavka = Zastavky[17],
                        CasKDalsejZastavke = 1.2,
                    }
                    ),
                    new KeyValuePair<Zastavka, ZastavkaData>(
                        Zastavky[17],
                        new ZastavkaData()
                        {
                            DalsiaZastavka = Zastavky[18],
                            CasKDalsejZastavke = 2.3
                        }
                    ),
                    new KeyValuePair<Zastavka, ZastavkaData>(
                        Zastavky[18],
                        new ZastavkaData()
                        {
                            DalsiaZastavka = Zastavky[19],
                            CasKDalsejZastavke = 3.2
                        }
                    ),
                    new KeyValuePair<Zastavka, ZastavkaData>(
                        Zastavky[19],
                        new ZastavkaData()
                        {
                            DalsiaZastavka = Zastavky[1],
                            CasKDalsejZastavke = 4.3
                        }
                    ),
                    new KeyValuePair<Zastavka, ZastavkaData>(
                        Zastavky[1],
                        new ZastavkaData()
                        {
                            DalsiaZastavka = Zastavky[20],
                            CasKDalsejZastavke = 1.2
                        }
                    ),
                    new KeyValuePair<Zastavka, ZastavkaData>(
                        Zastavky[20],
                        new ZastavkaData()
                        {
                            DalsiaZastavka = Zastavky[21],
                            CasKDalsejZastavke = 2.7
                        }
                    ),
                    new KeyValuePair<Zastavka, ZastavkaData>(
                        Zastavky[21],
                        new ZastavkaData()
                        {
                            DalsiaZastavka = Zastavky[2],
                            CasKDalsejZastavke = 3
                        }
                    ),
                    new KeyValuePair<Zastavka, ZastavkaData>(
                        Zastavky[2],
                        new ZastavkaData()
                        {
                            DalsiaZastavka = Zastavky[22],
                            CasKDalsejZastavke = 6
                        }
                    ),
                    new KeyValuePair<Zastavka, ZastavkaData>(
                        Zastavky[22],
                        new ZastavkaData()
                        {
                            DalsiaZastavka = Zastavky[23],
                            CasKDalsejZastavke = 4.3
                        }
                    ),
                    new KeyValuePair<Zastavka, ZastavkaData>(
                        Zastavky[23],
                        new ZastavkaData()
                        {
                            DalsiaZastavka = Zastavky[24],
                            CasKDalsejZastavke = 0.5
                        }
                    ),
                    new KeyValuePair<Zastavka, ZastavkaData>(
                        Zastavky[24],
                        new ZastavkaData()
                        {
                            DalsiaZastavka = Zastavky[25],
                            CasKDalsejZastavke = 2.7
                        }
                    ),
                    new KeyValuePair<Zastavka, ZastavkaData>(
                        Zastavky[25],
                        new ZastavkaData()
                        {
                            DalsiaZastavka = Zastavky[3],
                            CasKDalsejZastavke = 1.3
                        }
                    )
                }
                },

                  new Linka(MySim)
                {

                Meno = "C",
                Zastavky = new List<KeyValuePair<Zastavka, ZastavkaData>>()
                {
                    new KeyValuePair<Zastavka, ZastavkaData>(
                        Zastavky[26],
                    new ZastavkaData()
                    {
                        DalsiaZastavka = Zastavky[27],
                        CasKDalsejZastavke = 0.6,
                    }
                    ),
                    new KeyValuePair<Zastavka, ZastavkaData>(
                        Zastavky[27],
                        new ZastavkaData()
                        {
                            DalsiaZastavka = Zastavky[0],
                            CasKDalsejZastavke = 2.3
                        }
                    ),
                    new KeyValuePair<Zastavka, ZastavkaData>(
                        Zastavky[0],
                        new ZastavkaData()
                        {
                            DalsiaZastavka = Zastavky[1],
                            CasKDalsejZastavke = 4.1
                        }
                    ),
                    new KeyValuePair<Zastavka, ZastavkaData>(
                        Zastavky[1],
                        new ZastavkaData()
                        {
                            DalsiaZastavka = Zastavky[28],
                            CasKDalsejZastavke = 6
                        }
                    ),
                    new KeyValuePair<Zastavka, ZastavkaData>(
                        Zastavky[28],
                        new ZastavkaData()
                        {
                            DalsiaZastavka = Zastavky[29],
                            CasKDalsejZastavke = 2.3
                        }
                    ),
                    new KeyValuePair<Zastavka, ZastavkaData>(
                        Zastavky[29],
                        new ZastavkaData()
                        {
                            DalsiaZastavka = Zastavky[30],
                            CasKDalsejZastavke = 7.1
                        }
                    ),
                    new KeyValuePair<Zastavka, ZastavkaData>(
                        Zastavky[30],
                        new ZastavkaData()
                        {
                            DalsiaZastavka = Zastavky[31],
                            CasKDalsejZastavke = 4.8
                        }
                    ),
                    new KeyValuePair<Zastavka, ZastavkaData>(
                        Zastavky[31],
                        new ZastavkaData()
                        {
                            DalsiaZastavka = Zastavky[32],
                            CasKDalsejZastavke = 3.7
                        }
                    ),
                    new KeyValuePair<Zastavka, ZastavkaData>(
                        Zastavky[32],
                        new ZastavkaData()
                        {
                            DalsiaZastavka = Zastavky[3],
                            CasKDalsejZastavke = 7.2
                        }
                    ),
                   }
                },
            };

            foreach (var linka in Linky)
            {
                double casKuStadionu = 0;
                foreach (var zastavka in linka.Zastavky)
                {
                    casKuStadionu += zastavka.Value.CasKDalsejZastavke;
                }

                for (int i = 0; i < linka.Zastavky.Count; i++)
                {
                    if (i == 0)
                    {
                        linka.Zastavky[i].Value.CasKuStadionu = casKuStadionu;
                    }
                    else
                    {
                        casKuStadionu -= linka.Zastavky[i - 1].Value.CasKDalsejZastavke;
                        linka.Zastavky[i].Value.CasKuStadionu = casKuStadionu;

                    }
                }
            }
        }

        public void VypisZastavky()
        {
            foreach (var linka in Linky)
            {
                foreach (var Zastavka in linka.Zastavky)
                {
                    Console.WriteLine($"{Zastavka.Key.Meno} {Zastavka.Value.CasKDalsejZastavke} {Zastavka.Key.CelkovyPocetCestujucich} {Zastavka.Value.CasKuStadionu} ({Zastavka.Value.DalsiaZastavka.Meno})");
                }

                Console.WriteLine();
                Console.WriteLine();
            }
          
        }

        public void VypisCestujucich()
        {
            Console.Clear();
            Console.WriteLine(TimeSpan.FromMinutes(MySim.CurrentTime));
            foreach (var zastavka in Zastavky)
            {
                Console.WriteLine(zastavka.Meno + " " + zastavka.Cestujuci.Count);
            }
        }
    }
}
