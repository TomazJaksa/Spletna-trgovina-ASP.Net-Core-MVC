using Voleska.Models;
using System;
using System.Linq;
using Voleska.Areas.Identity.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNet.Identity;

namespace Voleska.Data
{
    public static class DbInitializer
    {
        public static void Initialize(ApplicationDbContext context, Microsoft.AspNetCore.Identity.UserManager<ApplicationUser> userManager,
Microsoft.AspNetCore.Identity.RoleManager<ApplicationRole> roleManager)
        {
            context.Database.EnsureCreated();

            // Preveri, če obstaja kakšen izdelek
            if (context.Izdelki.Any())
            {
                return;   // DB has been seeded
            }

            var tipiIzdelkov = new TipIzdelka[]
           {
            new TipIzdelka{Ime="Uhani", Slika="c15d704d-a223-4ca2-a258-2a488fae9f53-Uhani6.jpg"},
            new TipIzdelka{Ime="Broške", Slika="c57fa462-59c3-412f-93ce-93c042d98e73-Broška3.jpg"},
            new TipIzdelka{Ime="Zapestnice", Slika="e8236209-f949-4a1e-b047-8d6810910999-Zapestnice3.jpg"},
            new TipIzdelka{Ime="Ogrlice", Slika="6c5653ff-f29c-4ea7-84f0-32f67a322e6c-Ogrlice3.jpg"},
            new TipIzdelka{Ime="Prstani", Slika="f41f033d-5275-4270-b900-446dc14c3090-Prstani2.jpg"},
            new TipIzdelka{Ime="Obeski za ključe", Slika="92fb61e2-bac0-4a26-9c91-34add753246a-Obeski4.jpg"}
           };
            foreach (TipIzdelka t in tipiIzdelkov)
            {
                context.TipiIzdelkov.Add(t);
            }
            context.SaveChanges();

            var materiali = new Material[]
            {
            new Material{Ime="Nerjaveče jeklo", Opis="Osnovni material. Ne rjavi."},
            new Material{Ime="Titan", Opis="Bolj kakovostna izbira materiala. Ne rjavi in je trdnejši."},
            };
            foreach (Material m in materiali)
            {
                context.Materiali.Add(m);
            }
            context.SaveChanges();

            var izdelki = new Izdelek[]
            {
            new Izdelek{Ime="Vesolje",Opis="Vesolje ima zelo preprost opis.",Podrobnosti="Jaz predstavljam podrobnosti izdelka. Sem bolj obsežen opis.", Cena=3.99m, Izbrisan=false,  Dodan=DateTime.Parse("2020-08-14"), Posodobljen=DateTime.Parse("2020-08-14"), MaterialID=1,TipIzdelkaID=1},
            new Izdelek{Ime="Ptiček",Opis="Ptiček ima zelo preprost opis.",Podrobnosti="Jaz predstavljam podrobnosti izdelka. Sem bolj obsežen opis.", Cena=4.99m, Izbrisan=false,  Dodan=DateTime.Parse("2020-08-14"), Posodobljen=DateTime.Parse("2020-08-14"), MaterialID=2,TipIzdelkaID=2},
            new Izdelek{Ime="Vrtnica",Opis="Rožica ima zelo preprost opis.",Podrobnosti="Jaz predstavljam podrobnosti izdelka. Sem bolj obsežen opis.", Cena=5.99m, Izbrisan=false,  Dodan=DateTime.Parse("2020-08-14"), Posodobljen=DateTime.Parse("2020-08-14"), MaterialID=1,TipIzdelkaID=3}

            };
            foreach (Izdelek i in izdelki)
            {
                context.Izdelki.Add(i);
            }
            context.SaveChanges();

            var tipiOpcij = new TipOpcije[]
            {
            new TipOpcije{Ime="Barva"}

            };
            foreach (TipOpcije t in tipiOpcij)
            {
                context.TipiOpcij.Add(t);
            }
            context.SaveChanges();

            var opcije = new Opcija[]
            {
            new Opcija{Ime="Smaragdna tančica",Slika="Slikica pride tukaj" ,Zaloga=true, Dodan=DateTime.Parse("2020-08-14"), Posodobljen=DateTime.Parse("2020-08-14"), IzdelekID=1, TipOpcijeID=1},
            new Opcija{Ime="Rubinasta tančica",Slika="Slikica pride tukaj" ,Zaloga=true, Dodan=DateTime.Parse("2020-08-14"), Posodobljen=DateTime.Parse("2020-08-14"), IzdelekID=1, TipOpcijeID=1},
            new Opcija{Ime="Modra",Slika="Slikica pride tukaj" ,Zaloga=true, Dodan=DateTime.Parse("2020-08-14"), Posodobljen=DateTime.Parse("2020-08-14"), IzdelekID=2, TipOpcijeID=1},
            new Opcija{Ime="Siva",Slika="Slikica pride tukaj" ,Zaloga=true, Dodan=DateTime.Parse("2020-08-14"), Posodobljen=DateTime.Parse("2020-08-14"), IzdelekID=2, TipOpcijeID=1},
            new Opcija{Ime="Rdeča",Slika="Slikica pride tukaj" ,Zaloga=true, Dodan=DateTime.Parse("2020-08-14"), Posodobljen=DateTime.Parse("2020-08-14"), IzdelekID=3, TipOpcijeID=1},
            new Opcija{Ime="Bela",Slika="Slikica pride tukaj" ,Zaloga=true, Dodan=DateTime.Parse("2020-08-14"), Posodobljen=DateTime.Parse("2020-08-14"), IzdelekID=3, TipOpcijeID=1},

            };
            foreach (Opcija o in opcije)
            {
                context.Opcije.Add(o);
            }
            context.SaveChanges();


            var poste = new Posta[]
            {
            new Posta{PostnaStevilka="8000", Kraj="Novo mesto", Dodan=DateTime.Parse("2020-08-14"), Posodobljen=DateTime.Parse("2020-08-14")},
            new Posta{PostnaStevilka="8220", Kraj="Šmarješke Toplice", Dodan=DateTime.Parse("2020-08-14"), Posodobljen=DateTime.Parse("2020-08-14")},
            new Posta{PostnaStevilka="8000", Kraj="Šmarješke Toplice", Dodan=DateTime.Parse("2020-08-14"), Posodobljen=DateTime.Parse("2020-08-14")}

            };
            foreach (Posta p in poste)
            {
                context.Poste.Add(p);
            }
            context.SaveChanges();

            var naslovi = new Naslov[]
            {
            new Naslov{Ulica="Kandijska cesta", HisnaStevilka="37a", Dodan=DateTime.Parse("2020-08-14"), Posodobljen=DateTime.Parse("2020-08-14"), PostaID=1},
            new Naslov{Ulica="Brezovica", HisnaStevilka="26", Dodan=DateTime.Parse("2020-08-14"), Posodobljen=DateTime.Parse("2020-08-14"), PostaID=2},
            new Naslov{Ulica="Ulica Slavka Gruma", HisnaStevilka="14", Dodan=DateTime.Parse("2020-08-14"), Posodobljen=DateTime.Parse("2020-08-14"), PostaID=3}
            };
            foreach (Naslov n in naslovi)
            {
                context.Naslovi.Add(n);
            }
            context.SaveChanges();

            var vloge = new ApplicationRole[]
            {
            new ApplicationRole{Name="ADMINISTRATOR", NormalizedName="ADMINISTRATOR"},
            new ApplicationRole{Name="UPORABNIK", NormalizedName="UPORABNIK"},
            };
            foreach (ApplicationRole v in vloge)
            {
                context.Roles.Add(v);
            }
            context.SaveChanges();

            var uporabniki = new ApplicationUser[]
            {
            new ApplicationUser{UserName="TomazJaksa", Email="jaksa.tomaz@gmail.com" , Ime="Tomaž", Priimek="Jakša", Aktiven=true, Dodan=DateTime.Parse("2021-01-01"), Posodobljen=DateTime.Parse("2021-01-01"), NaslovID=1,SecurityStamp = Guid.NewGuid().ToString()},
            
            };
            foreach (ApplicationUser u in uporabniki)
            {
                

                Microsoft.AspNetCore.Identity.IdentityResult result = userManager.CreateAsync(u, "Android51!").Result;
                if (result.Succeeded)
                {
                    userManager.AddToRoleAsync(u,
                                        "ADMINISTRATOR").Wait();
                }
                context.Users.Add(u);
            }
            context.SaveChanges();

            
            var blogi = new Blog[]
            {
            new Blog{Naslov="Prvi Blog", Slika="Tukaj pride slika", Povzetek="Jaz sem povzetek" , Clanek="Sem članek. In imam nekoliko več teksta.", Bloger="Colarič Urška", Dodan=DateTime.Parse("2020-08-14"), Posodobljen=DateTime.Parse("2020-08-14"), SteviloDislike=2, SteviloLike=10},
            new Blog{Naslov="Drugi Blog", Slika="Tukaj pride slika", Povzetek="Jaz sem povzetek" , Clanek="Sem članek. In imam nekoliko več teksta.", Bloger="Colarič Urška", Dodan=DateTime.Parse("2020-08-14"), Posodobljen=DateTime.Parse("2020-08-14"), SteviloDislike=2, SteviloLike=10},
            new Blog{Naslov="Tretji Blog", Slika="Tukaj pride slika", Povzetek="Jaz sem povzetek" , Clanek="Sem članek. In imam nekoliko več teksta.", Bloger="Colarič Urška", Dodan=DateTime.Parse("2020-08-14"), Posodobljen=DateTime.Parse("2020-08-14"), SteviloDislike=2, SteviloLike=10}

            };
            foreach (Blog b in blogi)
            {
                context.Blogi.Add(b);
            }
            context.SaveChanges();

            var komentarji = new Komentar[]
            {
            new Komentar{Vsebina="Ta blog je res zanimiv!", Dodan=DateTime.Parse("2020-08-14"), Posodobljen=DateTime.Parse("2020-08-14"), SteviloDislike=3, SteviloLike=20, ApplicationUserID="227e9b8e-790b-4f77-90c3-237428798ec1", BlogID=1},
            new Komentar{Vsebina="Ta blog je res povprečen!", Dodan=DateTime.Parse("2020-08-14"), Posodobljen=DateTime.Parse("2020-08-14"), SteviloDislike=6, SteviloLike=6, ApplicationUserID="6aa55c98-8963-41be-98e0-85a62a81ac6f", BlogID=2},
            new Komentar{Vsebina="Ta blog je res klišejast!", Dodan=DateTime.Parse("2020-08-14"), Posodobljen=DateTime.Parse("2020-08-14"), SteviloDislike=5, SteviloLike=1, ApplicationUserID="f7cc55e7-6154-4065-aa48-c7ce78d029ad", BlogID=3}

            };
            foreach (Komentar k in komentarji)
            {
                context.Komentarji.Add(k);
            }
            context.SaveChanges();

            var lajkaniKomentarji = new LajkanjeKomentarjev[]
            {
            new LajkanjeKomentarjev{KomentarID=1, ApplicationUserID="3c189f0c-72e0-41ea-8f45-940f7b411514", Lajk= true, Dodan=DateTime.Parse("2020-08-14"), Posodobljen=DateTime.Parse("2020-08-14")},
            new LajkanjeKomentarjev{KomentarID=2, ApplicationUserID="b515f38a-777b-4107-831f-d5e7d4939b53", Lajk= false, Dodan=DateTime.Parse("2020-08-14"), Posodobljen=DateTime.Parse("2020-08-14")},
            new LajkanjeKomentarjev{KomentarID=3, ApplicationUserID="f35b8fa5-ad02-48e7-bc16-0e3f14a44119", Lajk= false, Dodan=DateTime.Parse("2020-08-14"), Posodobljen=DateTime.Parse("2020-08-14")},

            };
            foreach (LajkanjeKomentarjev l in lajkaniKomentarji)
            {
                context.LajkaniKomentarji.Add(l);
            }
            context.SaveChanges();

            var lajkaniBlogi = new LajkanjeBlogov[]
            {
            new LajkanjeBlogov{BlogID=1, ApplicationUserID="f35b8fa5-ad02-48e7-bc16-0e3f14a44119", Lajk= true, Dodan=DateTime.Parse("2020-08-14"), Posodobljen=DateTime.Parse("2020-08-14")},
            new LajkanjeBlogov{BlogID=2, ApplicationUserID="b515f38a-777b-4107-831f-d5e7d4939b53", Lajk= false, Dodan=DateTime.Parse("2020-08-14"), Posodobljen=DateTime.Parse("2020-08-14")},
            new LajkanjeBlogov{BlogID=3, ApplicationUserID="3c189f0c-72e0-41ea-8f45-940f7b411514", Lajk= true, Dodan=DateTime.Parse("2020-08-14"), Posodobljen=DateTime.Parse("2020-08-14")},

            };
            foreach (LajkanjeBlogov l in lajkaniBlogi)
            {
                context.LajkaniBlogi.Add(l);
            }
            context.SaveChanges();


            var transakcije = new Transakcija[]
            {
            new Transakcija{ID=1 ,SkupniZnesek=3.99m, Dodan=DateTime.Parse("2020-08-14"), ApplicationUserID="f35b8fa5-ad02-48e7-bc16-0e3f14a44119"},
            new Transakcija{ID=2 ,SkupniZnesek=7.98m, Dodan=DateTime.Parse("2020-08-14"), ApplicationUserID="b515f38a-777b-4107-831f-d5e7d4939b53"},
            new Transakcija{ID=3 ,SkupniZnesek=21.95m, Dodan=DateTime.Parse("2020-08-14"), ApplicationUserID=""}

            };
            foreach (Transakcija t in transakcije)
            {
                context.Transakcije.Add(t);
            }
            context.SaveChanges();


            var narocila = new Narocilo[]
            {
            new Narocilo{Kolicina=1, Znesek= 3.99m, Dodan=DateTime.Parse("2020-08-14"), IzdelekID=1, TransakcijaID=1},
            new Narocilo{Kolicina=2, Znesek= 7.98m, Dodan=DateTime.Parse("2020-08-14"), IzdelekID=1, TransakcijaID=2},
            new Narocilo{Kolicina=4, Znesek= 15.96m, Dodan=DateTime.Parse("2020-08-14"), IzdelekID=1, TransakcijaID=3},
            new Narocilo{Kolicina=1, Znesek= 5.99m, Dodan=DateTime.Parse("2020-08-14"), IzdelekID=3, TransakcijaID=3}


            };
            foreach (Narocilo n in narocila)
            {
                context.Narocila.Add(n);
            }
            context.SaveChanges();


        }
    }
}