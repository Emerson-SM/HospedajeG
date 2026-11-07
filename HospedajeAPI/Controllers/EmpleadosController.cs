using Microsoft.AspNetCore.Mvc;
using HospedajeAccesoDatos.Repositories;
using HospedajeAccesoDatos.Models;
using HospedajeAccesoDatos.Repositories.Interfaces;

namespace HospedajeAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmpleadosController : ControllerBase
    {
        private readonly IEmpleadoRepository _repo;

        public EmpleadosController(IEmpleadoRepository repo)
        {
            _repo = repo;
        }

        // GET: api/Empleados
        [HttpGet]
        public ActionResult<IEnumerable<Empleado>> Get()
        {
            return Ok(_repo.ObtenerTodo());
        }

        // GET: api/Empleados/1-1111-1111
        [HttpGet("{id}")]
        public ActionResult<Empleado> GetPorId(string id)
        {
            if (string.IsNullOrWhiteSpace(id))
                return BadRequest(new { mensaje = "El ID del empleado es requerido." });

            var empleado = _repo.ObtenerPorId(id);

            if (empleado == null)
                return NotFound(new { mensaje = "No existe el empleado consultado." });

            return Ok(empleado);
        }

        // POST: api/Empleados
        [HttpPost]
        public IActionResult Post([FromBody] Empleado nuevoEmpleado)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (nuevoEmpleado == null || string.IsNullOrWhiteSpace(nuevoEmpleado.EmpleadoId))
                return BadRequest(new { mensaje = "Datos del empleado inválidos." });

            var existente = _repo.ObtenerPorId(nuevoEmpleado.EmpleadoId);
            if (existente != null)
                return BadRequest(new { mensaje = "Ya existe un empleado registrado con esta identificación." });

            _repo.Add(nuevoEmpleado);

            // Genera la cabecera Location -> api/Empleados/1-1111-1111
            return CreatedAtAction(nameof(GetPorId), new { id = nuevoEmpleado.EmpleadoId }, nuevoEmpleado);
        }

        // PUT: api/Empleados/1-1111-1111
        [HttpPut("{id}")]
        public IActionResult Put(string id, [FromBody] Empleado empleadoActualizado)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (empleadoActualizado == null || id != empleadoActualizado.EmpleadoId)
                return BadRequest(new { mensaje = "El ID de la ruta no coincide con la identificación enviada." });

            var existente = _repo.ObtenerPorId(id);
            if (existente == null)
                return NotFound(new { mensaje = "No existe el empleado que desea actualizar." });

            _repo.Update(empleadoActualizado);
            return NoContent(); // 204 No Content
        }

        // DELETE: api/Empleados/1-1111-1111
        [HttpDelete("{id}")]
        public IActionResult Delete(string id)
        {
            if (string.IsNullOrWhiteSpace(id))
                return BadRequest(new { mensaje = "El ID es requerido." });

            var existente = _repo.ObtenerPorId(id);
            if (existente == null)
                return NotFound(new { mensaje = "No existe el empleado a eliminar." });

            _repo.Delete(id);
            return NoContent();
        }
    }
}
