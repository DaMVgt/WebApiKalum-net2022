using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApiKalum.Dtos;
using WebApiKalum.Entities;

namespace WebApiKalum.Controller
{
    [ApiController]
    [Route("v1/KalumManagement/ResultadoExamenAdmision")]
    public class ResultadoExamenAdmisionController : ControllerBase
    {
        private readonly KalumDbContext DbContext;
        private readonly ILogger<ResultadoExamenAdmisionController> Logger;
        private readonly IMapper Mapper;
        public ResultadoExamenAdmisionController(KalumDbContext _dbContext, ILogger<ResultadoExamenAdmisionController> _Logger, IMapper _Mapper)
        {
            this.DbContext = _dbContext;
            this.Logger = _Logger;
            this.Mapper = _Mapper;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ResultadoExamenAdmisionListDTO>>> Get()
        {
            Logger.LogDebug("Iniciando el proceso de consulta de resultados de examenes en la base de datos");
            List<ResultadoExamenAdmision> resultadoExamenAdmisions = await DbContext.ResultadoExamenAdmision.Include(rea => rea.Aspirante).ToListAsync();
            if (resultadoExamenAdmisions == null || resultadoExamenAdmisions.Count == 0)
            {
                Logger.LogWarning("No existen resultados de examenes en la base de datos");
                return new NoContentResult();
            }
            List<ResultadoExamenAdmisionListDTO> lista = Mapper.Map<List<ResultadoExamenAdmisionListDTO>>(resultadoExamenAdmisions);
            Logger.LogInformation("Se ejecuto la peticion de forma exitosa");
            return Ok(lista);
        }
        [HttpGet("{id}", Name = "GetResultadoExamenAdmision")]
        public async Task<ActionResult<ResultadoExamenAdmisionListDTO>> GetResultadoExamenAdmision(string id)
        {
            Logger.LogDebug("Iniciando el proceso de busqueda de resultado con ");
            var resultadoExamenAdmisions = await DbContext.ResultadoExamenAdmision.Include(rea => rea.Aspirante).FirstOrDefaultAsync(rea => rea.NoExpediente == id);
            if (resultadoExamenAdmisions == null)
            {
                Logger.LogWarning($"No existe ningun resultado con expediente {id}");
                return new NoContentResult();
            }
            var lista = Mapper.Map<ResultadoExamenAdmisionListDTO>(resultadoExamenAdmisions);
            Logger.LogInformation("Finalizando el proceso de busqueda de forma exitosa");
            return Ok(lista);
        }
        [HttpPost]
        public async Task<ActionResult<ResultadoExamenAdmision>> PostResultadoExamenAdmision([FromBody] ResultadoExamenAdmision value)
        {
            Logger.LogDebug("Iniciando el proceso de creacion de un nuevo resultado");
            Aspirante aspirante = await DbContext.Aspirante.FirstOrDefaultAsync(a => a.NoExpediente == value.NoExpediente);
            if (aspirante == null)
            {
                Logger.LogInformation($"No existe un aspirante con el id: {value.NoExpediente}");
                return BadRequest();
            }
            await DbContext.ResultadoExamenAdmision.AddAsync(value);
            await DbContext.SaveChangesAsync();
            Logger.LogInformation("Finalizando el proceso de creacion de un nuevo resultado");
            return new CreatedAtRouteResult("GetResultadoExamenAdmision", new { id = value.NoExpediente }, value);
        }
        [HttpDelete("{id}")]
        public async Task<ActionResult<ResultadoExamenAdmision>> DeleteResultadoExamenAdmision(string id)
        {
            Logger.LogDebug("Iniciando el proceso de eliminacion de un cargo");
            ResultadoExamenAdmision resultadoExamenAdmision = await DbContext.ResultadoExamenAdmision.FirstOrDefaultAsync(rea => rea.NoExpediente == id);
            if (resultadoExamenAdmision == null)
            {
                Logger.LogWarning($"No existe un resultado con el id {id}");
                return NotFound();
            }
            else
            {
                DbContext.ResultadoExamenAdmision.Remove(resultadoExamenAdmision);
                await DbContext.SaveChangesAsync();
                Logger.LogInformation("Finalizando el proceso de eliminacion de un resultado");
                return resultadoExamenAdmision;
            }
        }
        [HttpPut("{id}")]
        public async Task<ActionResult> PutResultadoExamenAdmision(string id, [FromBody] ResultadoExamenAdmision value)
        {
            Logger.LogDebug($"Iniciando el proceso de actualizacion de un resultado con el id: {id}");
            ResultadoExamenAdmision resultadoExamenAdmision = await DbContext.ResultadoExamenAdmision.FirstOrDefaultAsync(rea => rea.NoExpediente == id);
            if (resultadoExamenAdmision == null)
            {
                Logger.LogWarning($"No existe un resultado con el id: {id}");
                return NotFound();
            }
            resultadoExamenAdmision.Descripcion = value.Descripcion;
            resultadoExamenAdmision.Nota = value.Nota;
            DbContext.Entry(resultadoExamenAdmision).State = EntityState.Modified;
            await DbContext.SaveChangesAsync();
            Logger.LogInformation($"Finalizando el proceso de actualizacion de una resultado con el id: {id}");
            return NoContent();
        }
    }
}