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
    }
}