namespace WebApiKalum.Dtos.Views
{
    public class CuentaXCobrarViewDTO 
    {
        public string Cargo { get; set; }
        public string Descripcion { get; set; }
        public DateTime FechaCargo { get; set; }
        public DateTime FechaAplica { get; set; }
        public decimal Monto { get; set; }
        public decimal Mora { get; set; }
        public decimal Descuento { get; set; }
    }
}