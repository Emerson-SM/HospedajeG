using Microsoft.AspNetCore.Mvc;
using HospedajeAccesoDatos.Repositories;
using HospedajeAccesoDatos.Repositories.Interfaces;
using HospedajeAccesoDatos.Models;

namespace HospedajeAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReservacionesController : ControllerBase
    {
        private readonly IReservacionesRepository _repo;
        private readonly IClienteRepository _clienteRepo;
        private readonly IHabitacionesRepository _habitacionRepo;

        public ReservacionesController(
            IReservacionesRepository repo,
            IClienteRepository clienteRepo,
            IHabitacionesRepository habitacionRepo)
        {
            _repo = repo;
            _clienteRepo = clienteRepo;
            _habitacionRepo = habitacionRepo;
        }

        // GET: api/Reservaciones
        [HttpGet]
        public ActionResult<IEnumerable<Reservacion>> Get()
        {
            return Ok(_repo.ObtenerTodo());
        }

        // GET: api/Reservaciones/buscar?criterioBusqueda=RES-0001
        [HttpGet("buscar")]
        public ActionResult<IEnumerable<Reservacion>> Buscar([FromQuery] string criterioBusqueda)
        {
            if (string.IsNullOrEmpty(criterioBusqueda))
                return BadRequest("El criterio de búsqueda es requerido.");

            // Filtramos sobre la lista total para soportar la barra de búsqueda flexible del MVC.
            var resultados = _repo.ObtenerTodo()
                .Where(r => r.ReservacionId.Contains(criterioBusqueda, StringComparison.OrdinalIgnoreCase) ||
                            r.ClienteId.Contains(criterioBusqueda, StringComparison.OrdinalIgnoreCase))
                .ToList();

            return Ok(resultados);
        }

        // GET: api/Reservaciones/RES-0001
        [HttpGet("{id}")]
        public ActionResult<Reservacion> GetPorId(string id)
        {
            var reservacion = _repo.ObtenerPorId(id);

            if (reservacion == null)
                return NotFound($"No existe la reservación con el código: {id}");

            return Ok(reservacion);
        }

        // POST: api/Reservaciones
        [HttpPost]
        public IActionResult Post([FromBody] Reservacion nuevaReservacion)
        {
            if (nuevaReservacion == null)
                return BadRequest(new { mensaje = "Datos de la reservación inválidos." });

            if (nuevaReservacion.FechaSalida <= nuevaReservacion.FechaInicio)
            {
                ModelState.AddModelError("FechaSalida", "La fecha de salida debe ser posterior a la fecha de inicio.");
            }

            // Validar si existe el cliente
            bool clienteExiste =
                !string.IsNullOrWhiteSpace(nuevaReservacion.ClienteId) &&
                _clienteRepo.ObtenerPorId(nuevaReservacion.ClienteId) != null;

            if (!clienteExiste)
            {
                ModelState.AddModelError("ClienteId",
                    "El cliente especificado no está registrado en el sistema.");
            }

            // Validar si existe la habitación
            bool habitacionExiste =
                !string.IsNullOrWhiteSpace(nuevaReservacion.HabitacionId) &&
                _habitacionRepo.ObtenerPorId(nuevaReservacion.HabitacionId) != null;

            if (!habitacionExiste)
            {
                ModelState.AddModelError("HabitacionId",
                    "La habitación seleccionada no existe.");
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (string.IsNullOrWhiteSpace(nuevaReservacion.ReservacionId))
            {
                nuevaReservacion.ReservacionId = $"RES-{Guid.NewGuid().ToString().Substring(0, 5).ToUpper()}";
            }

            _repo.Add(nuevaReservacion);

            return CreatedAtAction(nameof(GetPorId), new { id = nuevaReservacion.ReservacionId }, nuevaReservacion);
        }

        // PUT: api/Reservaciones/RES-0001
        [HttpPut("{id}")]
        public IActionResult Put(string id, [FromBody] Reservacion reservacionActualizada)
        {
            if (reservacionActualizada == null || id != reservacionActualizada.ReservacionId)
                return BadRequest("El identificador no coincide con los datos proporcionados.");

            if (reservacionActualizada.FechaSalida <= reservacionActualizada.FechaInicio)
                return BadRequest("La fecha de salida debe ser posterior a la fecha de inicio.");

            var existente = _repo.ObtenerPorId(id);
            if (existente == null)
                return NotFound("No existe la reservación que desea actualizar.");
            try
            {
                _repo.Update(reservacionActualizada);
                return NoContent();
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { mensaje = ex.Message });
            }

            _repo.Update(reservacionActualizada);
            return NoContent();
        }

        // DELETE: api/Reservaciones/RES-0001
        [HttpDelete("{id}")]
        public IActionResult Delete(string id)
        {
            var existente = _repo.ObtenerPorId(id);
            if (existente == null)
                return NotFound("No existe la reservación solicitada.");

            if (existente.Estado != null && existente.Estado.Equals("En proceso", StringComparison.OrdinalIgnoreCase))
                return BadRequest("No se puede eliminar una reservación que ya se encuentra en proceso en el hotel.");

            _repo.Delete(id);
            return NoContent();
        }
    }
}