using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApiKalum.Dtos.Creates;
using WebApiKalum.Dtos.Lists;
using WebApiKalum.Entities;

namespace WebApiKalum.Controller
{
    [ApiController]
    [Route("v1/KalumManagement/ExamenAdmision")]
    public class ExamenAdmisionController : ControllerBase
    {
        private readonly KalumDbContext DbContext;
        private readonly ILogger<ExamenAdmisionController> Logger;
        private readonly IMapper Mapper;
        public ExamenAdmisionController(KalumDbContext _dbContext, ILogger<ExamenAdmisionController> _Logger, IMapper _Mapper)
        {
            this.DbContext = _dbContext;
            this.Logger = _Logger;
            this.Mapper = _Mapper;
        }
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ExamenAdmisionListDTO>>> Get()
        {
            Logger.LogDebug("Iniciando proceso de consulta en la base de datos");
            List<ExamenAdmision> examenAdmision = await DbContext.ExamenAdmision.Include(e => e.Aspirantes).ToListAsync();
            if (examenAdmision == null || examenAdmision.Count == 0)
            {
                Logger.LogWarning("No existen examenes en la base de datos");
                return new NoContentResult();
            }
            List<ExamenAdmisionListDTO> lista = Mapper.Map<List<ExamenAdmisionListDTO>>(examenAdmision);
            Logger.LogInformation("Se consultaron los examenes exitosamente");
            return Ok(lista);
        }
        [HttpGet("{id}", Name = "GetExamenAdmision")]
        public async Task<ActionResult<ExamenAdmision>> GetExamenAdmision(string id)
        {
            Logger.LogDebug("Iniciando el proceso de busca con el id: " + id);
            var examenAdmision = await DbContext.ExamenAdmision.Include(e => e.Aspirantes).FirstOrDefaultAsync(e => e.ExamenId == id);
            if (examenAdmision == null)
            {
                Logger.LogWarning("No existe un examen con el id: " + id);
                return new NoContentResult();
            }
            var lista = Mapper.Map<ExamenAdmisionListDTO>(examenAdmision);
            Logger.LogInformation("Se ejecuto la peticion de forma exitosa");
            return Ok(lista);
        }

        [HttpPost]
        public async Task<ActionResult<ExamenAdmision>> PostExamenAdmision([FromBody] ExamenAdmisionCreateDTO value)
        {
            Logger.LogDebug("Iniciando el proceso de creacion de un examen");
            ExamenAdmision nuevo = Mapper.Map<ExamenAdmision>(value);
            nuevo.ExamenId = Guid.NewGuid().ToString().ToUpper();
            await DbContext.ExamenAdmision.AddAsync(nuevo);
            await DbContext.SaveChangesAsync();
            Logger.LogInformation("Se creo el examen exitosamente");
            return new CreatedAtRouteResult("GetExamenAdmision", new { id = nuevo.ExamenId }, nuevo);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<ExamenAdmision>> DeleteExamenAdmision(string id)
        {
            Logger.LogDebug("Iniciando el proceso de eliminacion de un examen");
            ExamenAdmision examenAdmision = await DbContext.ExamenAdmision.FirstOrDefaultAsync(e => e.ExamenId == id);
            if (examenAdmision == null)
            {
                Logger.LogWarning($"No existe un examen con el id: {id}");
                return NotFound();
            }
            else
            {
                DbContext.ExamenAdmision.Remove(examenAdmision);
                await DbContext.SaveChangesAsync();
                Logger.LogInformation($"Se elimino el examen con el id: {id}");
                return examenAdmision;
            }
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> PutExamenAdmision(string id, [FromBody] ExamenAdmision value)
        {
            Logger.LogDebug($"Iniciando el proceso de actualizacion de un examen con el id: {id}");
            ExamenAdmision examenAdmision = await DbContext.ExamenAdmision.FirstOrDefaultAsync(e => e.ExamenId == id);
            if (examenAdmision == null)
            {
                Logger.LogWarning($"No existe un examen con el id: {id}");
                return BadRequest();
            }
            examenAdmision.FechaExamen = value.FechaExamen;
            DbContext.Entry(examenAdmision).State = EntityState.Modified;
            await DbContext.SaveChangesAsync();
            Logger.LogInformation($"Se actualizo el examen con el id: {id}");
            return NoContent();
        }
    }
}