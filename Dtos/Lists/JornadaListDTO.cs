using WebApiKalum.Dtos.Views;

namespace WebApiKalum.Dtos.Lists
{
    public class JornadaListDTO
    {
        public string JornadaId { get; set; }
        public string Prefijo { get; set; }
        public string Descripcion { get; set; }
        public List<AspiranteViewDTO> Aspirantes { get; set; }
    }
}