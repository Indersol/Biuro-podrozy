using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Biuro_Podróży.Models
{
    public class Zakwaterowanie
    {
        [Key]
        public int Id_zakwaterowania { get; set; }
        [Required(ErrorMessage = "Nazwa opcji jest wymagana!")]
        [StringLength(50)]
        public string Nazwa { get; set; }
        public virtual ICollection<Wycieczka> Wycieczka { get; set; }
    }
}