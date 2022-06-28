using System.ComponentModel.DataAnnotations;

namespace WebApiKalum.Dtos.Creates
{
    public class ExamenAdmisionCreateDTO
    {
        [Required]
        [DataType(DataType.Date)]
        public DateTime FechaExamen { get; set; }
    }
}