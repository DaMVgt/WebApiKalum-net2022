using System.Text;
using System.Text.Json;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RabbitMQ.Client;
using WebApiKalum.Dtos;
using WebApiKalum.Dtos.Lists;
using WebApiKalum.Entities;

namespace WebApiKalum.Controller
{
    [ApiController]
    [Route("v1/KalumManagement/Inscripciones")]
    public class InscripcionController : ControllerBase
    {
        private readonly KalumDbContext DbContext;
        private readonly ILogger<InscripcionController> Logger;
        private readonly IMapper Mapper;
        public InscripcionController(KalumDbContext _dbContext, ILogger<InscripcionController> _Logger, IMapper _Mapper)
        {
            this.DbContext = _dbContext;
            this.Logger = _Logger;
            this.Mapper = _Mapper;
        }
        [HttpGet]
        public async Task<ActionResult<IEnumerable<InscripcionesListDTO>>> Get()
        {
            Logger.LogDebug("Iniciando proceso de consulta de inscripciones en base de datos");
            List<Inscripcion> inscripciones = await DbContext.Inscripcion.Include(i => i.Jornada).Include(i => i.CarreraTecnica).Include(i => i.Alumno).ToListAsync();
            if (inscripciones == null || inscripciones.Count == 0)
            {
                Logger.LogWarning("No existen inscripciones en la base de datos");
                return new NoContentResult();
            }
            List<InscripcionesListDTO> lista = Mapper.Map<List<InscripcionesListDTO>>(inscripciones);
            Logger.LogInformation("Se ejecuto la peticion de forma exitosa");
            return Ok(lista);
        }
        [HttpGet("{id}", Name = "GetInscripcion")]
        public async Task<ActionResult<InscripcionesListDTO>> GetInscripcion(string id)
        {
            Logger.LogDebug($"Iniciando el proceso de busca con el ID: {id}");
            var inscripcion = await DbContext.Inscripcion.Include(i => i.Jornada).Include(i => i.CarreraTecnica).Include(i => i.Alumno).FirstOrDefaultAsync(i => i.InscripcionId == id);
            if (inscripcion == null)
            {
                Logger.LogWarning($"No existe una inscripcion con ID: {id}");
                return new NoContentResult();
            }
            var lista = Mapper.Map<InscripcionesListDTO>(inscripcion);
            Logger.LogInformation("Finalizando el proceso de busqueda de forma exitosa");
            return Ok(lista);
        }
        [HttpPost]
        public async Task<ActionResult<Inscripcion>> PostInscripcion([FromBody] Inscripcion value)
        {
            Logger.LogDebug("Iniciando el proceso de creacion de una inscripcion");
            value.InscripcionId = Guid.NewGuid().ToString().ToUpper();
            Alumno alumno = await DbContext.Alumno.FirstOrDefaultAsync(a => a.Carne == value.Carne);
            if (alumno == null)
            {
                Logger.LogInformation($"No existe un alumno con el ID: {value.Carne}");
                return BadRequest();
            }
            CarreraTecnica carreraTecnica = await DbContext.CarreraTecnica.FirstOrDefaultAsync(ct => ct.CarreraId == value.CarreraId);
            if (carreraTecnica == null)
            {
                Logger.LogInformation($"No existe una carrera tecnica con el ID: {value.CarreraId}");
                return BadRequest();
            }
            Jornada jornada = await DbContext.Jornada.FirstOrDefaultAsync(j => j.JornadaId == value.JornadaId);
            if (jornada == null)
            {
                Logger.LogInformation($"No existe una jornada con el ID: {value.JornadaId}");
                return BadRequest();
            }
            await DbContext.Inscripcion.AddAsync(value);
            await DbContext.SaveChangesAsync();
            Logger.LogInformation($"Se creo una inscripcion con el id: {value.InscripcionId}");
            return Ok(value);
        }

        [HttpPost("Enrollments")]
        public async Task<ActionResult<ResponseEnrollmentDTO>> EnrollmentCreatesAsync([FromBody] EnrollmentDTO value)
        {
            Aspirante aspirante = await DbContext.Aspirante.FirstOrDefaultAsync(a => a.NoExpediente == value.NoExpediente);
            if (aspirante == null)
            {
                return NoContent();
            }
            CarreraTecnica carreraTecnica = await DbContext.CarreraTecnica.FirstOrDefaultAsync(ct => ct.CarreraId == value.CarreraId);
            if (carreraTecnica == null)
            {
                return NoContent();
            }
            bool respuesta = await CrearSolicitudAsync(value);
            if (respuesta == true)
            {
                ResponseEnrollmentDTO response = new ResponseEnrollmentDTO();
                response.HttpStatus = 201;
                response.Message = "El proceso de inscripcion se ha realizado con exito";
                return Ok(response);
            }
            else
            {
                return StatusCode(503, value);
            }
        }
        private async Task<bool> CrearSolicitudAsync(EnrollmentDTO value)
        {
            bool proceso = false;
            ConnectionFactory factory = new ConnectionFactory();
            IConnection conexion = null;
            IModel channel = null;
            factory.HostName = "Localhost";
            factory.VirtualHost = "/";
            factory.Port = 5672;
            factory.UserName = "guest";
            factory.Password = "guest";
            try
            {
                conexion = factory.CreateConnection();
                channel = conexion.CreateModel();
                channel.BasicPublish("kalum.exchange.enrollment", "", null, Encoding.UTF8.GetBytes(JsonSerializer.Serialize(value)));
                proceso = true;
            }
            catch (Exception e)
            {
                Logger.LogError(e.Message);
            }
            finally
            {
                channel.Close();
                conexion.Close();
            }
            return proceso;
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<Inscripcion>> DeleteInscripcion(string id)
        {
            Logger.LogDebug("Iniciando el proceso de eliminacion de una inscripcion");
            Inscripcion inscripcion = await DbContext.Inscripcion.FirstOrDefaultAsync(i => i.InscripcionId == id);
            if (inscripcion == null)
            {
                Logger.LogWarning($"No existe una inscripcion con ID: {id}");
                return NotFound();
            }
            else
            {
                DbContext.Inscripcion.Remove(inscripcion);
                await DbContext.SaveChangesAsync();
                Logger.LogInformation("Finalizando el proceso de eliminacion de una inscripcion");
                return inscripcion;
            }
        }
        [HttpPut("{id}")]
        public async Task<ActionResult> PutInscripcion(string id, [FromBody] Inscripcion value)
        {
            Logger.LogDebug($"Iniciando el proceso de actualizacion de un cargo con ID: {id}");
            Inscripcion inscripcion = await DbContext.Inscripcion.FirstOrDefaultAsync(i => i.InscripcionId == id);
            if (inscripcion == null)
            {
                Logger.LogWarning($"No existe una inscripcion con ID: {id}");
                return NotFound();
            }
            inscripcion.Carne = value.Carne;
            inscripcion.CarreraId = value.CarreraId;
            inscripcion.JornadaId = value.JornadaId;
            inscripcion.Ciclo = value.Ciclo;
            inscripcion.FechaInscripcion = value.FechaInscripcion;
            DbContext.Entry(inscripcion).State = EntityState.Modified;
            await DbContext.SaveChangesAsync();
            Logger.LogInformation($"Finalizando el proceso de actualizacion de una inscripcion con el ID: {id}");
            return NoContent();
        }
    }
}