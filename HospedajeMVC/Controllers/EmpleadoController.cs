using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering; 
using HospedajeMvc.Models;
using HospedajeMVC.Services;

namespace API_project.Controllers
{
    public class EmpleadoController : Controller
    {
        private readonly IEmpleadoApiService _apiService;

        public EmpleadoController(IEmpleadoApiService apiService)
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

        // GET: Empleado
        public async Task<ActionResult> Index()
        {
            var listaEmpleados = await _apiService.ObtenerTodosAsync();
            return View(listaEmpleados);
        }

        // GET: Empleado/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: Empleado/Create
        public ActionResult Create()
        {
            CargarTiposIdentificacion();
            return View();
        }

        // POST: Empleado/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(Empleado nuevoEmpleado)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    CargarTiposIdentificacion(); 
                    return View(nuevoEmpleado);
                }

                var existente = await _apiService.BuscarPorIdAsync(nuevoEmpleado.EmpleadoId);
                if (existente != null)
                {
                    ModelState.AddModelError("EmpleadoId", "Ya existe un empleado registrado con esta identificación.");
                    CargarTiposIdentificacion(); 
                    return View(nuevoEmpleado);
                }

                bool exito = await _apiService.CrearAsync(nuevoEmpleado);
                if (exito)
                {
                    TempData["SuccessMessage"] = "Empleado registrado con éxito.";
                    return RedirectToAction(nameof(Index));
                }

                ModelState.AddModelError("", "No se pudo registrar el empleado en la API.");
                CargarTiposIdentificacion(); 
                return View(nuevoEmpleado);
            }
            catch
            {
                CargarTiposIdentificacion(); 
                return View(nuevoEmpleado);
            }
        }

        // GET: Empleado/Edit/5
        public async Task<ActionResult> Edit(string id)
        {
            var empleado = await _apiService.BuscarPorIdAsync(id);
            if (empleado == null)
            {
                return NotFound();
            }

            CargarTiposIdentificacion(); 
            return View(empleado);
        }

        // POST: Empleado/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(string id, Empleado emModificado)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    CargarTiposIdentificacion(); 
                    return View(emModificado);
                }

                emModificado.EmpleadoId = id;
                bool exito = await _apiService.ActualizarAsync(id, emModificado);

                if (exito)
                {
                    TempData["SuccessMessage"] = "Empleado actualizado con éxito.";
                    return RedirectToAction(nameof(Index));
                }

                CargarTiposIdentificacion();
                return NotFound();
            }
            catch
            {
                CargarTiposIdentificacion();
                return View(emModificado);
            }
        }

        // GET: Empleado/Delete/5
        public async Task<ActionResult> Delete(string id)
        {
            var empleado = await _apiService.BuscarPorIdAsync(id);
            if (empleado == null)
            {
                return NotFound();
            }

            return View(empleado);
        }

        // POST: Empleado/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Delete(string id, IFormCollection collection)
        {
            try
            {
                string? errorMsg = await _apiService.EliminarAsync(id);

                if (errorMsg == null)
                {
                    TempData["SuccessMessage"] = "Empleado eliminado con éxito.";
                    return RedirectToAction(nameof(Index));
                }

                TempData["ErrorMessage"] = errorMsg;
                var empleado = await _apiService.BuscarPorIdAsync(id);
                return View(empleado);
            }
            catch
            {
                TempData["ErrorMessage"] = "Ocurrió un error inesperado al intentar eliminar el registro.";
                var empleado = await _apiService.BuscarPorIdAsync(id);
                return View(empleado);
            }
        }

        // GET: Empleado/Buscar
        [HttpGet]
        public async Task<IActionResult> Buscar(string criterioBusqueda)
        {
            var resultados = new List<Empleado>();

            if (!string.IsNullOrEmpty(criterioBusqueda))
            {
                var empleado = await _apiService.BuscarPorIdAsync(criterioBusqueda);
                if (empleado != null)
                {
                    resultados.Add(empleado);
                }

                ViewData["CriterioActual"] = criterioBusqueda;
            }

            return View(resultados);
        }
    }
}