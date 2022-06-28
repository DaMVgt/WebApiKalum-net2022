using WebApiKalum.Dtos.Views;

namespace WebApiKalum.Dtos.Lists
{
    public class CargoListDTO
    {
        public string CargoId { get; set; }
        public string Descripcion { get; set; }
        public string Prefijo { get; set; }
        public decimal Monto { get; set; }
        public List<CuentaXCobrarViewDTO> CuentasXCobrar { get; set; }
    }
}