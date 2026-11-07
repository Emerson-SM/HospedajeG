using HospedajeMvc.Models;
using System.Net.Http.Json;

namespace HospedajeMVC.Services
{
    public class HabitacionApiService : IHabitacionApiService
    {
        private readonly HttpClient _httpClient;

        public HabitacionApiService(HttpClient httpClient, IConfiguration configuration)
        {
            _httpClient = httpClient;
            // Lee de forma automática la URL base configurada en el appsettings
            _httpClient.BaseAddress = new Uri(configuration["ApiSettings:BaseUrl"] ?? throw new ArgumentNullException("API URL no configurada"));
        }

        public async Task<IEnumerable<Habitacion>> ObtenerTodosAsync()
        {
            var respuesta = await _httpClient.GetAsync("habitaciones");
            if (respuesta.IsSuccessStatusCode)
            {
                return await respuesta.Content.ReadFromJsonAsync<IEnumerable<Habitacion>>() ?? Enumerable.Empty<Habitacion>();
            }
            return Enumerable.Empty<Habitacion>();
        }

        public async Task<Habitacion?> BuscarPorIdAsync(string habitacionId)
        {
            var respuesta = await _httpClient.GetAsync($"habitaciones/buscar?habitacionId={habitacionId}");
            if (respuesta.IsSuccessStatusCode)
            {
                return await respuesta.Content.ReadFromJsonAsync<Habitacion>();
            }
            return null;
        }

        public async Task<bool> CrearAsync(Habitacion habitacion)
        {
            var respuesta = await _httpClient.PostAsJsonAsync("habitaciones", habitacion);
            return respuesta.IsSuccessStatusCode;
        }

        public async Task<bool> ActualizarAsync(string id, Habitacion habitacion)
        {
            var respuesta = await _httpClient.PutAsJsonAsync($"habitaciones/{id}", habitacion);
            return respuesta.IsSuccessStatusCode;
        }

        public async Task<string?> EliminarAsync(string id)
        {
            var respuesta = await _httpClient.DeleteAsync($"habitaciones/{id}");

            if (respuesta.IsSuccessStatusCode)
                return null; // Todo salió bien

            var errorMensaje = await respuesta.Content.ReadAsStringAsync();
            return !string.IsNullOrEmpty(errorMensaje) ? errorMensaje : "Ocurrió un error al intentar eliminar la habitacion.";
        }
    }
}