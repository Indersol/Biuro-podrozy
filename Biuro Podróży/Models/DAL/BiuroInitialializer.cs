using System;
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
        }
    }
}
