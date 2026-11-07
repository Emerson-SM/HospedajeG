using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using HospedajeMvc.Models;
using HospedajeMVC.Services;

namespace API_project.Controllers
{
    public class ReservacionController : Controller
    {
        private readonly IReservacionApiService _apiService;

        public ReservacionController(IReservacionApiService apiService)
        {
            _apiService = apiService;
        }

        // GET: Reservacion
        public async Task<ActionResult> Index()
        {
            var listaReservaciones = await _apiService.ObtenerTodosAsync();
            return View(listaReservaciones);
        }

        // GET: Reservacion/Details/5
        public async Task<ActionResult> Details(string id)
        {
            var reservacion = await _apiService.BuscarPorIdAsync(id);
            if (reservacion == null)
            {
                return NotFound();
            }
            return View(reservacion);
        }

        // GET: Reservacion/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Reservacion/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Reservacion nuevaReservacion)
        {
            try
            {

                if (!ModelState.IsValid)
                {
                    return View(nuevaReservacion);
                }

                if (nuevaReservacion.FechaSalida <= nuevaReservacion.FechaInicio)
                {
                    ModelState.AddModelError("", "La fecha de salida debe ser posterior a la fecha de inicio.");
                    return View(nuevaReservacion);
                }

                bool exito = await _apiService.CrearAsync(nuevaReservacion);

                if (exito)
                {
                    TempData["SuccessMessage"] = "Reservación registrada con éxito.";
                    return RedirectToAction(nameof(Index));
                }

                ModelState.AddModelError("", "No se pudo completar la reserva. La habitación seleccionada ya se encuentra ocupada o reservada para las fechas indicadas.");
                return View(nuevaReservacion);
            }
            catch
            {
                ModelState.AddModelError("", "Ocurrió un error de comunicación con el servicio de reservaciones.");
                return View(nuevaReservacion);
            }
        }

        // GET: Reservacion/Edit/5
        public async Task<ActionResult> Edit(string id)
        {
            var reservacion = await _apiService.BuscarPorIdAsync(id);
            if (reservacion == null)
            {
                return NotFound();
            }

            return View(reservacion);
        }

        // POST: Reservacion/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(string id, Reservacion reservacionMod)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return View(reservacionMod);
                }

                if (reservacionMod.FechaSalida <= reservacionMod.FechaInicio)
                {
                    ModelState.AddModelError("", "La fecha de salida debe ser posterior a la fecha de inicio.");
                    return View(reservacionMod);
                }

                reservacionMod.ReservacionId = id;
                bool exito = await _apiService.ActualizarAsync(id, reservacionMod);

                if (exito)
                {
                    TempData["SuccessMessage"] = "Reservación modificada con éxito.";
                    return RedirectToAction(nameof(Index));
                }

                ModelState.AddModelError("", "No se pudo actualizar la reserva. Verifique que no exista un traslape de fechas con otra reservación.");
                return View(reservacionMod);
            }
            catch
            {
                ModelState.AddModelError("", "Ocurrió un error inesperado al actualizar el registro.");
                return View(reservacionMod);
            }
        }

        // GET: Reservacion/Delete/5
        public async Task<ActionResult> Delete(string id)
        {
            var reservacion = await _apiService.BuscarPorIdAsync(id);
            if (reservacion == null)
            {
                return NotFound();
            }

            return View(reservacion);
        }

        // POST: Reservacion/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Delete(string id, IFormCollection collection)
        {
            try
            {
                string? errorMsg = await _apiService.EliminarAsync(id);

                if (errorMsg == null)
                {
                    TempData["SuccessMessage"] = "Reservación eliminada con éxito.";
                    return RedirectToAction(nameof(Index));
                }

                TempData["ErrorMessage"] = errorMsg;
                var reservacion = await _apiService.BuscarPorIdAsync(id);
                return View(reservacion);
            }
            catch
            {
                TempData["ErrorMessage"] = "Ocurrió un error inesperado al intentar eliminar el registro.";
                var reservacion = await _apiService.BuscarPorIdAsync(id);
                return View(reservacion);
            }
        }

        // GET: Reservacion/Buscar
        [HttpGet]
        public async Task<IActionResult> Buscar(string criterioBusqueda)
        {
            var resultados = new List<Reservacion>();

            if (!string.IsNullOrEmpty(criterioBusqueda))
            {
                ViewData["CriterioActual"] = criterioBusqueda;
                var todas = await _apiService.ObtenerTodosAsync();
                resultados = todas.Where(r =>
                    r.ReservacionId.Contains(criterioBusqueda, StringComparison.OrdinalIgnoreCase) ||
                    r.ClienteId.Contains(criterioBusqueda, StringComparison.OrdinalIgnoreCase)).ToList();
            }

            return View(resultados);
        }
    }
}