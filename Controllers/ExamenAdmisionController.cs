using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApiKalum.Entities;

namespace WebApiKalum.Controller
{
    [ApiController]
    [Route("v1/KalumManagement/ExamenAdmision")]
    public class ExamenAdmisionController : ControllerBase
    {
        private readonly KalumDbContext DbContext;
        private readonly ILogger<ExamenAdmisionController> Logger;
        public ExamenAdmisionController(KalumDbContext _dbContext, ILogger<ExamenAdmisionController> _Logger)
        {
            this.DbContext = _dbContext;
            this.Logger = _Logger;
        }
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ExamenAdmision>>> Get()
        {
            List<ExamenAdmision> examenAdmision = null;
            Logger.LogDebug("Iniciando proceso de consulta en la base de datos");
            examenAdmision = await DbContext.ExamenAdmision.Include(e => e.Aspirantes).ToListAsync();
            if (examenAdmision == null || examenAdmision.Count == 0){
                Logger.LogWarning("No existen examenes en la base de datos");
                return new NoContentResult();
            }
            Logger.LogInformation("Se consultaron los examenes exitosamente");
            return Ok(examenAdmision);
        }
        [HttpGet("{id}", Name = "GetExamenAdmision")]
        public async Task<ActionResult<ExamenAdmision>> GetExamenAdmision(string id)
        {
            Logger.LogDebug("Iniciando el proceso de busca con el id: " + id);
            var examenAdmision = await DbContext.ExamenAdmision.Include(e => e.Aspirantes).FirstOrDefaultAsync(e => e.ExamenId == id);
            if (examenAdmision == null){
                Logger.LogWarning("No existe un examen con el id: " + id);
                return new NoContentResult();
            }
            Logger.LogInformation("Se ejecuto la peticion de forma exitosa");
            return Ok(examenAdmision);
        }
    }
}