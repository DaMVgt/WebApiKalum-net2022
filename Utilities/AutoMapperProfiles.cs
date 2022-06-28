using AutoMapper;
using WebApiKalum.Dtos;
using WebApiKalum.Dtos.Views;
using WebApiKalum.Entities;

namespace WebApiKalum.Utilities
{
    public class AutoMapperProfiles : Profile
    {
        public AutoMapperProfiles()
        {
            CreateMap<CarreraTecnicaCreateDTO, CarreraTecnica>();
            CreateMap<CarreraTecnica, CarreraTecnicaCreateDTO>();
            CreateMap<Jornada, JornadaCreateDTO>();
            CreateMap<ExamenAdmision, ExamenAdmisionCreateDTO>();
            CreateMap<Aspirante, AspiranteListDTO>().ConstructUsing(e => new AspiranteListDTO { NombreCompleto = $"{e.Apellidos} {e.Nombres}" });
            CreateMap<CarreraTecnica, CarreraTecnicaListDTO>();
            CreateMap<Inscripcion, CarreraTecnicaInscripcionesDTO>();
            CreateMap<Aspirante, CarreraTecnicaAspiranteDTO>().ConstructUsing(e => new CarreraTecnicaAspiranteDTO { NombreCompleto = $"{e.Apellidos} {e.Nombres}" });
            CreateMap<Jornada, JornadaListDTO>();
            CreateMap<JornadaCreateDTO, Jornada>();
            CreateMap<Aspirante, JornadaAspirantesDTO>().ConstructUsing(e => new JornadaAspirantesDTO { NombreCompleto = $"{e.Apellidos} {e.Nombres} " });
            CreateMap<Alumno, AlumnoListDTO>().ConstructUsing(e => new AlumnoListDTO { NombreCompleto = $"{e.Apellidos} {e.Nombres}" });
            CreateMap<CuentaXCobrar, AlumnoCuentaXCobrarDTO>();
            CreateMap<ExamenAdmisionCreateDTO, ExamenAdmision>();
            CreateMap<ExamenAdmision, ExamenAdmisionListDTO>();
            CreateMap<CargoCreateDTO, Cargo>();
            CreateMap<Cargo, CargoListDTO>();
            CreateMap<ResultadoExamenAdmision, ResultadoExamenAdmisionListDTO>();
            CreateMap<Alumno, AlumnoViewDTO>().ConstructUsing(e => new AlumnoViewDTO { NombreCompleto = $"{e.Apellidos} {e.Nombres}" });
            CreateMap<Inscripcion, InscripcionesListDTO>();
        }
    }
}