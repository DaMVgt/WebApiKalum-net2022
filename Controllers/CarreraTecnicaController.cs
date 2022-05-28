using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApiKalum.Entities;

namespace WebApiKalum.Controllers
{
    [ApiController]
    [Route("v1/KalumManagement/[controller]")]
    public class CarreraTecnicaController : ControllerBase
    {
        private readonly KalumDbContext DbContext;
        public CarreraTecnicaController(KalumDbContext _dbContext)
        {
            this.DbContext = _dbContext;
        }
        [HttpGet]
        public ActionResult<List<CarreraTecnicaController>> Get()
        {
            List<CarreraTecnica> carrerasTecnicas = null;
            carrerasTecnicas = DbContext.CarreraTecnica.Include(c => c.Aspirantes).Include(i => i.Inscripciones).ToList();
            if (carrerasTecnicas == null || carrerasTecnicas.Count == 0){
                return new NoContentResult();
            }
            return Ok(carrerasTecnicas);
        }
    }
}