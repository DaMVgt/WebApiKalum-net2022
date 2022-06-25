using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApiKalum.Dtos;
using WebApiKalum.Entities;
using WebApiKalum.Utilities;

namespace WebApiKalum.Controllers
{
    [ApiController]
    [Route("v1/KalumManagement/Aspirantes")]
    public class AspiranteController : ControllerBase
    {
        private readonly KalumDbContext DbContext;
        private readonly ILogger<AspiranteController> Logger;
        private readonly IMapper Mapper;
        public AspiranteController(KalumDbContext _dbContext, ILogger<AspiranteController> _Logger, IMapper _Mapper)
        {
            this.DbContext = _dbContext;
            this.Logger = _Logger;
            this.Mapper = _Mapper;
        }
        [HttpGet]
        [ServiceFilter(typeof(ActionFilter))]
        public async Task<ActionResult<IEnumerable<AspiranteListDTO>>> Get()
        {
            Logger.LogDebug("Iniciando proceso de consulta de aspirantes en la base de datos");
            List<Aspirante> aspirantes = await DbContext.Aspirante.Include(a => a.Jornada).Include(a => a.CarreraTecnica).Include( a => a.ExamenAdmision).ToListAsync();
            if (aspirantes == null || aspirantes.Count == 0)
            {
                Logger.LogWarning("No existen aspirantes en la base de datos");
                return new NoContentResult();
            }
            List<AspiranteListDTO> lista = Mapper.Map<List<AspiranteListDTO>>(aspirantes);
            Logger.LogInformation("Se ejecuto la preticion de forma exitosa");
            return Ok(lista);
        }
        [HttpGet("{id}", Name = "GetAspirante")]
        public async Task<ActionResult<Aspirante>> GetAspirante(string id)
        {
            Logger.LogDebug("Iniciando el proceso de busca con el id: " + id);
            var aspirante = await DbContext.Aspirante.Include(a => a.ResultadoExamenAdmision).Include(a => a.InscripcionPago).FirstOrDefaultAsync(a => a.NoExpediente == id);
            if (aspirante == null)
            {
                Logger.LogWarning("No existe un aspirante con el id: " + id);
                return new NoContentResult();
            }
            Logger.LogInformation("Se ejecuto la peticion de forma exitosa");
            return Ok(aspirante);
        }
        [HttpPost]
        public async Task<ActionResult<Aspirante>> PostAspirante([FromBody] Aspirante value)
        {
            Logger.LogDebug("Iniciando el proceso de creacion de un aspirante");
            CarreraTecnica carreraTecnica = await DbContext.CarreraTecnica.FirstOrDefaultAsync(ct => ct.CarreraId == value.CarreraId);
            if (carreraTecnica == null)
            {
                Logger.LogInformation($"No existe una carrera tecnica con el id: {value.CarreraId}");
                return BadRequest();
            }
            Jornada jornada = await DbContext.Jornada.FirstOrDefaultAsync(j => j.JornadaId == value.JornadaId);
            if (jornada == null)
            {
                Logger.LogInformation($"No existe una jornada con el id: {value.JornadaId}");
                return BadRequest();
            }
            ExamenAdmision examenAdmision = await DbContext.ExamenAdmision.FirstOrDefaultAsync(e => e.ExamenId == value.ExamenId);
            if (examenAdmision == null)
            {
                Logger.LogInformation($"No existe un examen con el id: {value.ExamenId}");
                return BadRequest();
            }
            await DbContext.Aspirante.AddAsync(value);
            await DbContext.SaveChangesAsync();
            Logger.LogInformation($"Se creo un aspirante con el id: {value.NoExpediente}");
            return Ok(value);
        }
    }
}