using WebApiKalum.Dtos.Creates;
using WebApiKalum.Dtos.Views;

namespace WebApiKalum.Dtos.Lists
{
    public class InscripcionesListDTO
    {
        public string InscripcionId { get; set; }
        public string Carne { get; set; }
        public string Ciclo { get; set; }
        public DateTime FechaInscripcion { get; set; }
        public JornadaCreateDTO Jornada { get; set; }
        public CarreraTecnicaCreateDTO CarreraTecnica { get; set; }
        public AlumnoViewDTO Alumno { get; set; }
    }
}