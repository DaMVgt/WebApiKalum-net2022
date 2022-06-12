using System.ComponentModel.DataAnnotations;

namespace WebApiKalum.Entities
{
    public class Alumno
    {
        [Required]
        [StringLength(8, MinimumLength = 8)]
        public string Carne { get; set; }
        [Required]
        [StringLength(128, MinimumLength = 5, ErrorMessage = "La cantidad minima de caracteres es {2} y la maxima es {1} para el campo {0}")]
        public string Apellidos { get; set; }
        [Required]
        [StringLength(128, MinimumLength = 5, ErrorMessage = "La cantidad minima de caracteres es {2} y la maxima es {1} para el campo {0}")]
        public string Nombres { get; set; }
        [Required]
        [StringLength(128, MinimumLength = 5, ErrorMessage = "La cantidad minima de caracteres es {2} y la maxima es {1} para el campo {0}")]
        public string Direccion { get; set; }
        [Required]
        [StringLength(64, MinimumLength = 8)]
        public string Telefono { get; set; }
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        public virtual List<Inscripcion> Inscripciones { get; set; }
        public virtual List<CuentaXCobrar> CuentaXCobrar { get; set; }
    }
}