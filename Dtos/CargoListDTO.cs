namespace WebApiKalum.Dtos
{
    public class CargoListDTO
    {
        public string CargoId { get; set; }
        public string Descripcion { get; set; }
        public string Prefijo { get; set; }
        public decimal Monto { get; set; }
        public List<AlumnoCuentaXCobrarDTO> CuentasXCobrar { get; set; }
    }
}