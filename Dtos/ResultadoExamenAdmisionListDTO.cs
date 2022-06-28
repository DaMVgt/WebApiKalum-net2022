namespace WebApiKalum.Dtos
{
    public class ResultadoExamenAdmisionListDTO
    {
        public string Anio { get; set; }
        public string Descripcion { get; set; }
        public int Nota { get; set; }
        public JornadaAspirantesDTO Aspirante { get; set; }
    }
}