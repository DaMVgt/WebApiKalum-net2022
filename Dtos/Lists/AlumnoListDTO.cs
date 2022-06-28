using WebApiKalum.Dtos.Views;

namespace WebApiKalum.Dtos.Lists
{
    public class AlumnoListDTO
    {
        public string Carne { get; set; }
        public string NombreCompleto { get; set; }
        public string Direccion { get; set; }
        public string Telefono { get; set; }
        public string Email { get; set; }
        public List<InscripcionesViewDTO> Inscripciones { get; set; }
        public List<CuentaXCobrarViewDTO> CuentaXCobrar { get; set; }
    }
}