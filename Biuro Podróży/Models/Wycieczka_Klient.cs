using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Biuro_Podróży.Models
{
    public class Wycieczka_Klient
    {
        [Key]
        public int Id_zamowienia { get; set; }
        [ForeignKey("Wycieczka")]
        public int Id_wycieczki { get; set; }

        [Required(ErrorMessage = "Ustawienie ilości biletów jest wymagane!")]
        public int Bilety { get; set; }
//to jeszcze do poprawy 
        public virtual Wycieczka Wycieczka { get; set; }

    }
}
