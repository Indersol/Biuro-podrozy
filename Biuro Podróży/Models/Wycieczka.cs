using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Biuro_Podróży.Models
{
    public class Wycieczka
    {
        [Key]
        public int Id_wycieczki { get; set; }

        [Required(ErrorMessage = "Miejsce jest wymagane!")]
        [StringLength(100)]
        public string Miejsce { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime Data_start { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime Data_end { get; set; }
        [Required(ErrorMessage = "Cena jest wymagana!")]
        public decimal Cena { get; set; }
        [Required(ErrorMessage = "Opis jest wymagany!")]
        public string Opis { get; set; }
        public byte[] Image { get; set; }
        [ForeignKey("Jedzenie")]
        public int Id_jedzenia { get; set; }
        [ForeignKey("Zakwaterowanie")]
        public int Id_zakwaterowania { get; set; }
        public virtual Jedzenie Jedzenie { get; set; }
        public virtual Zakwaterowanie Zakwaterowanie { get; set; }

    }
    public class AllowedExtensionsAttribute : ValidationAttribute
    {
        private readonly string[] _Extensions;
        public AllowedExtensionsAttribute(string[] Extensions)
        {
            _Extensions = Extensions;
        }
        protected override ValidationResult IsValid(
        object value, ValidationContext validationContext)
        {
            var file = value as String;
            var extension = Path.GetExtension(file);
            if (!(file == null))
            {
                if (!_Extensions.Contains(extension.ToLower()))
                {
                    return new ValidationResult(GetErrorMessage());
                }
            }
            return ValidationResult.Success;
        }

        public string GetErrorMessage()
        {
            return $"Błąd!";
        }
    }
}
