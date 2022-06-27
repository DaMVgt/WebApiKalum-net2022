using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApiKalum.Entities;

namespace WebApiKalum.Controller
{
    [ApiController]
    [Route("v1/KalumManagement/InscripcionPagos")]
    public class InscripcionPagoController : ControllerBase
    {
        private readonly KalumDbContext DbContext;
        private readonly ILogger<InscripcionPagoController> Logger;
        public InscripcionPagoController(KalumDbContext _dbContext, ILogger<InscripcionPagoController> _Logger)
        {
            this.DbContext = _dbContext;
            this.Logger = _Logger;
        }
        [HttpGet]
        public async Task<ActionResult<IEnumerable<InscripcionPago>>> Get()
        {
            Logger.LogDebug("Iniciando el proceso de consulta de pagos de inscripciones en la base de datos");
            List<InscripcionPago> inscripcionPagos = await DbContext.InscripcionPago.ToListAsync();
            if (inscripcionPagos == null || inscripcionPagos.Count == 0)
            {
                Logger.LogWarning("No existen cargos en la base de datos");
                return new NoContentResult();
            }
            Logger.LogInformation("Se ejecuto la peticion de forma exitosa");
            return Ok(inscripcionPagos);
        }
        [HttpGet("{id}", Name = "GetInscripcionPago")]
        public async Task<ActionResult<InscripcionPago>> GetInscripcionPago(string id)
        {
            Logger.LogDebug($"Iniciando el proceso de busqueda con el id: {id}");
            var inscripcionPago = await DbContext.InscripcionPago.FirstOrDefaultAsync(ip => ip.BoletaPago == id);
            if (inscripcionPago == null)
            {
                Logger.LogWarning($"No existe una boleta de pago con el id {id}");
                return new NoContentResult();
            }
            Logger.LogInformation("Finalizando el procesoo de busqueda de forma exitosa");
            return Ok(inscripcionPago);
        }
        [HttpPost]
        public async Task<ActionResult<InscripcionPago>> PostInscripcionPago([FromBody] InscripcionPago value)
        {
            Logger.LogDebug("Iniciando el proceso de creacion de un pago de inscripcion");
            value.BoletaPago = Guid.NewGuid().ToString().ToUpper();
            Aspirante aspirante = await DbContext.Aspirante.FirstOrDefaultAsync(a => a.NoExpediente == value.NoExpediente);
            if (aspirante == null)
            {
                Logger.LogInformation($"No existe un aspirante con el id {value.NoExpediente}");
                return BadRequest();
            }
            await DbContext.InscripcionPago.AddAsync(value);
            await DbContext.SaveChangesAsync();
            Logger.LogInformation($"Se creo un pago de inscripcion con el Id: {value.BoletaPago}");
            return Ok(value);
        }
        [HttpDelete("{id}")]
        public async Task<ActionResult<InscripcionPago>> DeleteInscripcionPago(string id)
        {
            Logger.LogDebug("Iniciando el proceso de eliminacion de un cargo");
            InscripcionPago inscripcionPago = await DbContext.InscripcionPago.FirstOrDefaultAsync(ip => ip.BoletaPago == id);
            if (inscripcionPago == null)
            {
                Logger.LogWarning($"No existe un cargo con el id: {id}");
                return NotFound();
            }
            else
            {
                DbContext.InscripcionPago.Remove(inscripcionPago);
                await DbContext.SaveChangesAsync();
                Logger.LogInformation("Finalizando el proceso de eliminacion de un pago de inscripcion");
                return inscripcionPago;
            }
        }
        [HttpPut("{id}")]
        public async Task<ActionResult> PutInscripcionPago(string id, [FromBody] InscripcionPago value)
        {
            Logger.LogDebug($"Iniciando el proceso de actualizacion de un pago de boleta: {id}");
            InscripcionPago inscripcionPago = await DbContext.InscripcionPago.FirstOrDefaultAsync(ip => ip.BoletaPago == id);
            if (inscripcionPago == null)
            {
                Logger.LogWarning($"No existe un pago con boleta {id}");
                return NotFound();
            }
            inscripcionPago.Anio = value.Anio;
            inscripcionPago.FechaPago = value.FechaPago;
            inscripcionPago.Monto = value.Monto;
            DbContext.Entry(inscripcionPago).State = EntityState.Modified;
            await DbContext.SaveChangesAsync();
            Logger.LogInformation($"Finalizando el proceso de modificacion de un pago con boleta: {id}");
            return NoContent();
        }
    }
}