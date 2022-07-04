using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApiKalum.Dtos.Creates;
using WebApiKalum.Dtos.Lists;
using WebApiKalum.Entities;
using WebApiKalum.Utilities;

namespace WebApiKalum.Controllers
{
    [ApiController]
    [Route("v1/KalumManagement/Cargos")]
    public class CargoController : ControllerBase
    {
        private readonly KalumDbContext DbContext;
        private readonly ILogger<CargoController> Logger;
        private readonly IMapper Mapper;
        public CargoController(KalumDbContext _dbContext, ILogger<CargoController> _Logger, IMapper _Mapper)
        {
            this.DbContext = _dbContext;
            this.Logger = _Logger;
            this.Mapper = _Mapper;
        }
        [HttpGet]
        public async Task<ActionResult<IEnumerable<CargoListDTO>>> Get()
        {
            Logger.LogDebug("Iniciando proceso de consulta de cargos en la base de datos");
            List<Cargo> cargos = await DbContext.Cargo.Include(c => c.CuentasXCobrar).ToListAsync();
            if (cargos == null || cargos.Count == 0)
            {
                Logger.LogWarning("No existen cargos en la base de datos");
                return new NoContentResult();
            }
            List<CargoListDTO> lista = Mapper.Map<List<CargoListDTO>>(cargos);
            Logger.LogInformation("Se ejecutó la peticion de forma exitosa");
            return Ok(lista);
        }
        [HttpGet("page/{page}")]
        public async Task<ActionResult<IEnumerable<CargoListDTO>>> GetPaginacion(int page)
        {
            var queryable = await DbContext.Cargo.Include(c => c.CuentasXCobrar).ToListAsync();
            var lista = Mapper.Map<List<CargoListDTO>>(queryable).AsQueryable();
            var paginacion = new HttpResponsePaginacion<CargoListDTO>(lista, page);
            if (paginacion.Content == null && paginacion.Content.Count == 0)
            {
                Logger.LogWarning("No existen cargos en la base de datos");
                return NoContent();
            }
            else
            {
                Logger.LogInformation("Se ejecuto la peticion de forma exitosa");
                return Ok(paginacion);
            }
        }
        [HttpGet("{id}", Name = "GetCargo")]
        public async Task<ActionResult<CargoListDTO>> GetCargo(string id)
        {
            Logger.LogDebug("Iniciando el proceso de busca con el id: " + id);
            var cargo = await DbContext.Cargo.Include(c => c.CuentasXCobrar).FirstOrDefaultAsync(c => c.CargoId == id);
            if (cargo == null)
            {
                Logger.LogWarning("No existe un cargo con el id: " + id);
                return new NoContentResult();
            }
            var lista = Mapper.Map<CargoListDTO>(cargo);
            Logger.LogInformation("Finalizando el proceso de busqueda de forma exitosa");
            return Ok(lista);
        }
        [HttpPost]
        public async Task<ActionResult<Cargo>> PostCargo([FromBody] CargoCreateDTO value)
        {
            Logger.LogDebug("Iniciando el proceso de creacion de un nuevo cargo");
            Cargo nuevo = Mapper.Map<Cargo>(value);
            nuevo.CargoId = Guid.NewGuid().ToString().ToUpper();
            nuevo.GeneraMora = false;
            nuevo.PorcentajeMora = 0;
            await DbContext.Cargo.AddAsync(nuevo);
            await DbContext.SaveChangesAsync();
            Logger.LogInformation("Finalizando el proceso de creacion de un nuevo cargo");
            return new CreatedAtRouteResult("GetCargo", new { id = nuevo.CargoId }, nuevo);
        }
        [HttpDelete("{id}")]
        public async Task<ActionResult<Cargo>> DeleteCargo(string id)
        {
            Logger.LogDebug("Iniciando el proceso de eliminacion de un cargo");
            Cargo cargo = await DbContext.Cargo.FirstOrDefaultAsync(c => c.CargoId == id);
            if (cargo == null)
            {
                Logger.LogWarning($"No existe un cargo con el id: {id}");
                return NotFound();
            }
            else
            {
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
            if (cargo == null)
            {
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