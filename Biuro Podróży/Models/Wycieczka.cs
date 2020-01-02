using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Biuro_Podróży.Models
{
    public class Wycieczka
    {
        public int Id_wycieczki { get; set; }

        [Required(ErrorMessage = "Miejsce jest wymagane!")]
        [StringLength(100)]
        public string Miejsce { get; set; }
        public DateTime Data_start { get; set; }
        public DateTime Data_end { get; set; }
        [Required(ErrorMessage = "Cena jest wymagana!")]
        public decimal Cena { get; set; }
        [Required(ErrorMessage = "Opis jest wymagany!")]
        public string Opis { get; set; }
        public int Id_jedzenia{ get; set; }
        public int Id_zakwaterowania { get; set; }
        public virtual Jedzenie Jedzenie { get; set; }
        public virtual Zakwaterowanie Zakwaterowanie { get; set; }
    }
}
