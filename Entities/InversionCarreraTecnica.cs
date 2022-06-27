using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace WebApiKalum.Entities
{
    public class InversionCarreraTecnica
    {
        [Required]
        public string InversionId { get; set; }
        [Required]
        public string CarreraId { get; set; }
        [Required]
        [Precision(10,2)]
        public decimal MontoInscripcion { get; set; }
        [Required]
        public int NumeroPagos { get; set; }
        [Required]
        [Precision(10,2)]
        public decimal MontoPago { get; set; }
        public CarreraTecnica CarreraTecnica { get; set; }
    }
}