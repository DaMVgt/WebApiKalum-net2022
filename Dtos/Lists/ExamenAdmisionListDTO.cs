using WebApiKalum.Dtos.Views;

namespace WebApiKalum.Dtos.Lists
{
    public class ExamenAdmisionListDTO
    {
        public string ExamenId { get; set; }
        public DateTime FechaExamen { get; set; }
        public List<AspiranteViewDTO> Aspirantes { get; set; }
    }
}