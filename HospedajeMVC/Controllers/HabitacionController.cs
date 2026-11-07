using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using HospedajeMvc.Models;
using HospedajeMVC.Services; 

namespace API_project.Controllers
{
    public class HabitacionController : Controller
    {
        private readonly IHabitacionApiService _apiService;

        public HabitacionController(IHabitacionApiService apiService)
        {
            _apiService = apiService;
        }

        // GET: Habitacion
        public async Task<ActionResult> Index()
        {
            var listaHabitaciones = await _apiService.ObtenerTodosAsync();
            return View(listaHabitaciones);
        }

        // GET: Habitacion/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: Habitacion/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Habitacion/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(Habitacion nuevaHabitacion)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return View(nuevaHabitacion);
                }

                var existente = await _apiService.BuscarPorIdAsync(nuevaHabitacion.HabitacionId);
                if (existente != null)
                {
                    ModelState.AddModelError("HabitacionId", "Ya existe una habitación registrada con esta identificación.");
                    return View(nuevaHabitacion);
                }

                bool exito = await _apiService.CrearAsync(nuevaHabitacion);
                if (exito)
                {
                    TempData["SuccessMessage"] = "Habitación registrada con éxito.";
                    return RedirectToAction(nameof(Index));
                }

                ModelState.AddModelError("", "No se pudo registrar la habitación en el servidor central.");
                return View(nuevaHabitacion);
            }
            catch
            {
                return View(nuevaHabitacion);
            }
        }

        // GET: Habitacion/Edit/5
        public async Task<ActionResult> Edit(string id)
        {
            var habitacion = await _apiService.BuscarPorIdAsync(id);
            if (habitacion == null)
            {
                return NotFound();
            }

            return View(habitacion);
        }

        // POST: Habitacion/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(string id, Habitacion habitacionMod)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return View(habitacionMod);
                }

                habitacionMod.HabitacionId = id; 
                bool exito = await _apiService.ActualizarAsync(id, habitacionMod);

                if (exito)
                {
                    TempData["SuccessMessage"] = "Habitación actualizada con éxito.";
                    return RedirectToAction(nameof(Index));
                }

                return NotFound();
            }
            catch
            {
                return View(habitacionMod);
            }
        }

        // GET: Habitacion/Delete/5
        public async Task<ActionResult> Delete(string id)
        {
            var habitacion = await _apiService.BuscarPorIdAsync(id);
            if (habitacion == null)
            {
                return NotFound();
            }

            return View(habitacion);
        }

        // POST: Habitacion/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Delete(string id, IFormCollection collection)
        {
            try
            {
                string? errorMsg = await _apiService.EliminarAsync(id);

                if (errorMsg == null)
                {
                    TempData["SuccessMessage"] = "Habitación eliminada con éxito.";
                    return RedirectToAction(nameof(Index));
                }

                TempData["ErrorMessage"] = errorMsg;
                var habitacion = await _apiService.BuscarPorIdAsync(id);
                return View(habitacion);
            }
            catch
            {
                TempData["ErrorMessage"] = "Ocurrió un error inesperado al intentar eliminar el registro.";
                var habitacion = await _apiService.BuscarPorIdAsync(id);
                return View(habitacion);
            }
        }

        // GET: Habitacion/Buscar
        [HttpGet]
        public async Task<IActionResult> Buscar(string criterioBusqueda)
        {
            var resultados = new List<Habitacion>();

            if (!string.IsNullOrEmpty(criterioBusqueda))
            {
                var habitacion = await _apiService.BuscarPorIdAsync(criterioBusqueda);
                if (habitacion != null)
                {
                    resultados.Add(habitacion);
                }

                ViewData["CriterioActual"] = criterioBusqueda;
            }

            return View(resultados);
        }
    }
}