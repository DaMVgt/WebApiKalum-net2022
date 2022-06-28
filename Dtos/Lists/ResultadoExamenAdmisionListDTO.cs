using WebApiKalum.Dtos.Views;

namespace WebApiKalum.Dtos.Lists
{
    public class ResultadoExamenAdmisionListDTO
    {
        public string NoExpediente { get; set; }
        public string Anio { get; set; }
        public string Descripcion { get; set; }
        public int Nota { get; set; }
        public AspiranteViewDTO Aspirante { get; set; }
    }
}