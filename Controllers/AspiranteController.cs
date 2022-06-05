using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApiKalum.Entities;

namespace WebApiKalum.Controllers
{
    [ApiController]
    [Route("v1/KalumManagement/Aspirantes")]
    public class AspiranteController : ControllerBase
    {
        private readonly KalumDbContext DbContext;
        private readonly ILogger<AspiranteController> Logger;
        public AspiranteController(KalumDbContext _dbContext, ILogger<AspiranteController> _Logger)
        {
            this.DbContext = _dbContext;
            this.Logger = _Logger;
        }
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Aspirante>>> Get()
        {
            List<Aspirante> aspirantes = null;
            Logger.LogDebug("Iniciando proceso de consulta de aspirantes en la base de datos");
            aspirantes = await DbContext.Aspirante.Include(a => a.ResultadoExamenAdmision).Include(a => a.InscripcionPago).ToListAsync();
            if (aspirantes == null || aspirantes.Count == 0){
                Logger.LogWarning("No existen aspirantes en la base de datos");
                return new NoContentResult();
            }
            Logger.LogInformation("Se ejecuto la preticion de forma exitosa");
            return Ok(aspirantes);
        }
        [HttpGet("{id}", Name = "GetAspirante")]
        public async Task<ActionResult<Aspirante>> GetAspirante(string id)
        {
            Logger.LogDebug("Iniciando el proceso de busca con el id: " + id);
            var aspirante = await DbContext.Aspirante.Include(a => a.ResultadoExamenAdmision).Include(a => a.InscripcionPago).FirstOrDefaultAsync(a => a.NoExpediente == id);
            if (aspirante == null){
                Logger.LogWarning("No existe un aspirante con el id: " + id);
                return new NoContentResult();
            }
            Logger.LogInformation("Se ejecuto la peticion de forma exitosa");
            return Ok(aspirante);
        }
    }
}