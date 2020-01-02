using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Biuro_Podróży.Models
{
    public class User
{
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id_usera { get; set; }
        public string Login { get; set; }

        [Required(ErrorMessage = "Hasło jest wymagane!")]
        [DataType("Password")]
        public string Password { get; set; }

        [NotMapped]
        [DataType("Password")]
        [Compare("Password", ErrorMessage = "Hasła nie są takie same!")]
        public string ConfirmPassword { get; set; }

        [Required(ErrorMessage = "Należy podać adres Email!")]
        [EmailAddress(ErrorMessage = "Nie prawidłowy adres Email!")]
        public string Email { get; set; }

        public string Imie { get; set; }
        public string Nazwisko { get; set; }
        public string Miejscowosc { get; set; }
        public string Telefon { get; set; }
        public Uprawnienia Uprawnienia { get; set; }

    }
    public enum Uprawnienia
    {
        Admin,
        Moderator,
        Klient
    }
}
