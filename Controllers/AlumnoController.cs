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
        [HttpPost]
        public async Task<ActionResult<Alumno>> PostAlumno([FromBody] Alumno value)
        {
            Logger.LogDebug("Iniciando el proceso de creacion de un nuevo alumno");
            await DbContext.Alumno.AddAsync(value);
            await DbContext.SaveChangesAsync();
            Logger.LogInformation("Finalizando el proceso de creacion de un nuevo alumno");
            return new CreatedAtRouteResult("GetAlumno", new { id = value.Carne }, value);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<Alumno>> DeleteAlumno(string id)
        {
            Logger.LogDebug("Iniciando el proceso de eliminacion de un alumno");
            Alumno alumno = await DbContext.Alumno.FirstOrDefaultAsync(a => a.Carne == id);
            if (alumno == null){
                Logger.LogWarning("No existe un alumno con el id: " + id);
                return NotFound();
            }
            else{
                DbContext.Alumno.Remove(alumno);
                await DbContext.SaveChangesAsync();
                Logger.LogInformation("Finalizando el proceso de eliminacion de un alumno");
                return alumno;
            }
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> PutAlumno(string id, [FromBody] Alumno value)
        {
            Logger.LogDebug($"Iniciando el proceso de actualizacion de un alumno con el id: {id}");
            Alumno alumno = await DbContext.Alumno.FirstOrDefaultAsync(a => a.Carne == id);
            if(alumno == null){
                Logger.LogWarning($"No existe un alumno con el id: {id}");
                return BadRequest();
            }
            alumno.Carne = value.Carne;
            alumno.Apellidos = value.Apellidos;
            alumno.Nombres = value.Nombres;
            alumno.Direccion = value.Direccion;
            alumno.Telefono = value.Telefono;
            alumno.Email = value.Email;
            DbContext.Entry(alumno).State = EntityState.Modified;
            await DbContext.SaveChangesAsync();
            Logger.LogInformation($"Finalizando el proceso de actualizacion de un alumno con el id: {id}");
            return NoContent();
        }
    }
}