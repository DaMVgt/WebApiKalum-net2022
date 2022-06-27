namespace WebApiKalum.Dtos
{
    public class CarreraTecnicaListDTO
    {
        public string CarreraId { get; set; }
        public string Nombre { get; set; }
        public List<CarreraTecnicaAspiranteDTO> Aspirantes { get; set; }
        public List<CarreraTecnicaInscripcionesDTO> Inscripciones { get; set; }
    }
}