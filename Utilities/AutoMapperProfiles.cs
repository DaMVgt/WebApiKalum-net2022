using AutoMapper;
using WebApiKalum.Dtos.Creates;
using WebApiKalum.Dtos.Lists;
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
            CreateMap<Inscripcion, InscripcionesViewDTO>();
            CreateMap<Aspirante, AspiranteViewDTO>().ConstructUsing(e => new AspiranteViewDTO { NombreCompleto = $"{e.Apellidos} {e.Nombres}" });
            CreateMap<Jornada, JornadaListDTO>();
            CreateMap<JornadaCreateDTO, Jornada>();
            CreateMap<Alumno, AlumnoListDTO>().ConstructUsing(e => new AlumnoListDTO { NombreCompleto = $"{e.Apellidos} {e.Nombres}" });
            CreateMap<CuentaXCobrar, CuentaXCobrarViewDTO>();
            CreateMap<ExamenAdmisionCreateDTO, ExamenAdmision>();
            CreateMap<ExamenAdmision, ExamenAdmisionListDTO>();
            CreateMap<CargoCreateDTO, Cargo>();
            CreateMap<Cargo, CargoListDTO>();
            CreateMap<ResultadoExamenAdmision, ResultadoExamenAdmisionListDTO>();
            CreateMap<Alumno, AlumnoViewDTO>().ConstructUsing(e => new AlumnoViewDTO { NombreCompleto = $"{e.Apellidos} {e.Nombres}" });
            CreateMap<Inscripcion, InscripcionesListDTO>();
            CreateMap<InversionCarreraTecnica, InversionCarreraTecnicaListDTO>();
            CreateMap<InversionCarreraTecnicaCreateDTO, InversionCarreraTecnica>();
        }
    }
}