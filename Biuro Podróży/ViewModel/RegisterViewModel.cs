using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Biuro_Podróży.ViewModel
{
    public class RegisterViewModel
    {
        [Required]
        [EmailAddress]
        [Remote(action:"IsEmailInUse",controller:"Account")]
        public string Email { get; set; }
        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }
        [DataType(DataType.Password)]
        [Display(Name = "Potwierdź hasło")]
        [Compare ("Password", ErrorMessage ="Hasła nie są takie same")]
        public string ConfirmPassword { get; set; }
    }
}
