namespace WebApiKalum.Dtos
{
    public class JornadaListDTO
    {
        public string JornadaId { get; set; }
        public string Prefijo { get; set; }
        public string Descripcion { get; set; }
        public List<JornadaAspirantesDTO> Aspirantes { get; set; }
    }
}