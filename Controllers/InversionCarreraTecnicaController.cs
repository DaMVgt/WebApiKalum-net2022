using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApiKalum.Entities;

namespace WebApiKalum.Controller
{
    [ApiController]
    [Route("v1/KalumManagement/InversionCarreraTecnicas")]
    public class InversionCarreraTecnicaController : ControllerBase
    {
        private readonly KalumDbContext DbContext;
        private readonly ILogger<InversionCarreraTecnicaController> Logger;
        public InversionCarreraTecnicaController(KalumDbContext _dbContext, ILogger<InversionCarreraTecnicaController> _Logger)
        {
            this.DbContext = _dbContext;
            this.Logger = _Logger;
        }
        [HttpGet]
        public async Task<ActionResult<IEnumerable<InversionCarreraTecnica>>> Get()
        {
            Logger.LogDebug("Iniciando proceso de consulta de inversion carrera tecnica en la base de datos");
            List<InversionCarreraTecnica> inversionCarreraTecnicas = await DbContext.InversionCarreraTecnica.Include(ict => ict.CarreraTecnica).ToListAsync();
            if (inversionCarreraTecnicas == null || inversionCarreraTecnicas.Count == 0)
            {
                Logger.LogWarning("No existen inversiones de carreras tecnicas en la base de datos");
                return new NoContentResult();
            }
            Logger.LogInformation("Se ejecuto la peticion de forma exitosa");
            return Ok(inversionCarreraTecnicas);
        }
        [HttpGet("{id}", Name = "GetInversionCarreraTecnica")]
        public async Task<ActionResult<InversionCarreraTecnica>> GetInversionCarreraTecnica(string id)
        {
            Logger.LogDebug($"Iniciando el proceso de inversion con ID: {id}");
            var inversionCarreraTecnica = await DbContext.InversionCarreraTecnica.Include(ict => ict.CarreraTecnica).FirstOrDefaultAsync(ict => ict.InversionId == id);
            if (inversionCarreraTecnica == null)
            {
                Logger.LogWarning($"No existe una inversion con el ID: {id}");
                return new NoContentResult();
            }
            Logger.LogInformation("Finalizando el proceso de busqueda de forma exitosa");
            return Ok(inversionCarreraTecnica);
        }
        [HttpPost]
        public async Task<ActionResult<InversionCarreraTecnica>> PostInversionCarreraTecnica([FromBody] InversionCarreraTecnica value)
        {
            Logger.LogDebug("Iniciando el proceso de creacion de un nuevo  cargo");
            value.InversionId = Guid.NewGuid().ToString().ToUpper();
            CarreraTecnica carreraTecnica = await DbContext.CarreraTecnica.FirstOrDefaultAsync(ct => ct.CarreraId == value.CarreraId);
            if (carreraTecnica == null)
            {
                Logger.LogInformation($"No existe una carrera tecnica con el ID {value.CarreraId}");
                return BadRequest();
            }
            await DbContext.InversionCarreraTecnica.AddAsync(value);
            await DbContext.SaveChangesAsync();
            Logger.LogInformation("Finalizando el proceso de creacion de una nuevo inversion");
            return new CreatedAtRouteResult("GetInversionCarreraTecnica", new { id = value.InversionId }, value);
        }
        [HttpDelete("{id}")]
        public async Task<ActionResult<InversionCarreraTecnica>> DeleteInversionCarreraTecnica(string id)
        {
            Logger.LogDebug("Iniciando proceso de eliminacion de una inversion de carrera tecnica");
            InversionCarreraTecnica inversionCarreraTecnica = await DbContext.InversionCarreraTecnica.FirstOrDefaultAsync(ict => ict.InversionId == id);
            if (inversionCarreraTecnica == null)
            {
                Logger.LogWarning($"No existe una inversion con el ID: {id}");
                return NotFound();
            }
            else
            {
                DbContext.InversionCarreraTecnica.Remove(inversionCarreraTecnica);
                await DbContext.SaveChangesAsync();
                Logger.LogInformation("Finalizando el proceso de eliminacion de una inversion");
                return inversionCarreraTecnica;
            }
        }
        [HttpPut("{id}")]
        public async Task<ActionResult> PutInversionCarreraTecnica(string id, [FromBody] InversionCarreraTecnica value)
        {
            Logger.LogDebug($"Iniciando el proceso de actualizacion de una inversion con ID: {id}");
            InversionCarreraTecnica inversionCarreraTecnica = await DbContext.InversionCarreraTecnica.FirstOrDefaultAsync(ict => ict.InversionId == id);
            if (inversionCarreraTecnica == null)
            {
                Logger.LogWarning($"No existe una inversion con el ID: {id}");
                return NotFound();
            }
            inversionCarreraTecnica.CarreraId = value.CarreraId;
            inversionCarreraTecnica.MontoInscripcion = value.MontoInscripcion;
            inversionCarreraTecnica.NumeroPagos = value.NumeroPagos;
            inversionCarreraTecnica.MontoPago = value.MontoPago;
            DbContext.Entry(inversionCarreraTecnica).State = EntityState.Modified;
            await DbContext.SaveChangesAsync();
            Logger.LogInformation($"Finalizando el proceso de actualizacion de una inversion con ID: {id}");
            return NoContent();
        }
    }
}