using System.ComponentModel.DataAnnotations;

namespace WebApiKalum.Entities
{
    public class CarreraTecnica
    {
        [Required]
        public string CarreraId { get; set; }
        [Required]
        [StringLength(128, MinimumLength = 5, ErrorMessage = "La cantidad minima de caracteres es {2} y la maxima es {1} para el campo {0}")]
        public string Nombre { get; set; }
        public virtual List<Aspirante> Aspirantes { get; set; }
        public virtual List<Inscripcion> Inscripciones { get; set; }
        public virtual List<InversionCarreraTecnica> Inversiones { get; set; }
    }
}