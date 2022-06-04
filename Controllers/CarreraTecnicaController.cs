using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApiKalum.Entities;

namespace WebApiKalum.Controllers
{
    [ApiController]
    [Route("v1/KalumManagement/CarrerasTecnicas")]
    public class CarreraTecnicaController : ControllerBase
    {
        private readonly KalumDbContext DbContext;
        private readonly ILogger<CarreraTecnicaController> Logger;
        public CarreraTecnicaController(KalumDbContext _dbContext, ILogger<CarreraTecnicaController> _Logger)
        {
            this.DbContext = _dbContext;
            this.Logger = _Logger;
        }
        [HttpGet]
        public async Task<ActionResult<IEnumerable<CarreraTecnica>>> Get()
        {
            List<CarreraTecnica> carrerasTecnicas = null;
            Logger.LogDebug("Iniciando proceso de consulta de carreras técnicas en la base de datos");
            carrerasTecnicas = await DbContext.CarreraTecnica.Include(c => c.Aspirantes).Include(i => i.Inscripciones).ToListAsync();
            if (carrerasTecnicas == null || carrerasTecnicas.Count == 0){
                Logger.LogWarning("No existen carreras técnicas en la base de datos");
                return new NoContentResult();
            }
            Logger.LogInformation("Se ejecutó la peticion de forma exitosa");
            return Ok(carrerasTecnicas);
        }
        
        [HttpGet("{id}", Name = "GetCarreraTecnica")]
        public async Task<ActionResult<CarreraTecnica>> GetCarreraTecnica(string id)
        {
            Logger.LogDebug("Iniciando el proceso de busca con el id: " + id);
            var carrera = await DbContext.CarreraTecnica.Include(c => c.Aspirantes).Include(c => c.Inscripciones).FirstOrDefaultAsync(ct => ct.CarreraId == id);
            if(carrera == null){
                Logger.LogWarning("No existe una carrera técnica con el id: " + id);
                return new NoContentResult();
            }
            Logger.LogInformation("Finalizando el proceso de busqueda de forma exitosa");
            return Ok(carrera);
        }
    }
}