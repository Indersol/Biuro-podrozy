using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Biuro_Podróży.Models
{
    public class Wycieczka_Klient
    {
        public int Id_zamowienia { get; set; }
        public int Id_usera { get; set; }
        public int Id_wycieczki { get; set; }
        public int Bilety { get; set; }
        public virtual User user { get; set; }
        public virtual Wycieczka wycieczka { get; set; }

    }
}
