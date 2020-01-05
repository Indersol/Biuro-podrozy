using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Biuro_Podróży.Models
{
    public class Jedzenie
    {

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id_jedzenia { get; set; }
        [Required(ErrorMessage = "Nazwa opcji jest wymagana!")]
        [StringLength(50)]
        public string Nazwa { get; set; }
        public virtual ICollection<Wycieczka> Wycieczka { get; set; }
    }
}