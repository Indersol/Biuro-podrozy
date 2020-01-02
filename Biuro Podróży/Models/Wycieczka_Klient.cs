﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Biuro_Podróży.Models
{
    public class Wycieczka_Klient
    {
        [Key]
        public int Id_zamowienia { get; set; }
        public int Id_usera { get; set; }
        public int Id_wycieczki { get; set; }

        [Required(ErrorMessage = "Ustawienie ilości biletów jest wymagane!")]
        public int Bilety { get; set; }
        public virtual User User { get; set; }
        public virtual Wycieczka Wycieczka { get; set; }

    }
}
