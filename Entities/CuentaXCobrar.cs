using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace WebApiKalum.Entities
{
    public class CuentaXCobrar
    {
        [Required]
        [StringLength(128, MinimumLength = 4)]
        public string Cargo { get; set; }
        [Required]
        [StringLength(4, MinimumLength = 4)]
        public string Anio { get; set; }
        [Required]
        [StringLength(8, MinimumLength = 8)]
        public string Carne { get; set; }
        [Required]
        [StringLength(128, MinimumLength = 4)]
        public string CargoId { get; set; }
        [Required]
        [StringLength(128, MinimumLength = 4)]
        public string Descripcion { get; set; }
        [Required]
        [DataType(DataType.Date)]
        public DateTime FechaCargo { get; set; }
        [Required]
        [DataType(DataType.Date)]
        public DateTime FechaAplica { get; set; }
        [Required]
        [Precision(10, 2)]
        public decimal Monto { get; set; }
        [Required]
        [Precision(10, 2)]
        public decimal Mora { get; set; }
        [Required]
        [Precision(10, 2)]
        public decimal Descuento { get; set; }
        public Cargo Cargos { get; set; }
        public Alumno Alumnos { get; set; }
    }
}