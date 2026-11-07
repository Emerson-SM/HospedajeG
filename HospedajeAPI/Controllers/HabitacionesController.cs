using Microsoft.AspNetCore.Mvc;
using HospedajeAccesoDatos.Repositories;
using HospedajeAccesoDatos.Models;
using HospedajeAccesoDatos.Repositories.Interfaces;

namespace HospedajeAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HabitacionesController : ControllerBase
    {
        private readonly IHabitacionesRepository _repo;

        public HabitacionesController(IHabitacionesRepository repo)
        {
            _repo = repo;
        }

        // GET: api/Clientes
        [HttpGet]
        public ActionResult<IEnumerable<Habitacion>> Get()
        {
            return Ok(_repo.ObtenerTodo());
        }

        // GET: api/Clientes/buscar?habitacionId=1-0000-0000
        [HttpGet("buscar")]
        public ActionResult<Habitacion> Buscar([FromQuery] string habitacionId)
        {
            if (string.IsNullOrEmpty(habitacionId))
                return BadRequest("El ID del habitacion es requerido.");

            var habitacion = _repo.ObtenerPorId(habitacionId);

            if (habitacion == null)
                return NotFound("No existe la habitacion");

            return Ok(habitacion);
        }

        // POST: api/Clientes
        [HttpPost]
        public IActionResult Post([FromBody] Habitacion nuevaHabitacion)
        {
            if (nuevaHabitacion == null)
                return BadRequest("Datos del habitacion inválidos.");

            var existente = _repo.ObtenerPorId(nuevaHabitacion.HabitacionId);
            if (existente != null)
                return BadRequest("Ya existe un habitacion con esta identificación.");

            _repo.Add(nuevaHabitacion);
            return CreatedAtAction(nameof(Buscar), new { habitacionId = nuevaHabitacion.HabitacionId }, nuevaHabitacion);
        }

        // PUT: api/Clientes/1-0000-0000
        [HttpPut("{id}")]
        public IActionResult Put(string id, [FromBody] Habitacion habitacionActualizada)
        {
            if (habitacionActualizada == null || id != habitacionActualizada.HabitacionId)
                return BadRequest("El ID no coincide con los datos proporcionados.");

            var existente = _repo.ObtenerPorId(id);
            if (existente == null)
                return NotFound("No existe la habitacion que desea actualizar.");

            _repo.Update(habitacionActualizada);
            return NoContent();
        }

        // DELETE: api/Clientes/1-0000-0000
        [HttpDelete("{id}")]
        public IActionResult Delete(string id)
        {
            var existente = _repo.ObtenerPorId(id);
            if (existente == null)
                return NotFound("No existe la habitacion");

            // Validación obligatoria de negocio antes de borrar
            if (_repo.TieneReservacionesAsociadas(id))
            {
                return BadRequest("No se puede eliminar la habitacion porque tiene reservaciones registradas.");
            }

            _repo.Delete(id);
            return NoContent();
        }
    }
}