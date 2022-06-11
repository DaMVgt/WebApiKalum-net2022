using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApiKalum.Entities;

namespace WebApiKalum.Controllers
{
    [ApiController]
    [Route("v1/KalumManagement/Cargos")]
    public class CargoController : ControllerBase
    {
        private readonly KalumDbContext DbContext;
        private readonly ILogger<CargoController> Logger;
        public CargoController(KalumDbContext _dbContext, ILogger<CargoController> _Logger)
        {
            this.DbContext = _dbContext;
            this.Logger = _Logger;
        }
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Cargo>>> Get()
        {
            List<Cargo> cargos = null;
            Logger.LogDebug("Iniciando proceso de consulta de cargos en la base de datos");
            cargos = await DbContext.Cargo.Include(c => c.CuentasXCobrar).ToListAsync();
            if (cargos == null || cargos.Count == 0)
            {
                Logger.LogWarning("No existen cargos en la base de datos");
                return new NoContentResult();
            }
            Logger.LogInformation("Se ejecutó la peticion de forma exitosa");
            return Ok(cargos);
        }
        [HttpGet("{id}", Name = "GetCargo")]
        public async Task<ActionResult<Cargo>> GetCargo(string id)
        {
            Logger.LogDebug("Iniciando el proceso de busca con el id: " + id);
            var cargo = await DbContext.Cargo.Include(c => c.CuentasXCobrar).FirstOrDefaultAsync(c => c.CargoId == id);
            if (cargo == null)
            {
                Logger.LogWarning("No existe un cargo con el id: " + id);
                return new NoContentResult();
            }
            Logger.LogInformation("Finalizando el proceso de busqueda de forma exitosa");
            return Ok(cargo);
        }
        [HttpPost]
        public async Task<ActionResult<Cargo>> PostCargo([FromBody] Cargo value)
        {
            Logger.LogDebug("Iniciando el proceso de creacion de un nuevo cargo");
            value.CargoId = Guid.NewGuid().ToString().ToUpper();
            value.GeneraMora = false;
            value.PorcentajeMora = 0;
            await DbContext.Cargo.AddAsync(value);
            await DbContext.SaveChangesAsync();
            Logger.LogInformation("Finalizando el proceso de creacion de un nuevo cargo");
            return new CreatedAtRouteResult("GetCargo", new { id = value.CargoId }, value);
        }
        [HttpDelete("{id}")]
        public async Task<ActionResult<Cargo>> DeleteCargo(string id)
        {
            Logger.LogDebug("Iniciando el proceso de eliminacion de un cargo");
            Cargo cargo = await DbContext.Cargo.FirstOrDefaultAsync(c => c.CargoId == id);
            if(cargo == null)
            {
                Logger.LogWarning($"No existe un cargo con el id: {id}");
                return NotFound();
            }
            else{
                DbContext.Cargo.Remove(cargo);
                await DbContext.SaveChangesAsync();
                Logger.LogInformation("Finalizando el proceso de eliminacion de un cargo");
                return cargo;
            }
        }
        [HttpPut("{id}")]
        public async Task<ActionResult> PutCargo(string id, [FromBody] Cargo value)
        {
            Logger.LogDebug($"Iniciando el proceso de actualizacion de un cargo con el id: {id}");
            Cargo cargo = await DbContext.Cargo.FirstOrDefaultAsync(c => c.CargoId == id);
            if(cargo == null){
                Logger.LogWarning($"No existe un cargo con el id: {id}");
                return NotFound();
            }
            cargo.Descripcion = value.Descripcion;
            cargo.Prefijo = value.Prefijo;
            cargo.Monto = value.Monto;
            DbContext.Entry(cargo).State = EntityState.Modified;
            await DbContext.SaveChangesAsync();
            Logger.LogInformation($"Finalizando el proceso de actualizacion de un cargo con el id: {id}");
            return NoContent();
        }
    }
}