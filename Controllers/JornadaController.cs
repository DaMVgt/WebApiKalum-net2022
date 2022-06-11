using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApiKalum.Entities;

namespace WebApiKalum.Controllers
{
    [ApiController]
    [Route("v1/KalumManagement/Jornadas")]
    public class JornadaController : ControllerBase
    {
        private readonly KalumDbContext DbContext;
        private readonly ILogger<JornadaController> Logger;
        public JornadaController(KalumDbContext _dbContext, ILogger<JornadaController> _Logger)
        {
            this.DbContext = _dbContext;
            this.Logger = _Logger;
        }
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Jornada>>> Get()
        {
            List<Jornada> jornadas = null;
            Logger.LogDebug("Iniciando proceso de consulta de jornadas en la base de datos");
            jornadas = await DbContext.Jornada.Include(j => j.Aspirantes).Include(j => j.Inscripciones).ToListAsync();
            if (jornadas == null || jornadas.Count == 0){
                Logger.LogWarning("No existen jornadas en la base de datos");
                return new NoContentResult();
            }
            return Ok(jornadas);
        }
        [HttpGet("{id}", Name = "GetJornada")]
        public async Task<ActionResult<Jornada>> GetJornada(string id)
        {
            Logger.LogDebug("Iniciando el proceso de busca con el id: " + id);
            var jornada = await DbContext.Jornada.Include(j => j.Aspirantes).Include(j => j.Inscripciones).FirstOrDefaultAsync(j => j.JornadaId == id);
            if (jornada == null){
                Logger.LogWarning("No existe una jornada con el id: " + id);
                return new NoContentResult();
            }
            return Ok(jornada);
        }
        [HttpPost]
        public async Task<ActionResult<Jornada>> PostJornda([FromBody] Jornada value)
        {
            Logger.LogDebug("Iniciando el proceso de creacion de una jornada");
            value.JornadaId = Guid.NewGuid().ToString().ToUpper();
            await DbContext.Jornada.AddAsync(value);
            await DbContext.SaveChangesAsync();
            Logger.LogInformation("Se ha creado una jornada con el id: ");
            return new CreatedAtRouteResult("GetJornada", new { id = value.JornadaId }, value);
        }
        [HttpDelete("{id}")]
        public async Task<ActionResult<Jornada>> DeleteJornada(string id)
        {
            Logger.LogDebug("Iniciando el proceso de eliminacion de una jornada");
            Jornada jornada = await DbContext.Jornada.FirstOrDefaultAsync(j => j.JornadaId == id);
            if (jornada == null){
                Logger.LogWarning($"No existe una jornada con el id: {id}");
                return NotFound();
            }
            else{
                DbContext.Jornada.Remove(jornada);
                await DbContext.SaveChangesAsync();
                Logger.LogInformation($"Se ha eliminado la jornada con el id: {id}");
                return jornada;
            }
        }
        [HttpPut("{id}")]
        public async Task<ActionResult> PutJornada(string id, [FromBody] Jornada value)
        {
            Logger.LogDebug($"Iniciando el proceso de actualizacion de la jornada con el id: {id}");
            Jornada jornada = await DbContext.Jornada.FirstOrDefaultAsync(j => j.JornadaId == id);
            if(jornada == null){
                Logger.LogWarning($"No existe una jornada con el id: {id}");
                return BadRequest();
            }
            jornada.Prefijo = value.Prefijo;
            jornada.Descripcion = value.Descripcion;
            await DbContext.SaveChangesAsync();
            Logger.LogInformation($"Se ha actualizado la jornada con el id: {id}");
            return NoContent();
        }
    }
}