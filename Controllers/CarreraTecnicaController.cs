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
            carrerasTecnicas = await DbContext.CarreraTecnica.Include(c => c.Aspirantes).Include(i => i.Inscripciones).Include( c => c.Inversiones).ToListAsync();
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
            var carrera = await DbContext.CarreraTecnica.Include(c => c.Aspirantes).Include(c => c.Inscripciones).Include(c => c.Inversiones).FirstOrDefaultAsync(ct => ct.CarreraId == id);
            if(carrera == null){
                Logger.LogWarning("No existe una carrera técnica con el id: " + id);
                return new NoContentResult();
            }
            Logger.LogInformation("Finalizando el proceso de busqueda de forma exitosa");
            return Ok(carrera);
        }

        [HttpPost]
        public async Task<ActionResult<CarreraTecnica>> PostCarreraTecnica([FromBody] CarreraTecnica value)
        {
            Logger.LogDebug("Iniciando el proceso de creacion de una nueva carrera técnica");
            value.CarreraId = Guid.NewGuid().ToString().ToUpper();
            await DbContext.CarreraTecnica.AddAsync(value);
            await DbContext.SaveChangesAsync();
            Logger.LogInformation("Finalizando el proceso de creacion de una nueva carrera técnica");
            return new CreatedAtRouteResult("GetCarreraTecnica", new { id = value.CarreraId }, value);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<CarreraTecnica>> DeleteCarreraTecnica(string id)
        {
            Logger.LogDebug("Iniciando el proceso de eliminacion de una carrera técnica");
            CarreraTecnica carreraTecnica = await DbContext.CarreraTecnica.FirstOrDefaultAsync(ct => ct.CarreraId == id);
            if(carreraTecnica == null){
                Logger.LogWarning($"No existe una carrera técnica con el id: {id}");
                return NotFound();
            }
            else{
                DbContext.CarreraTecnica.Remove(carreraTecnica);
                await DbContext.SaveChangesAsync();
                Logger.LogInformation($"Finalizando el proceso de eliminacion de una carrera técnica con el id: {id}");
                return carreraTecnica;
            }
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> PutCarreraTecnica(string id, [FromBody] CarreraTecnica value)
        {
            Logger.LogDebug($"Iniciando el proceso de actualizacion de una carrera técnica con id: {id}");
            CarreraTecnica carreraTecnica = await DbContext.CarreraTecnica.FirstOrDefaultAsync(ct => ct.CarreraId == id);
            if(carreraTecnica == null){
                Logger.LogWarning($"No existe una carrera técnica con el id: {id}");
                return BadRequest();
            }
            carreraTecnica.Nombre = value.Nombre;
            DbContext.Entry(carreraTecnica).State = EntityState.Modified;
            await DbContext.SaveChangesAsync();
            Logger.LogInformation($"Finalizando el proceso de actualizacion de una carrera técnica con id: {id}");
            return NoContent();
        }
    }
}