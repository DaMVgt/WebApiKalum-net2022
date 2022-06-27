using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;
using WebApiKalum.Helpers;

namespace WebApiKalum.Entities
{
    public class InscripcionPago
    {
        [Required]
        public string BoletaPago { get; set; }
        [Required]
        [NoExpediente]
        public string NoExpediente { get; set; }
        [Required]
        [StringLength(4, MinimumLength = 4)]
        public string Anio { get; set; }
        [Required]
        [DataType(DataType.Date)]
        public DateTime FechaPago { get; set; }
        [Required]
        [Precision(10,2)]
        public decimal Monto { get; set; }
        public virtual Aspirante Aspirante { get; set; }
    }
}