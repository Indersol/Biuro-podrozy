using System.Collections.Generic;

namespace Biuro_Podróży.Models
{
    public class Zakwaterowanie
    {
        public int Id_zakwaterowania { get; set; }
        public string Opcja { get; set; }
        public decimal Cena { get; set; }
        public virtual ICollection<Wycieczka> Wycieczka { get; set; }
    }
}