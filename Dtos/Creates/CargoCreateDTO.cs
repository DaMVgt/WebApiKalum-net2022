using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace WebApiKalum.Dtos.Creates
{
    public class CargoCreateDTO
    {
        [Required]
        [StringLength(128, MinimumLength = 5, ErrorMessage = "La cantidad minima de caracteres es {2} y la maxima es {1} para el campo {0}")]
        public string Descripcion { get; set; }
        [Required]
        [StringLength(64, MinimumLength = 2, ErrorMessage = "La cantidad minima de caracteres es {2} y la maxima es {1} para el campo {0}")]
        public string Prefijo { get; set; }
        [Required]
        [Precision(10, 2)]
        public decimal Monto { get; set; }

    }
}