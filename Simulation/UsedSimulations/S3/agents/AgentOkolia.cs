using System;
using System.Collections.Generic;
using System.Threading;
using OSPABA;
using simulation;
using managers;
using continualAssistants;
using PropertyChanged;
using Simulations.Distributions;
using Simulations.UsedSimulations.S3;
using Simulations.UsedSimulations.S3.entities;
using System.Linq;

namespace agents
{
    [AddINotifyPropertyChangedInterface]
    //meta! id="8"
    public class AgentOkolia : Agent
    {
        public List<Linka> Linky { get; set; }
        public List<Zastavka> Zastavky { get; set; }
        public int CelkovyPocetCestujucich { get; set; }
        public AgentOkolia(int id, OSPABA.Simulation mySim, Agent parent) :
            base(id, mySim, parent)
        {
            Init();
            VytvorZastavky();
        }

        override public void PrepareReplication()
        {
            base.PrepareReplication();

            foreach (var zastavka in Zastavky)
            {
                zastavka.Reset();
            }

            CelkovyPocetCestujucich = 0;
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
            AddOwnMessage(Mc.CestujuciNastupil);
        }

        public void StartGenerovanieCestujucich()
        {
            foreach (var linka in Linky)
            {
                foreach (var zastavka in linka.Zastavky)
                {
                    //if (zastavka.Zastavka.Meno != "K1" && zastavka.Zastavka.Meno != "K2" && zastavka.Zastavka.Meno != "K3")
                    //{
                    var sprava = new MyMessage(MySim);
                    zastavka.CasKoncaGenerovania = (((MySimulation)MySim).Configration.ZaciatokZapasu - ((10 + zastavka.CasKuStadionu)));
                    sprava.ZastavkaData = zastavka;
                    sprava.ZastavkaData.CasZaciatkuGenerovania = (((MySimulation)MySim).Configration.ZaciatokZapasu - ((75 + zastavka.CasKuStadionu)));

                    if ((zastavka.Zastavka.Meno == "K1" || zastavka.Zastavka.Meno == "K2" || zastavka.Zastavka.Meno == "K3"))
                    {
                        var druha = (from y in Linky select (from x in y.Zastavky where x.Zastavka.Meno == zastavka.Zastavka.Meno select x).SingleOrDefault()).ToList().Where(x => x != null).ToList();
                        druha = druha.OrderBy(x => x.CasKuStadionu).ToList();


                        druha[0].CasKoncaGenerovania = (((MySimulation)MySim).Configration.ZaciatokZapasu - ((10 + druha[0].CasKuStadionu)));
                        druha[0].CasZaciatkuGenerovania = (((MySimulation)MySim).Configration.ZaciatokZapasu - ((75 + druha[1].CasKuStadionu)));

                        druha[1].CasKoncaGenerovania = (((MySimulation)MySim).Configration.ZaciatokZapasu - ((10 + druha[0].CasKuStadionu)));
                        druha[1].CasZaciatkuGenerovania = (((MySimulation)MySim).Configration.ZaciatokZapasu - ((75 + druha[1].CasKuStadionu)));

                        if (!druha[0].Zastavka.Vygenerovana)
                        {
                            druha[0].Zastavka.Vygenerovana = true;
                            druha[1].Zastavka.Vygenerovana = true;
                            sprava.ZastavkaData = druha[0];
                            sprava.Addressee = FindAssistant(SimId.PrichodyCestujucichNaZastavkuProces);
                            MyManager.StartContinualAssistant(sprava);
                        }
                    }
                    

                    sprava.Addressee = FindAssistant(SimId.PrichodyCestujucichNaZastavkuProces);

                    if (!zastavka.Zastavka.Vygenerovana)
                    {
                        MyManager.StartContinualAssistant(sprava);
                    }


                    // }
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
                    MaxPocetVygenerovanych = 260,
                },
                new Zastavka(MySim)
                {
                    Meno = "K2",
                    MaxPocetVygenerovanych = 210,
                },
                new Zastavka(MySim)
                {
                    Meno = "K3",
                    MaxPocetVygenerovanych = 220,
                },
                new Zastavka(MySim)
                {
                    Meno = "Stadion",
                },
            new Zastavka(MySim)
              {
                  Meno = "AA",
                  MaxPocetVygenerovanych = 123,
              },
              new Zastavka(MySim)
              {
                  Meno = "AB",
                  MaxPocetVygenerovanych = 92,
              },new Zastavka(MySim)
              {
                  Meno = "AC",
                  MaxPocetVygenerovanych = 241,
              }
              ,new Zastavka(MySim)
              {
                  Meno = "AD",
                  MaxPocetVygenerovanych = 123,
              },
              new Zastavka(MySim)
              {
                  Meno = "AE",
                  MaxPocetVygenerovanych = 215,
              },
              new Zastavka(MySim)
              {
                  Meno = "AF",
                  MaxPocetVygenerovanych = 245,
              },
              new Zastavka(MySim)
              {
                  Meno = "AG",
                  MaxPocetVygenerovanych = 137,
              },
              new Zastavka(MySim)
              {
                  Meno = "AH",
                  MaxPocetVygenerovanych = 132,
              },
              new Zastavka(MySim)
              {
                  Meno = "AI",
                  MaxPocetVygenerovanych = 164,
              },
              new Zastavka(MySim)
              {
                  Meno = "AJ",
                  MaxPocetVygenerovanych = 124,
              },
              new Zastavka(MySim)
              {
                  Meno = "AK",
                  MaxPocetVygenerovanych = 213,
              },
              new Zastavka(MySim)
              {
                  Meno = "AL",
                  MaxPocetVygenerovanych = 185,
              },


              new Zastavka(MySim)
              {
                  Meno = "BA",
                  MaxPocetVygenerovanych = 79,
              },
              new Zastavka(MySim)
              {
                  Meno = "BB",
                  MaxPocetVygenerovanych = 69,
              },
              new Zastavka(MySim)
              {
                  Meno = "BC",
                  MaxPocetVygenerovanych = 43,
              },
              new Zastavka(MySim)
              {
                  Meno = "BD",
                  MaxPocetVygenerovanych = 127,
              },
              new Zastavka(MySim)
              {
                  Meno = "BE",
                  MaxPocetVygenerovanych = 30,
              },
              new Zastavka(MySim)
              {
                  Meno = "BF",
                  MaxPocetVygenerovanych = 69,
              },
              new Zastavka(MySim)
              {
                  Meno = "BG",
                  MaxPocetVygenerovanych = 162,
              },
              new Zastavka(MySim)
              {
                  Meno = "BH",
                  MaxPocetVygenerovanych = 90,
              },
              new Zastavka(MySim)
              {
                  Meno = "BI",
                  MaxPocetVygenerovanych = 148,
              },
              new Zastavka(MySim)
              {
                  Meno = "BJ",
                  MaxPocetVygenerovanych = 171,
              },





              new Zastavka(MySim)
              {
                  Meno = "CA",
                  MaxPocetVygenerovanych = 240,
              },
              new Zastavka(MySim)
              {
                  Meno = "CB",
                  MaxPocetVygenerovanych = 310,
              },
              new Zastavka(MySim)
              {
                  Meno = "CC",
                  MaxPocetVygenerovanych = 131,
              },
              new Zastavka(MySim)
              {
                  Meno = "CD",
                  MaxPocetVygenerovanych = 190,
              },
              new Zastavka(MySim)
              {
                  Meno = "CE",
                  MaxPocetVygenerovanych = 132,
              },
              new Zastavka(MySim)
              {
                  Meno = "CF",
                  MaxPocetVygenerovanych = 128,
              },
              new Zastavka(MySim)
              {
                  Meno = "CG",
                  MaxPocetVygenerovanych = 70,
              },
            };

            //Linka A
            Linky = new List<Linka>()
            {
                new Linka(MySim)
                {

                Meno = "A",
                Zastavky = new List<ZastavkaData>()
                {
                    new ZastavkaData()
                    {
                        Zastavka = Zastavky[4],
                        CasKDalsejZastavke = 3.2,
                    }
                    ,
                        new ZastavkaData()
                        {
                            Zastavka = Zastavky[5],
                            CasKDalsejZastavke = 2.3
                        }
                    ,

                        new ZastavkaData()
                        {
                            Zastavka = Zastavky[6],
                            CasKDalsejZastavke = 2.1
                        }
                    ,

                        new ZastavkaData()
                        {
                            Zastavka = Zastavky[7],
                            CasKDalsejZastavke = 1.2
                        }
                    ,

                        new ZastavkaData()
                        {
                            Zastavka = Zastavky[0],
                            CasKDalsejZastavke = 5.4
                        }
                    ,

                        new ZastavkaData()
                        {
                            Zastavka = Zastavky[8],
                            CasKDalsejZastavke = 2.9
                        }
                    ,

                        new ZastavkaData()
                        {
                            Zastavka = Zastavky[9],
                            CasKDalsejZastavke = 3.4
                        }
                    ,

                        new ZastavkaData()
                        {
                            Zastavka = Zastavky[10],
                            CasKDalsejZastavke = 1.8
                        }
                    ,

                        new ZastavkaData()
                        {
                            Zastavka = Zastavky[2],
                            CasKDalsejZastavke = 4.0
                        }
                    ,

                        new ZastavkaData()
                        {
                            Zastavka = Zastavky[11],
                            CasKDalsejZastavke = 1.6
                        }
                    ,

                        new ZastavkaData()
                        {
                            Zastavka = Zastavky[12],
                            CasKDalsejZastavke = 4.6
                        }
                    ,

                        new ZastavkaData()
                        {
                            Zastavka = Zastavky[13],
                            CasKDalsejZastavke = 3.4
                        }
                    ,

                        new ZastavkaData()
                        {
                            Zastavka = Zastavky[14],
                            CasKDalsejZastavke = 1.2
                        }
                    ,

                        new ZastavkaData()
                        {
                            Zastavka = Zastavky[15],
                            CasKDalsejZastavke = 0.9
                        }
                    ,
                    new ZastavkaData()
                    {
                        Zastavka = Zastavky[3],
                        CasKDalsejZastavke = 25,
                        Konecna = true
                    }
                }
                },
                 new Linka(MySim)
                {

                Meno = "B",
                Zastavky = new List<ZastavkaData>()
                {
                    new ZastavkaData()
                    {
                        Zastavka = Zastavky[16],
                        CasKDalsejZastavke = 1.2,
                    }
                    ,

                        new ZastavkaData()
                        {
                            Zastavka = Zastavky[17],
                            CasKDalsejZastavke = 2.3
                        }
                    ,

                        new ZastavkaData()
                        {
                            Zastavka = Zastavky[18],
                            CasKDalsejZastavke = 3.2
                        }
                    ,

                        new ZastavkaData()
                        {
                            Zastavka = Zastavky[19],
                            CasKDalsejZastavke = 4.3
                        }
                    ,

                        new ZastavkaData()
                        {
                            Zastavka = Zastavky[1],
                            CasKDalsejZastavke = 1.2
                        }
                    ,

                        new ZastavkaData()
                        {
                            Zastavka = Zastavky[20],
                            CasKDalsejZastavke = 2.7
                        }
                    ,

                        new ZastavkaData()
                        {
                            Zastavka = Zastavky[21],
                            CasKDalsejZastavke = 3
                        }
                    ,

                        new ZastavkaData()
                        {
                            Zastavka = Zastavky[2],
                            CasKDalsejZastavke = 6
                        }
                    ,

                        new ZastavkaData()
                        {
                            Zastavka = Zastavky[22],
                            CasKDalsejZastavke = 4.3
                        }
                    ,

                        new ZastavkaData()
                        {
                            Zastavka = Zastavky[23],
                            CasKDalsejZastavke = 0.5
                        }
                    ,

                        new ZastavkaData()
                        {
                            Zastavka = Zastavky[24],
                            CasKDalsejZastavke = 2.7
                        }
                    ,

                        new ZastavkaData()
                        {
                            Zastavka = Zastavky[25],
                            CasKDalsejZastavke = 1.3
                        },
                        new ZastavkaData()
                        {
                            Zastavka = Zastavky[3],
                            CasKDalsejZastavke = 10,
                            Konecna = true
                        }

                }
                },

                  new Linka(MySim)
                {

                Meno = "C",
                Zastavky = new List<ZastavkaData>()
                {
                    new ZastavkaData()
                    {
                        Zastavka = Zastavky[26],
                        CasKDalsejZastavke = 0.6,
                    },
                    new ZastavkaData()
                    {
                         Zastavka = Zastavky[27],
                         CasKDalsejZastavke = 2.3
                    },
                    new ZastavkaData()
                    {
                            Zastavka = Zastavky[0],
                            CasKDalsejZastavke = 4.1
                    }
                    ,
                    new ZastavkaData()
                     {
                        Zastavka = Zastavky[1],
                         CasKDalsejZastavke = 6
                     },


                        new ZastavkaData()
                        {
                            Zastavka = Zastavky[28],
                            CasKDalsejZastavke = 2.3
                        }
                    ,

                        new ZastavkaData()
                        {
                            Zastavka = Zastavky[29],
                            CasKDalsejZastavke = 7.1
                        },


                        new ZastavkaData()
                        {
                            Zastavka = Zastavky[30],
                            CasKDalsejZastavke = 4.8
                        }
                    ,

                        new ZastavkaData()
                        {
                            Zastavka = Zastavky[31],
                            CasKDalsejZastavke = 3.7
                        }
                    ,
                        new ZastavkaData()
                        {
                            Zastavka = Zastavky[32],
                            CasKDalsejZastavke = 7.2
                        }
                    ,
                        new ZastavkaData()
                        {
                            Zastavka = Zastavky[3],
                            CasKDalsejZastavke = 30,
                            Konecna = true
                        },
                   }
                },
            };

            foreach (var linka in Linky)
            {
                for (int i = 0; i < linka.Zastavky.Count; i++)
                {
                    if (i != linka.Zastavky.Count - 1)
                    {
                        linka.Zastavky[i].DalsiaZastavka = linka.Zastavky[i + 1];
                    }
                    else
                    {
                        linka.Zastavky[i].DalsiaZastavka = linka.Zastavky[0];
                    }

                    if (linka.Zastavky[i].Zastavka.Meno == "K1")
                    {
                        linka.Zastavky[i].Generator = new ExponentialDistribution((1.0 / (71.0 / linka.Zastavky[i].Zastavka.MaxPocetVygenerovanych)),
                           ((MySimulation)MySim).Random.Next());
                    }
                    else if (linka.Zastavky[i].Zastavka.Meno == "K2")
                    {
                        linka.Zastavky[i].Generator = new ExponentialDistribution((1.0 / (74.4 / linka.Zastavky[i].Zastavka.MaxPocetVygenerovanych)),
                           ((MySimulation)MySim).Random.Next());
                    }
                    else if (linka.Zastavky[i].Zastavka.Meno == "K3")
                    {
                        linka.Zastavky[i].Generator = new ExponentialDistribution((1.0 / (65.9 / linka.Zastavky[i].Zastavka.MaxPocetVygenerovanych)),
                           ((MySimulation)MySim).Random.Next());
                    }
                    else
                        linka.Zastavky[i].Generator = new ExponentialDistribution((1.0 / (65.0 / linka.Zastavky[i].Zastavka.MaxPocetVygenerovanych)),
                         ((MySimulation)MySim).Random.Next());

                }
            }

            int index = 1;
            foreach (var linka in Linky)
            {
                double casKuStadionu = 0;
                foreach (var zastavka in linka.Zastavky)
                {
                    if (zastavka.Zastavka != Zastavky[3] && index != linka.Zastavky.Count)
                        casKuStadionu += zastavka.CasKDalsejZastavke;

                    index++;
                }

                for (int i = 0; i < linka.Zastavky.Count; i++)
                {
                    if (i == 0)
                    {
                        linka.Zastavky[i].CasKuStadionu = casKuStadionu;
                    }
                    else if (linka.Zastavky[i].Zastavka != Zastavky[3] && i != linka.Zastavky.Count - 1)
                    {
                        casKuStadionu -= linka.Zastavky[i - 1].CasKDalsejZastavke;
                        linka.Zastavky[i].CasKuStadionu = casKuStadionu;
                    }
                    else
                    {
                        linka.Zastavky[i].CasKuStadionu = -1;
                    }
                }
            }

        }
        public void VypisZastavky()
        {
            foreach (var linka in Linky)
            {
                foreach (var zastavkaData in linka.Zastavky)
                {
                    Console.WriteLine($"{zastavkaData.Zastavka.Meno} " +
                                      $"{zastavkaData.CasKDalsejZastavke} " +
                                      $"{zastavkaData.Zastavka.MaxPocetVygenerovanych} " +
                                      $"{zastavkaData.CasKuStadionu} " +
                                      $"({zastavkaData.DalsiaZastavka.Zastavka.Meno})");
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
