using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering; 
using HospedajeMvc.Models;
using HospedajeMVC.Services;

namespace API_project.Controllers
{
    public class ClienteController : Controller
    {
        private readonly IClienteApiService _apiService;

        public ClienteController(IClienteApiService apiService)
        {
            _apiService = apiService;
        }

        // Método auxiliar privado para no duplicar código
        private void CargarTiposIdentificacion()
        {
            ViewBag.TiposIdentificacion = new List<SelectListItem>
            {
                new SelectListItem { Value = "Cédula", Text = "Cédula de Identidad" },
                new SelectListItem { Value = "DIMEX", Text = "DIMEX" },
                new SelectListItem { Value = "Pasaporte", Text = "Pasaporte" }
            };
        }

        // GET: Cliente
        public async Task<ActionResult> Index()
        {
            var listaClientes = await _apiService.ObtenerTodosAsync();
            return View(listaClientes);
        }

        // GET: Cliente/Create
        public ActionResult Create()
        {
            CargarTiposIdentificacion(); // <-- Cargamos el ViewBag
            return View();
        }

        // POST: Cliente/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(Cliente nuevoCliente)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    CargarTiposIdentificacion(); 
                    return View(nuevoCliente);
                }

                // Verificamos si existe consultando directamente a la API
                var existente = await _apiService.BuscarPorIdAsync(nuevoCliente.ClienteId);
                if (existente != null)
                {
                    ModelState.AddModelError("ClienteId", "Ya existe un cliente registrado con esta identificación.");
                    CargarTiposIdentificacion(); 
                    return View(nuevoCliente);
                }

                bool exito = await _apiService.CrearAsync(nuevoCliente);
                if (exito)
                {
                    TempData["SuccessMessage"] = "Cliente registrado con éxito.";
                    return RedirectToAction(nameof(Index));
                }

                ModelState.AddModelError("", "No se pudo registrar el cliente en el servidor central.");
                CargarTiposIdentificacion(); 
                return View(nuevoCliente);
            }
            catch
            {
                CargarTiposIdentificacion(); 
                return View(nuevoCliente);
            }
        }

        // GET: Cliente/Edit/
        [HttpGet("Cliente/Edit/{id}")]
        public async Task<ActionResult> Edit(string id)
        {
            var cliente = await _apiService.BuscarPorIdAsync(id);
            if (cliente == null)
            {
                return NotFound();
            }

            CargarTiposIdentificacion(); 
            return View(cliente);
        }

        // POST: Cliente/Edit/
        [HttpPost("Cliente/Edit/{id}")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(string id, Cliente clienteModificado)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    CargarTiposIdentificacion(); 
                    return View(clienteModificado);
                }

                clienteModificado.ClienteId = id;
                bool exito = await _apiService.ActualizarAsync(id, clienteModificado);

                if (exito)
                {
                    TempData["SuccessMessage"] = "Cliente actualizado con éxito.";
                    return RedirectToAction(nameof(Index));
                }

                CargarTiposIdentificacion();
                return NotFound();
            }
            catch
            {
                CargarTiposIdentificacion();
                return View(clienteModificado);
            }
        }

        // GET: Cliente/Delete/
        public async Task<ActionResult> Delete(string id)
        {
            var cliente = await _apiService.BuscarPorIdAsync(id);
            if (cliente == null)
            {
                return NotFound();
            }

            return View(cliente);
        }

        // POST: Cliente/Delete/
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Delete(string id, IFormCollection collection)
        {
            try
            {
                string? errorMsg = await _apiService.EliminarAsync(id);

                if (errorMsg == null)
                {
                    TempData["SuccessMessage"] = "Cliente eliminado con éxito.";
                    return RedirectToAction(nameof(Index));
                }

                TempData["ErrorMessage"] = errorMsg;
                var cliente = await _apiService.BuscarPorIdAsync(id);
                return View(cliente);
            }
            catch
            {
                TempData["ErrorMessage"] = "Ocurrió un error inesperado al intentar eliminar el registro.";
                var cliente = await _apiService.BuscarPorIdAsync(id);
                return View(cliente);
            }
        }

        // GET: Cliente/Buscar
        [HttpGet]
        public async Task<IActionResult> Buscar(string criterioBusqueda)
        {
            var resultados = new List<Cliente>();

            if (!string.IsNullOrEmpty(criterioBusqueda))
            {
                var cliente = await _apiService.BuscarPorIdAsync(criterioBusqueda);
                if (cliente != null)
                {
                    resultados.Add(cliente);
                }

                ViewData["CriterioActual"] = criterioBusqueda;
            }

            return View(resultados);
        }
    }
}