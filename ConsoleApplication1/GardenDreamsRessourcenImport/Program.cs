using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using EntityFramework.Extensions;

namespace GardenDreamsRessourcenImport
{
    class Program
    {
        public static FileInfo ErrorFile = new FileInfo(@"C:\Test\RessourcenImportErrors.txt");

        static void Main()
        {
            //UpdateArtikel();
            const int maxImport = 1;
            try
            {
                using (var context = new OLEntities())
                {
                    File.WriteAllText(ErrorFile.FullName, "Init" + Environment.NewLine);
                    var importList = (from c in context.amxImportRL
                                      select c).ToList();
                    KHKTan ppStan = (from c in context.KHKTan
                                     where c.Tabelle == "KHKPpsRessourcenPositionen"
                                     select c).First();
                    KHKTan agTan = (from c in context.KHKTan
                                    where c.Tabelle == "KHKPpsRessourcenAGPositionen"
                                    select c).First();

                    var belPosId = (int) ppStan.Tan;
                    var agPosId = (int) agTan.Tan;
                    var counter = 0;

                    var kopfBulk = new List<KHKPpsRessourcenkopf>();
                    var ressourcenbulk = new List<KHKPpsRessourcenPositionen>();
                    var ressourcenagbul = new List<KHKPpsRessourcenAGPositionen>();

                    foreach (amxImportRL importartikel in importList)
                    {
                        belPosId++;
                        agPosId++;
                        KHKPpsRessourcenkopf kopf = CreatePpsRessourcenKopf(importartikel, context);
                        if (kopf == null) continue;
                        var ressourcen = CreatePpsRessourcenPositionen(kopf, importartikel,
                            context, ref belPosId);
                        if (ressourcen == null) continue;
                        ressourcen = AddAgPositionen(kopf, ressourcen, context, ref belPosId, ref agPosId, ref ressourcenagbul);
                        kopfBulk.Add(kopf);
                        ressourcenbulk.AddRange(ressourcen);
                        ppStan.Tan = belPosId;
                        agTan.Tan = agPosId;
                        counter++;

                        if (counter == maxImport)
                        {
                            var artikellist = (from c in kopfBulk
                                               select c.Artikelnummer);
                            var importartikellist = (from c in context.amxImportRL
                                                     where artikellist.Any(x => c.ArtikelnummerNeu == x)
                                                     select c);
                            context.BulkInsert(kopfBulk);
                            context.BulkInsert(ressourcenbulk);
                            context.BulkInsert(ressourcenagbul);
                            context.BulkSaveChanges();
                            context.BulkDelete(importartikellist);
                            context.BulkSaveChanges();
                            kopfBulk = new List<KHKPpsRessourcenkopf>();
                            ressourcenbulk = new List<KHKPpsRessourcenPositionen>();
                            ressourcenagbul = new List<KHKPpsRessourcenAGPositionen>();
                            Console.WriteLine("Imported " + counter);
                            counter = 0;
                        }
                    }
                    context.Entry(ppStan).State = EntityState.Modified;
                    context.Entry(agTan).State = EntityState.Modified;
                    context.SaveChanges();
                    Console.WriteLine("OK!");
                    Console.ReadKey();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                Console.WriteLine(ex.InnerException);
                Console.ReadKey();
            }
        }

        private static KHKPpsRessourcenkopf CreatePpsRessourcenKopf(amxImportRL importartikel, OLEntities context)
        {
            try
            {
                var kopf = new KHKPpsRessourcenkopf
                {
                    Artikelnummer = importartikel.ArtikelnummerNeu,
                    AuspraegungID = 0,
                    UmrechnungsfaktorPZ = 1.00M,
                    Produktionszeit = 0,
                    Status = "E",
                    Mandant = 3,
                    Variante = "10",
                    BisMenge = 0,
                    VonMenge = 0
                };

                KHKArtikel artikel = (from c in context.KHKArtikel
                                      where c.Artikelnummer == kopf.Artikelnummer
                                      select c).FirstOrDefault();

                if (artikel == null) return kopf;
                kopf.Bezeichnung1 = artikel.Bezeichnung1;
                kopf.Bezeichnung2 = artikel.Bezeichnung2;
                kopf.Matchcode = artikel.Matchcode;

                return kopf;
            }
            catch (Exception ex)
            {
                File.AppendAllText(ErrorFile.FullName,
                "I-Artikel: " + importartikel.Artikelnummer + " CreatePPSRessourcenKopf " + " ERR: " + ex.Message + Environment.NewLine);
                return null;
            }
        }

        private static List<KHKPpsRessourcenPositionen> CreatePpsRessourcenPositionen(KHKPpsRessourcenkopf kopf,
                                                                                      amxImportRL importartikel,
                                                                                      OLEntities context,
                                                                                      ref int belPosId)
        {
            string mat = "";
            try
            {
                var ressourcenlist = new List<KHKPpsRessourcenPositionen>();
                Regex regexmat = new Regex("^[M][0-9]+$");
                Regex regexanz = new Regex("^[A][0-9]+$");
                var matprops = importartikel.GetType().GetProperties().Where(x => regexmat.IsMatch(x.Name));
                var anzahlprops = importartikel.GetType().GetProperties().Where(x => regexanz.IsMatch(x.Name));
                short positioncounter = 1;
                foreach (var propertyInfo in matprops)
                {
                    mat = (string) propertyInfo.GetValue(importartikel);
                    if (mat == null) continue;
                    var anzahl = (double)
                                 anzahlprops.FirstOrDefault(x => x.Name == propertyInfo.Name.Replace("M", "A"))
                                            .GetValue(importartikel);
                    var ressourcenartikel = (from c in context.KHKArtikel
                                             where c.Artikelnummer == mat
                                             select c).FirstOrDefault();
                    ressourcenlist.Add(new KHKPpsRessourcenPositionen
                    {
                        Artikelnummer = kopf.Artikelnummer,
                        Variante = kopf.Variante,
                        Mandant = 3,
                        Position = positioncounter,
                        PositionExtern = positioncounter * 10,
                        PositionExternAnzeige = (positioncounter * 10).ToString(),
                        Ressourcetyp = "MA",
                        Ressourcenummer = mat,
                        Matchcode = ressourcenartikel.Matchcode,
                        Dimensionstext = ressourcenartikel.Dimensionstext,
                        Menge = Convert.ToDecimal(anzahl),
                        MengeNKS = ressourcenartikel.DezimalstellenBasis,
                        Lagermengeneinheit = ressourcenartikel.Lagermengeneinheit,
                        PlatzID = ressourcenartikel.PlatzID,
                        BelPosID = belPosId,
                        RessourceTypAg = 0,
                        Leistungsgrad = 100,
                        Umrechnungsfaktor = 1,
                        Ruestzeit = 0,
                        Stueckzeit = 0,
                        Pufferzeit = 0,
                        Ueberlappungzeit = 0,
                        Preis = 0,
                        Transportzeit = 0,
                        AuspraegungID = 0,
                        Ueberlappungprozent = 0,
                        IstTextuebergabe = 0,
                        PosVariante = "0",
                        Anzahl = 0,
                        DIM1 = 0,
                        DIM2 = 0,
                        DIM3 = 0,
                        SollEinzelpreis = 0
                    });
                    positioncounter++;
                    belPosId++;
                }
                return ressourcenlist;
            }
            catch (Exception ex)
            {
                File.AppendAllText(ErrorFile.FullName,
                    "I-Artikel: " + importartikel.Artikelnummer + " Mat: "+ mat + " CreatePPSRessourcenPositionen " + " ERR: " + ex.Message + Environment.NewLine);
                return null;
            }
        }

        static List<KHKPpsRessourcenPositionen> AddAgPositionen(KHKPpsRessourcenkopf kopf,
                                                                List<KHKPpsRessourcenPositionen> ressourcenlist,
                                                                OLEntities context, ref int belPosId, ref int agPosId, ref List<KHKPpsRessourcenAGPositionen> ressourcenagbul)
        {
            try
            {
                ressourcenlist.Add(AddAgPicken(kopf, ressourcenlist, context, belPosId, agPosId, ref ressourcenagbul)); //Picken erstellen
                belPosId++;
                agPosId++;
                ressourcenlist.Add(AddAgSaegen(kopf, ressourcenlist, context, belPosId, agPosId, ref ressourcenagbul)); //Saegen erstellen
                belPosId++;
                agPosId++;
                ressourcenlist.Add(AddAgMontieren(kopf, ressourcenlist, context, belPosId, agPosId,ref ressourcenagbul)); //Montieren erstellen
                belPosId++;
                agPosId++;
                ressourcenlist.Add(AddAgVerpacken(kopf, ressourcenlist, context, belPosId, agPosId,ref ressourcenagbul)); //Verpacken erstellen
                belPosId++;
                agPosId++;
                ressourcenlist.Add(AddAgQc(kopf, ressourcenlist, context, belPosId, agPosId,ref ressourcenagbul)); //Verpacken erstellen
                return ressourcenlist;
            }
            catch (Exception ex)
            {
                File.AppendAllText(ErrorFile.FullName,
                    "I-Artikel: " + kopf.Artikelnummer + " AddAGPositionen " + " ERR: " + ex.Message + Environment.NewLine);
                return null;
            }

        }

        static KHKPpsRessourcenPositionen AddAgPicken(KHKPpsRessourcenkopf kopf,
                                                      List<KHKPpsRessourcenPositionen> ressourcenlist,
                                                      OLEntities context, int belPosId, int agPosId, ref List<KHKPpsRessourcenAGPositionen> ressourcenagbul)
        {
            short position = (short) (ressourcenlist.Max(x => x.Position) + 1);
            KHKPpsArbeitsgaenge agPicken = (from c in context.KHKPpsArbeitsgaenge
                                            where c.Arbeitsgangnummer == "001"
                                            select c).FirstOrDefault();
            KHKPpsRessourcenPositionen pos = new KHKPpsRessourcenPositionen
            {
                Artikelnummer = kopf.Artikelnummer,
                Variante = kopf.Variante,
                AuspraegungIDKopf = kopf.AuspraegungID,
                Mandant = kopf.Mandant,
                Position = position,
                Ressourcetyp = "AG",
                Ressourcenummer = agPicken.Arbeitsgangnummer,
                Matchcode = agPicken.Matchcode,
                Ruestzeittext = agPicken.Ruestzeittext,
                Ruestzeit = agPicken.Ruestzeit,
                Stueckzeittext = agPicken.Stueckzeittext,
                Stueckzeit = agPicken.Stueckzeit,
                Pufferzeit = agPicken.Pufferzeit,
                Ueberlappungzeittext = agPicken.Pufferzeittext,
                Ueberlappungzeit = agPicken.Ueberlappungzeit,
                Ueberlappungmenge = agPicken.Ueberlappungmenge,
                BelPosID = belPosId,
                PositionExtern = position * 10,
                PositionExternAnzeige = (position * 10).ToString(),
                RessourceNummerAg = agPicken.DefaultRessourceNummer,
                MindestmengeAG = 1,
                Dimensionstext = agPicken.Dimensionstext,
                Umrechnungsfaktor = 1,
                IstTextuebergabe = 0,
                Leistungsgrad = 100,
            };

            KHKPpsRessourcenAGPositionen agpos = new KHKPpsRessourcenAGPositionen
            {
                BelAGPosID = agPosId,
                Mandant = kopf.Mandant,
                BelPosID = pos.BelPosID,
                Position = 1,
                RessourceTypAg = 1,
                RessourcenummerAg = pos.RessourceNummerAg,
                Matchcode = (from c in context.KHKPpsArbeitsplaetze
                             where c.Arbeitsplatznummer == pos.RessourceNummerAg
                             select c.Matchcode).FirstOrDefault()
            };
            ressourcenagbul.Add(agpos);
            return pos;
        }

        static KHKPpsRessourcenPositionen AddAgSaegen(KHKPpsRessourcenkopf kopf,
                                                      List<KHKPpsRessourcenPositionen> ressourcenlist,
                                                      OLEntities context, int belPosId, int agPosId, ref List<KHKPpsRessourcenAGPositionen> ressourcenagbul)
        {
            short position = (short) (ressourcenlist.Max(x => x.Position) + 1);
            KHKPpsArbeitsgaenge agSaegen = (from c in context.KHKPpsArbeitsgaenge
                                            where c.Arbeitsgangnummer == "002"
                                            select c).FirstOrDefault();
            KHKPpsRessourcenPositionen pos = new KHKPpsRessourcenPositionen
            {
                Artikelnummer = kopf.Artikelnummer,
                Variante = kopf.Variante,
                AuspraegungIDKopf = kopf.AuspraegungID,
                Mandant = kopf.Mandant,
                Position = position,
                Ressourcetyp = "AG",
                Ressourcenummer = agSaegen.Arbeitsgangnummer,
                Matchcode = agSaegen.Matchcode,
                Ruestzeittext = agSaegen.Ruestzeittext,
                Ruestzeit = agSaegen.Ruestzeit,
                Stueckzeittext = agSaegen.Stueckzeittext,
                Stueckzeit = agSaegen.Stueckzeit,
                Pufferzeit = agSaegen.Pufferzeit,
                Ueberlappungzeittext = agSaegen.Pufferzeittext,
                Ueberlappungzeit = agSaegen.Ueberlappungzeit,
                Ueberlappungmenge = agSaegen.Ueberlappungmenge,
                BelPosID = belPosId,
                PositionExtern = position * 10,
                PositionExternAnzeige = (position * 10).ToString(),
                RessourceNummerAg = agSaegen.DefaultRessourceNummer,
                MindestmengeAG = 1,
                Dimensionstext = agSaegen.Dimensionstext,
                Umrechnungsfaktor = 1,
                IstTextuebergabe = 0,
                Leistungsgrad = 100
            };

            KHKPpsRessourcenAGPositionen agpos = new KHKPpsRessourcenAGPositionen
            {
                BelAGPosID = agPosId,
                Mandant = kopf.Mandant,
                BelPosID = pos.BelPosID,
                Position = 1,
                RessourceTypAg = 1,
                RessourcenummerAg = pos.RessourceNummerAg,
                Matchcode = (from c in context.KHKPpsArbeitsplaetze
                             where c.Arbeitsplatznummer == pos.RessourceNummerAg
                             select c.Matchcode).FirstOrDefault()
            };
            ressourcenagbul.Add(agpos);
            return pos;
        }

        static KHKPpsRessourcenPositionen AddAgMontieren(KHKPpsRessourcenkopf kopf,
                                                         List<KHKPpsRessourcenPositionen> ressourcenlist,
                                                         OLEntities context, int belPosId, int agPosId, ref List<KHKPpsRessourcenAGPositionen> ressourcenagbul)
        {
            short position = (short) (ressourcenlist.Max(x => x.Position) + 1);
            KHKPpsArbeitsgaenge agMontieren = (from c in context.KHKPpsArbeitsgaenge
                                               where c.Arbeitsgangnummer == "006"
                                               select c).FirstOrDefault();
            KHKPpsRessourcenPositionen pos = new KHKPpsRessourcenPositionen
            {
                Artikelnummer = kopf.Artikelnummer,
                Variante = kopf.Variante,
                AuspraegungIDKopf = kopf.AuspraegungID,
                Mandant = kopf.Mandant,
                Position = position,
                Ressourcetyp = "AG",
                Ressourcenummer = agMontieren.Arbeitsgangnummer,
                Matchcode = agMontieren.Matchcode,
                Ruestzeittext = agMontieren.Ruestzeittext,
                Ruestzeit = agMontieren.Ruestzeit,
                Stueckzeittext = agMontieren.Stueckzeittext,
                Stueckzeit = agMontieren.Stueckzeit,
                Pufferzeit = agMontieren.Pufferzeit,
                Ueberlappungzeittext = agMontieren.Pufferzeittext,
                Ueberlappungzeit = agMontieren.Ueberlappungzeit,
                Ueberlappungmenge = agMontieren.Ueberlappungmenge,
                BelPosID = belPosId,
                PositionExtern = position * 10,
                PositionExternAnzeige = (position * 10).ToString(),
                RessourceNummerAg = agMontieren.DefaultRessourceNummer,
                MindestmengeAG = 1,
                Dimensionstext = agMontieren.Dimensionstext,
                Umrechnungsfaktor = 1,
                IstTextuebergabe = 0,
                Leistungsgrad = 100
            };

            KHKPpsRessourcenAGPositionen agpos = new KHKPpsRessourcenAGPositionen
            {
                BelAGPosID = agPosId,
                Mandant = kopf.Mandant,
                BelPosID = pos.BelPosID,
                Position = 1,
                RessourceTypAg = 1,
                RessourcenummerAg = pos.RessourceNummerAg,
                Matchcode = (from c in context.KHKPpsArbeitsplaetze
                             where c.Arbeitsplatznummer == pos.RessourceNummerAg
                             select c.Matchcode).FirstOrDefault()
            };
            ressourcenagbul.Add(agpos);
            return pos;
        }

        static KHKPpsRessourcenPositionen AddAgVerpacken(KHKPpsRessourcenkopf kopf,
                                                         List<KHKPpsRessourcenPositionen> ressourcenlist,
                                                         OLEntities context, int belPosId, int agPosId, ref List<KHKPpsRessourcenAGPositionen> ressourcenagbul)
        {
            short position = (short) (ressourcenlist.Max(x => x.Position) + 1);
            KHKPpsArbeitsgaenge agVerpacken = (from c in context.KHKPpsArbeitsgaenge
                                               where c.Arbeitsgangnummer == "004"
                                               select c).FirstOrDefault();
            KHKPpsRessourcenPositionen pos = new KHKPpsRessourcenPositionen
            {
                Artikelnummer = kopf.Artikelnummer,
                Variante = kopf.Variante,
                AuspraegungIDKopf = kopf.AuspraegungID,
                Mandant = kopf.Mandant,
                Position = position,
                Ressourcetyp = "AG",
                Ressourcenummer = agVerpacken.Arbeitsgangnummer,
                Matchcode = agVerpacken.Matchcode,
                Ruestzeittext = agVerpacken.Ruestzeittext,
                Ruestzeit = agVerpacken.Ruestzeit,
                Stueckzeittext = agVerpacken.Stueckzeittext,
                Stueckzeit = agVerpacken.Stueckzeit,
                Pufferzeit = agVerpacken.Pufferzeit,
                Ueberlappungzeittext = agVerpacken.Pufferzeittext,
                Ueberlappungzeit = agVerpacken.Ueberlappungzeit,
                Ueberlappungmenge = agVerpacken.Ueberlappungmenge,
                BelPosID = belPosId,
                PositionExtern = position * 10,
                PositionExternAnzeige = (position * 10).ToString(),
                RessourceNummerAg = agVerpacken.DefaultRessourceNummer,
                MindestmengeAG = 1,
                Dimensionstext = agVerpacken.Dimensionstext,
                Umrechnungsfaktor = 1,
                IstTextuebergabe = 0,
                Leistungsgrad = 100
            };

            KHKPpsRessourcenAGPositionen agpos = new KHKPpsRessourcenAGPositionen
            {
                BelAGPosID = agPosId,
                Mandant = kopf.Mandant,
                BelPosID = pos.BelPosID,
                Position = 1,
                RessourceTypAg = 1,
                RessourcenummerAg = pos.RessourceNummerAg,
                Matchcode = (from c in context.KHKPpsArbeitsplaetze
                             where c.Arbeitsplatznummer == pos.RessourceNummerAg
                             select c.Matchcode).FirstOrDefault()
            };
            ressourcenagbul.Add(agpos);
            return pos;
        }


        static KHKPpsRessourcenPositionen AddAgQc(KHKPpsRessourcenkopf kopf,
                                                  List<KHKPpsRessourcenPositionen> ressourcenlist,
                                                  OLEntities context, int belPosId, int agPosId, ref List<KHKPpsRessourcenAGPositionen> ressourcenagbul)
        {
            short position = (short) (ressourcenlist.Max(x => x.Position) + 1);
            KHKPpsArbeitsgaenge agQualitaetskontrolle = (from c in context.KHKPpsArbeitsgaenge
                                                         where c.Arbeitsgangnummer == "009"
                                                         select c).FirstOrDefault();
            KHKPpsRessourcenPositionen pos = new KHKPpsRessourcenPositionen
            {
                Artikelnummer = kopf.Artikelnummer,
                Variante = kopf.Variante,
                AuspraegungIDKopf = kopf.AuspraegungID,
                Mandant = kopf.Mandant,
                Position = position,
                Ressourcetyp = "AG",
                Ressourcenummer = agQualitaetskontrolle.Arbeitsgangnummer,
                Matchcode = agQualitaetskontrolle.Matchcode,
                Ruestzeittext = agQualitaetskontrolle.Ruestzeittext,
                Ruestzeit = agQualitaetskontrolle.Ruestzeit,
                Stueckzeittext = agQualitaetskontrolle.Stueckzeittext,
                Stueckzeit = agQualitaetskontrolle.Stueckzeit,
                Pufferzeit = agQualitaetskontrolle.Pufferzeit,
                Ueberlappungzeittext = agQualitaetskontrolle.Pufferzeittext,
                Ueberlappungzeit = agQualitaetskontrolle.Ueberlappungzeit,
                Ueberlappungmenge = agQualitaetskontrolle.Ueberlappungmenge,
                BelPosID = belPosId,
                PositionExtern = position * 10,
                PositionExternAnzeige = (position * 10).ToString(),
                RessourceNummerAg = agQualitaetskontrolle.DefaultRessourceNummer,
                MindestmengeAG = 1,
                Dimensionstext = agQualitaetskontrolle.Dimensionstext,
                Umrechnungsfaktor = 1,
                IstTextuebergabe = 0,
                Leistungsgrad = 100
            };

            KHKPpsRessourcenAGPositionen agpos = new KHKPpsRessourcenAGPositionen
            {
                BelAGPosID = agPosId,
                Mandant = kopf.Mandant,
                BelPosID = pos.BelPosID,
                Position = 1,
                RessourceTypAg = 1,
                RessourcenummerAg = pos.RessourceNummerAg,
                Matchcode = (from c in context.KHKPpsArbeitsplaetze
                             where c.Arbeitsplatznummer == pos.RessourceNummerAg
                             select c.Matchcode).FirstOrDefault()
            };
            ressourcenagbul.Add(agpos);
            return pos;
        }

        static void UpdateArtikel()
        {
            try
            {
                string filePath = @"C:\Test\RESSOURCE LISTE TOTAAL UPLOAD.xlsx";
                using (var context = new OLEntities())
                {
                    Console.WriteLine("Starting");

                    var importList = (from c in context.amxImportRL
                                      select c).ToList();
                    Console.WriteLine("Importlist loaded");
                    int i = 1;
                    //KHKArtikel modartikel;
                    Console.WriteLine("Beginne");
                    foreach (amxImportRL item in importList)
                    {
                        context.KHKArtikel.Where(t => t.Artikelnummer == item.ArtikelnummerNeu)
                               .Update(t => new KHKArtikel
                               {
                                   Matchcode = item.Matchcode
                               });

                        if ((i == 1000) || (i == 2000) || (i == 50000) || (i == 100000)) Console.WriteLine(i);
                        i++;
                    }
                    Console.WriteLine(i + " Artikel updated!");

                    Console.WriteLine("Finished");
                    Console.ReadKey();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                Console.WriteLine(ex.InnerException);
                Console.ReadKey();
            }
        }
    }
}