using System.ComponentModel.DataAnnotations;

namespace WebApiKalum.Entities
{
    public class Inscripcion
    {
        [Required]
        public string InscripcionId { get; set; }
        [Required]
        [StringLength(8, MinimumLength = 8)]
        public string Carne { get; set; }
        [Required]
        [StringLength(128, MinimumLength = 4)]
        public string CarreraId { get; set; }
        [Required]
        [StringLength(128, MinimumLength = 4)]
        public string JornadaId { get; set; }
        [Required]
        [StringLength(4, MinimumLength = 4)]
        public string Ciclo { get; set; }
        [Required]
        [DataType(DataType.Date)]
        public DateTime FechaInscripcion { get; set; }
        public virtual Jornada Jornada { get; set; }
        public virtual CarreraTecnica CarreraTecnica { get; set; }
        public virtual Alumno Alumno { get; set; }
    }
}