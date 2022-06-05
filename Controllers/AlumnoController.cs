using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApiKalum.Entities;

namespace WebApiKalum.Controllers
{
    [ApiController]
    [Route("v1/KalumManagement/Alumnos")]
    public class AlumnoController : ControllerBase
    {
        private readonly KalumDbContext DbContext;
        private readonly ILogger<AlumnoController> Logger;
        public AlumnoController(KalumDbContext _dbContext, ILogger<AlumnoController> _Logger)
        {
            this.DbContext = _dbContext;
            this.Logger = _Logger;
        }
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Alumno>>> Get()
        {
            List<Alumno> alumnos = null;
            Logger.LogDebug("Iniciando proceso de consulta de alumnos en la base de datos");
            alumnos = await DbContext.Alumno.Include(a => a.Inscripciones).Include(a => a.CuentaXCobrar).ToListAsync();
            if (alumnos == null || alumnos.Count == 0){
                Logger.LogWarning("No existen alumnos en la base de datos");
                return new NoContentResult();
            }
            Logger.LogInformation("Se ejecuto la peticion de forma exitosa");
            return Ok(alumnos);
        }
        [HttpGet("{id}", Name = "GetAlumno")]
        public async Task<ActionResult<Alumno>> GetAlumno(string id)
        {
            Logger.LogDebug("Iniciando el proceso de busca con el id: " + id);
            var alumno = await DbContext.Alumno.Include(a => a.Inscripciones).Include(a => a.CuentaXCobrar).FirstOrDefaultAsync(a => a.Carne == id);
            if (alumno == null){
                Logger.LogWarning("No existe un alumno con el id: " + id);
                return new NoContentResult();
            }
            Logger.LogInformation("Se ejecuto la peticion de forma exitosa");
            return Ok(alumno);
        }
    }
}