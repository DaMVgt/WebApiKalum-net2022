using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApiKalum.Entities;

namespace WebApiKalum.Controller
{
    [ApiController]
    [Route("v1/KalumManagement/CuentaXCobrar")]
    public class CuentaXCobrarController : ControllerBase
    {
        private readonly KalumDbContext DbContext;
        private readonly ILogger<CuentaXCobrarController> Logger;
        public CuentaXCobrarController(KalumDbContext _dbContext, ILogger<CuentaXCobrarController> _Logger)
        {
            this.DbContext = _dbContext;
            this.Logger = _Logger;
        }
        [HttpGet]
        public async Task<ActionResult<IEnumerable<CuentaXCobrar>>> Get()
        {
            List<CuentaXCobrar> cuentaXCobrar = await DbContext.CuentaXCobrar.ToListAsync();
            if (cuentaXCobrar == null || cuentaXCobrar.Count == 0)
            {
                Logger.LogWarning("No existen cuentas por cobrar en la base de datos");
                return new NoContentResult();
            }
            Logger.LogInformation("Se ejecuto la peticion de forma exitosa");
            return Ok(cuentaXCobrar);
        }
        [HttpGet("{id}", Name = "GetCuentaXCobrar")]
        public async Task<ActionResult<CuentaXCobrar>> GetCuentaxCobrar(string id)
        {
            Logger.LogDebug($"Iniciando el proceso de busqueda con el id: {id}");
            var cuentaXCobrar = await DbContext.CuentaXCobrar.FirstOrDefaultAsync(cxc => cxc.Cargo == id);
            if (cuentaXCobrar == null)
            {
                Logger.LogWarning($"No existe una cuenta por cobrar con ID: {id}");
                return new NoContentResult();
            }
            Logger.LogInformation("Finalizando el proceso de busqueda de forma exitosa");
            return Ok(cuentaXCobrar);
        }
        [HttpPost]
        public async Task<ActionResult<CuentaXCobrar>> PostCuentaXCobrar([FromBody] CuentaXCobrar value)
        {
            Logger.LogDebug("Iniciando el proceso de creacion de un nuevo cargo");
            await DbContext.CuentaXCobrar.AddAsync(value);
            await DbContext.SaveChangesAsync();
            Logger.LogInformation("Finalizando el proceso de creacion de una cuenta por cobrar");
            return new CreatedAtRouteResult("GetCuentaXCobrar", new { id = value.Cargo }, value);
        }
        [HttpDelete("{id}")]
        public async Task<ActionResult<CuentaXCobrar>> DeleteCuentaXCobrar(string id)
        {
            Logger.LogDebug("Iniciando el proceso de eliminacion de una cuenta por cobrar");
            CuentaXCobrar cuentaXCobrar = await DbContext.CuentaXCobrar.FirstOrDefaultAsync(cxc => cxc.Cargo == id);
            if (cuentaXCobrar == null)
            {
                Logger.LogWarning($"No existe un cargo con el ID: {id}");
                return NotFound();
            }
            else
            {
                DbContext.CuentaXCobrar.Remove(cuentaXCobrar);
                Logger.LogInformation("Finalizando el proceso de eliminacion de una cuenta por cobrar");
                return cuentaXCobrar;
            }
        }
        [HttpPut]
        public async Task<ActionResult> PutCuentaXCobrar(string id, [FromBody] CuentaXCobrar value)
        {
            Logger.LogDebug($"Iniciando el proceso de actualizacion de una cuenta por cobrar con ID: {id}");
            CuentaXCobrar cuentaXCobrar = await DbContext.CuentaXCobrar.FirstOrDefaultAsync(cxc => cxc.Cargo == id);
            if (cuentaXCobrar == null)
            {
                Logger.LogWarning($"No existe una cuenta por cobrar con ID: {id}");
                return NotFound();
            }
            cuentaXCobrar.Cargo = value.Cargo;
            cuentaXCobrar.Anio = value.Anio;
            cuentaXCobrar.Carne = value.Carne;
            cuentaXCobrar.CargoId = value.CargoId;
            cuentaXCobrar.Descripcion = value.Descripcion;
            cuentaXCobrar.FechaCargo = value.FechaCargo;
            cuentaXCobrar.FechaAplica = value.FechaAplica;
            cuentaXCobrar.Monto = value.Monto;
            cuentaXCobrar.Mora = value.Mora;
            cuentaXCobrar.Descuento = value.Descuento;
            DbContext.Entry(cuentaXCobrar).State = EntityState.Modified;
            await DbContext.SaveChangesAsync();
            Logger.LogInformation($"Finalizando el proceso de actualizacion de una cuenta por cobrar con ID {id}");
            return NoContent();
        }
    }
}