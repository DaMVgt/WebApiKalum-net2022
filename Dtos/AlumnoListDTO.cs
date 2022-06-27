namespace WebApiKalum.Dtos
{
    public class AlumnoListDTO
    {
        public string Carne { get; set; }
        public string NombreCompleto { get; set; }
        public string Direccion { get; set; }
        public string Telefono { get; set; }
        public string Email { get; set; }
        public List<CarreraTecnicaInscripcionesDTO> Inscripciones { get; set; }
        public List<AlumnoCuentaXCobrarDTO> CuentaXCobrar { get; set; }
    }
}