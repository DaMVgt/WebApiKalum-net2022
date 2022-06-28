using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace WebApiKalum.Dtos.Creates
{
    public class InversionCarreraTecnicaCreateDTO
    {

        [Required]
        public string CarreraId { get; set; }
        [Required]
        [Precision(10, 2)]
        public decimal MontoInscripcion { get; set; }
        [Required]
        public int NumeroPagos { get; set; }
        [Required]
        [Precision(10, 2)]
        public decimal MontoPago { get; set; }
    }
}