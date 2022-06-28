using WebApiKalum.Dtos.Creates;

namespace WebApiKalum.Dtos.Lists
{
    public class InversionCarreraTecnicaListDTO
    {
        public string InversionId { get; set; }
        public decimal MontoInscripcion { get; set; }
        public int NumeroPagos { get; set; }
        public CarreraTecnicaCreateDTO CarreraTecnica { get; set; }
    }
}