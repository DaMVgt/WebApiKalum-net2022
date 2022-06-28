using System.ComponentModel.DataAnnotations;

namespace WebApiKalum.Dtos
{
    public class ExamenAdmisionCreateDTO
    {
        [Required]
        [DataType(DataType.Date)]
        public DateTime FechaExamen { get; set; }
    }
}