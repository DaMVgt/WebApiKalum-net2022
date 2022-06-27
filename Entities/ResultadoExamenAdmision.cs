using System.ComponentModel.DataAnnotations;
using WebApiKalum.Helpers;

namespace WebApiKalum.Entities
{
    public class ResultadoExamenAdmision
    {
        [Required]
        [NoExpediente]
        public string NoExpediente { get; set; }
        [Required]
        [StringLength(4, MinimumLength = 4)]
        public string Anio { get; set; }
        [Required]
        [StringLength(128, MinimumLength = 4)]
        public string Descripcion { get; set; }
        [Required]
        public int Nota { get; set; }
        public virtual Aspirante Aspirante { get; set; }
    }
}