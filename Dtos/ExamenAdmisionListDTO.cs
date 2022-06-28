namespace WebApiKalum.Dtos
{
    public class ExamenAdmisionListDTO
    {
        public string ExamenId { get; set; }
        public DateTime FechaExamen { get; set; }
        public List<JornadaAspirantesDTO> Aspirantes { get; set; }
    }
}