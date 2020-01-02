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
        [Required(ErrorMessage = "Login jest wymagany!")]
        [StringLength(50, MinimumLength = 3, ErrorMessage = "Login musi się składać z od 3 do 50 znaków")]
        public string Login { get; set; }

        [Required(ErrorMessage = "Hasło jest wymagane!")]
        [DataType("Password")]
        [StringLength(50, MinimumLength = 5, ErrorMessage = "Hasło musi się składać z od 5 do 50 znaków")]
        public string Password { get; set; }

        [NotMapped]
        [DataType("Password")]
        [StringLength(50, MinimumLength = 5,ErrorMessage ="Hasło musi się składać z od 5 do 50 znaków")]
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
