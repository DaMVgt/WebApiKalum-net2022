using System.ComponentModel.DataAnnotations;
using WebApiKalum.Helpers;

namespace WebApiKalum.Entities
{
    public class Aspirante
    {
        [Required]
        [StringLength(12, MinimumLength = 12)]
        [NoExpediente]
        public string NoExpediente { get; set; }
        [Required]
        public string Apellidos { get; set; }
        [Required]
        public string Nombres { get; set; }
        [Required]
        public string Direccion { get; set; }
        [Required]
        public string Telefono { get; set; }
        [EmailAddress]
        public string Email { get; set; }
        public string Estatus { get; set; } = "NO ASIGNADO";
        public string CarreraId { get; set; }
        public virtual CarreraTecnica CarreraTecnica { get; set; }
        public string JornadaId { get; set; }
        public virtual Jornada Jornada { get; set; }
        public string ExamenId { get; set; }
        public virtual ExamenAdmision ExamenAdmision { get; set; }
        public virtual List<InscripcionPago> InscripcionPago { get; set; }
        public virtual List<ResultadoExamenAdmision> ResultadoExamenAdmision { get; set; }
    }
}