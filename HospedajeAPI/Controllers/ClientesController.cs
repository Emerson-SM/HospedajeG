using Microsoft.AspNetCore.Mvc;
using HospedajeAccesoDatos.Repositories;
using HospedajeAccesoDatos.Repositories.Interfaces;
using HospedajeAccesoDatos.Models;

namespace HospedajeAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ClientesController : ControllerBase
    {
        private readonly IClienteRepository _repo;

        public ClientesController(IClienteRepository repo)
        {
            _repo = repo;
        }

        // GET: api/Clientes
        [HttpGet]
        public ActionResult<IEnumerable<Cliente>> Get()
        {
            return Ok(_repo.ObtenerTodo());
        }

        // GET: api/Clientes/1-1111-1111
        [HttpGet("{id}")]
        public ActionResult<Cliente> GetPorId(string id)
        {
            if (string.IsNullOrWhiteSpace(id))
                return BadRequest(new { mensaje = "El ID del cliente es requerido." });

            var cliente = _repo.ObtenerPorId(id);

            if (cliente == null)
                return NotFound(new { mensaje = "No existe el cliente consultado." });

            return Ok(cliente);
        }

        // POST: api/Clientes
        [HttpPost]
        public IActionResult Post([FromBody] Cliente nuevoCliente)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (nuevoCliente == null || string.IsNullOrWhiteSpace(nuevoCliente.ClienteId))
                return BadRequest(new { mensaje = "Datos del cliente inválidos." });

            var existente = _repo.ObtenerPorId(nuevoCliente.ClienteId);
            if (existente != null)
                return BadRequest(new { mensaje = "Ya existe un cliente registrado con esta identificación." });

            _repo.Add(nuevoCliente);

            // Genera la cabecera Location -> api/Clientes/1-1111-1111
            return CreatedAtAction(nameof(GetPorId), new { id = nuevoCliente.ClienteId }, nuevoCliente);
        }

        // PUT: api/Clientes/1-1111-1111
        [HttpPut("{id}")]
        public IActionResult Put(string id, [FromBody] Cliente clienteActualizado)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (clienteActualizado == null || id != clienteActualizado.ClienteId)
                return BadRequest(new { mensaje = "El ID de la ruta no coincide con la identificación enviada." });

            var existente = _repo.ObtenerPorId(id);
            if (existente == null)
                return NotFound(new { mensaje = "No existe el cliente que desea actualizar." });

            _repo.Update(clienteActualizado);
            return NoContent(); // 204 No Content
        }

        // DELETE: api/Clientes/1-1111-1111
        [HttpDelete("{id}")]
        public IActionResult Delete(string id)
        {
            if (string.IsNullOrWhiteSpace(id))
                return BadRequest(new { mensaje = "El ID es requerido." });

            var existente = _repo.ObtenerPorId(id);
            if (existente == null)
                return NotFound(new { mensaje = "No existe el cliente a eliminar." });

            // Validación obligatoria de negocio antes de borrar
            if (_repo.TieneReservacionesAsociadas(id))
            {
                return BadRequest(new { mensaje = "No se puede eliminar el cliente porque tiene reservaciones registradas." });
            }

            _repo.Delete(id);
            return NoContent();
        }
    }
}