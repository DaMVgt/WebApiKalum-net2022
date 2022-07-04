using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApiKalum.Dtos.Creates;
using WebApiKalum.Dtos.Lists;
using WebApiKalum.Entities;
using WebApiKalum.Utilities;

namespace WebApiKalum.Controllers
{
    [ApiController]
    [Route("v1/KalumManagement/Jornadas")]
    public class JornadaController : ControllerBase
    {
        private readonly KalumDbContext DbContext;
        private readonly ILogger<JornadaController> Logger;
        private readonly IMapper Mapper;
        public JornadaController(KalumDbContext _dbContext, ILogger<JornadaController> _Logger, IMapper _Mapper)
        {
            this.DbContext = _dbContext;
            this.Logger = _Logger;
            this.Mapper = _Mapper;
        }
        [HttpGet]
        public async Task<ActionResult<IEnumerable<JornadaListDTO>>> Get()
        {
            Logger.LogDebug("Iniciando proceso de consulta de jornadas en la base de datos");
            List<Jornada> jornadas = await DbContext.Jornada.Include(j => j.Aspirantes).Include(j => j.Inscripciones).ToListAsync();
            if (jornadas == null || jornadas.Count == 0)
            {
                Logger.LogWarning("No existen jornadas en la base de datos");
                return new NoContentResult();
            }
            List<JornadaListDTO> lista = Mapper.Map<List<JornadaListDTO>>(jornadas);
            Logger.LogInformation("Se ejecuto la peticion de forma exitosa");
            return Ok(lista);
        }
        [HttpGet("page/{page}")]
        public async Task<ActionResult<IEnumerable<JornadaListDTO>>> GetPaginacion(int page)
        {
            var queryable = await DbContext.Jornada.Include(j => j.Aspirantes).Include(j => j.Inscripciones).ToListAsync();
            var lista = Mapper.Map<List<JornadaListDTO>>(queryable).AsQueryable();
            var paginacion = new HttpResponsePaginacion<JornadaListDTO>(lista, page);
            if (paginacion.Content == null && paginacion.Content.Count == 0)
            {
                Logger.LogWarning("No existen jornadas en la base de datos");
                return NoContent();
            }
            else
            {
                Logger.LogInformation("Se ejecuto la peticion de forma exitosa");
                return Ok(paginacion);
            }
        }
        [HttpGet("{id}", Name = "GetJornada")]
        public async Task<ActionResult<JornadaListDTO>> GetJornada(string id)
        {
            Logger.LogDebug("Iniciando el proceso de busca con el id: " + id);
            var jornada = await DbContext.Jornada.Include(j => j.Aspirantes).Include(j => j.Inscripciones).FirstOrDefaultAsync(j => j.JornadaId == id);
            if (jornada == null)
            {
                Logger.LogWarning("No existe una jornada con el id: " + id);
                return new NoContentResult();
            }
            var lista = Mapper.Map<JornadaListDTO>(jornada);
            Logger.LogInformation("Finalizando el proceso de busqueda de forma exitosa");
            return Ok(lista);
        }
        [HttpPost]
        public async Task<ActionResult<Jornada>> PostJornda([FromBody] JornadaCreateDTO value)
        {
            Logger.LogDebug("Iniciando el proceso de creacion de una jornada");
            Jornada nuevo = Mapper.Map<Jornada>(value);
            nuevo.JornadaId = Guid.NewGuid().ToString().ToUpper();
            await DbContext.Jornada.AddAsync(nuevo);
            await DbContext.SaveChangesAsync();
            Logger.LogInformation("Se ha creado una jornada con el id: ");
            return new CreatedAtRouteResult("GetJornada", new { id = nuevo.JornadaId }, value);
        }
        [HttpDelete("{id}")]
        public async Task<ActionResult<Jornada>> DeleteJornada(string id)
        {
            Logger.LogDebug("Iniciando el proceso de eliminacion de una jornada");
            Jornada jornada = await DbContext.Jornada.FirstOrDefaultAsync(j => j.JornadaId == id);
            if (jornada == null)
            {
                Logger.LogWarning($"No existe una jornada con el id: {id}");
                return NotFound();
            }
            else
            {
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
            if (jornada == null)
            {
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