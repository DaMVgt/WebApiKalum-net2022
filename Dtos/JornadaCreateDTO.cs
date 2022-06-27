using System.ComponentModel.DataAnnotations;

namespace WebApiKalum.Dtos
{
    public class JornadaCreateDTO
    {
        [Required]
        [StringLength(2, MinimumLength = 1, ErrorMessage = "La cantidad minima de caracteres es {2} y la maxima es {1} para el campo {0}")]
        public string Prefijo { get; set; }
        [Required]
        [StringLength(128, MinimumLength = 5, ErrorMessage = "La cantidad minima de caracteres es {2} y la maxima es {1} para el campo {0}")]
        public string Descripcion { get; set; }
    }
}