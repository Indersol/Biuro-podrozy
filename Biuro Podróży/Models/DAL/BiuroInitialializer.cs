﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Biuro_Podróży.Models
{
    public static class BiuroInitialializer
    {
        public static void Initialize(BiuroContext context)
        {
            if(context.User.Any())
            {
                return;
            }
            var users = new User[]
            {
                new User{Imie="Igor",Nazwisko="Zimnowodzki",Login="Admin",Password="Admin",Miejscowosc="Białystok",
                    Email ="igor.zima@o2.pl",Telefon="123456789",Uprawnienia=Uprawnienia.Admin },
                new User{Imie="Marta",Nazwisko="Owczarczuk",Login="Mod",Password="Mod",Miejscowosc="Wasilków",
                    Email ="marta@gmail.com",Telefon="987654321",Uprawnienia=Uprawnienia.Moderator },
                new User{Imie="Janusz",Nazwisko="Kowalski",Login="User",Password="User",Miejscowosc="Warszawa",
                    Email ="Janusz.Kowalski@gmail.com",Telefon="944654321",Uprawnienia=Uprawnienia.Klient },
                new User{Imie="Andrzej",Nazwisko="Nowak",Login="User2",Password="User",Miejscowosc="Bełchatów",
                    Email ="AndrzejuNieDenerwujSie@gmail.com",Telefon="433334321",Uprawnienia=Uprawnienia.Klient }
            };
            foreach (User u in users)
            {
                context.User.Add(u);
            }
            context.SaveChanges();

            var posilki = new Jedzenie[]
            {
                new Jedzenie {Nazwa = "Brak",Cena = 0, Opis="Brak posiłków" },
                new Jedzenie {Nazwa = "Tylko śniadanie",Cena = 10, Opis="Same śniadania" },
                new Jedzenie {Nazwa = "Tylko obiad",Cena = 15, Opis="Same obiady" },
                new Jedzenie {Nazwa = "Śniadanie + obiad",Cena = 25, Opis="Śniadanie oraz obiad" }
            };
            foreach (Jedzenie p in posilki)
            {
                context.Jedzenie.Add(p);
            }
            context.SaveChanges();

            var hotele = new Zakwaterowanie[]
            {
                new Zakwaterowanie {Nazwa = "Brak", Cena = 0, Opis = "Brak zakwaterowania" },
                new Zakwaterowanie {Nazwa = "Hotel 1 gwiazdka", Cena = 100, Opis = "Tani hotel" },
                new Zakwaterowanie {Nazwa = "Hotel 2 gwiazdki", Cena = 200, Opis = "Średni hotel" },
                new Zakwaterowanie {Nazwa = "Hotel 3 gwiazdki", Cena = 400, Opis = "Średni hotel" },
                new Zakwaterowanie {Nazwa = "Hotel 4 gwiazdki", Cena = 700, Opis = "Drogi hotel" },
                new Zakwaterowanie {Nazwa = "Hotel 5 gwiazdek", Cena = 1000, Opis = "Full wypas" },
            };
            foreach (Zakwaterowanie z in hotele)
            {
                context.Zakwaterowanie.Add(z);
            }
            context.SaveChanges();

            var wycieczki = new Wycieczka[]
            {
                new Wycieczka {Miejsce = "San Escobar", Data_start = DateTime.Parse("2020-01-01"), Data_end = DateTime.Parse("2020-01-05"),
                    Opis = "Zapraszamy na wycieczkę do słonecznego San Ecsobar.", Cena = 2500, Id_jedzenia = posilki.Single(p => p.Nazwa == "Brak").Id_jedzenia,
                    Id_zakwaterowania = hotele.Single(h => h.Nazwa == "Brak").Id_zakwaterowania },
                new Wycieczka {Miejsce = "San Pablo", Data_start = DateTime.Parse("2020-01-06"), Data_end = DateTime.Parse("2020-01-09"),
                    Opis = "Zapraszamy na wycieczkę do słonecznego San Pablo.", Cena = 1500, Id_jedzenia = posilki.Single(p => p.Nazwa == "Brak").Id_jedzenia,
                    Id_zakwaterowania = hotele.Single(h => h.Nazwa == "Brak").Id_zakwaterowania },
                new Wycieczka {Miejsce = "Prypyat", Data_start = DateTime.Parse("2020-01-09"), Data_end = DateTime.Parse("2020-01-11"),
                    Opis = "Zapraszamy na wycieczkę do sarkofagu", Cena = 1200, Id_jedzenia = posilki.Single(p => p.Nazwa == "Brak").Id_jedzenia,
                    Id_zakwaterowania = hotele.Single(h => h.Nazwa == "Brak").Id_zakwaterowania },

            };
            foreach (Wycieczka w in wycieczki)
            {
                context.Wycieczka.Add(w);
            }
            context.SaveChanges();

            var rezerwacje = new Wycieczka_Klient[]
            {
                new Wycieczka_Klient {Id_usera = users.Single(s => s.Login == "User").Id_usera, Id_wycieczki = wycieczki.Single(w => w.Miejsce == "San Pablo").Id_wycieczki, Bilety = 2 },
                new Wycieczka_Klient {Id_usera = users.Single(s => s.Login == "User2").Id_usera, Id_wycieczki = wycieczki.Single(w => w.Miejsce == "Prypyat").Id_wycieczki, Bilety = 4 }

            };
            foreach (Wycieczka_Klient wk in rezerwacje)
            {
                context.Wycieczka_Klient.Add(wk);
            }
            context.SaveChanges();

        }
    }
}
