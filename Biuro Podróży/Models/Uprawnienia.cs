using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Biuro_Podróży.Models
{
    public class Uprawnienia
{
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id_uprawnien { get; set; }

        public string Opcja { get; set; }
    }
}
