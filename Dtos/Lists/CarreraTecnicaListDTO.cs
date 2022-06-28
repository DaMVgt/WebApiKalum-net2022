using WebApiKalum.Dtos.Views;

namespace WebApiKalum.Dtos.Lists
{
    public class CarreraTecnicaListDTO
    {
        public string CarreraId { get; set; }
        public string Nombre { get; set; }
        public List<AspiranteViewDTO> Aspirantes { get; set; }
        public List<InscripcionesViewDTO> Inscripciones { get; set; }
    }
}