using HospedajeMvc.Models;
using System.Net.Http.Json;

namespace HospedajeMVC.Services
{
    public class ReservacionApiService : IReservacionApiService
    {
        private readonly HttpClient _httpClient;

        public ReservacionApiService(HttpClient httpClient, IConfiguration configuration)
        {
            _httpClient = httpClient;
            // Lee de forma automática la URL base configurada en la appsettings
            _httpClient.BaseAddress = new Uri(configuration["ApiSettings:BaseUrl"] ?? throw new ArgumentNullException("API URL no configurada"));
        }

        public async Task<IEnumerable<Reservacion>> ObtenerTodosAsync()
        {
            // Apunta a: GET api/reservaciones
            var respuesta = await _httpClient.GetAsync("reservaciones");
            if (respuesta.IsSuccessStatusCode)
            {
                return await respuesta.Content.ReadFromJsonAsync<IEnumerable<Reservacion>>() ?? Enumerable.Empty<Reservacion>();
            }
            return Enumerable.Empty<Reservacion>();
        }

        public async Task<Reservacion?> BuscarPorIdAsync(string reservacionId)
        {
            // Apunta a: GET api/reservaciones/RES-0001
            var respuesta = await _httpClient.GetAsync($"reservaciones/{reservacionId}");
            if (respuesta.IsSuccessStatusCode)
            {
                return await respuesta.Content.ReadFromJsonAsync<Reservacion>();
            }
            return null;
        }

        public async Task<bool> CrearAsync(Reservacion reservacion)
        {
            // Apunta a: POST api/reservaciones
            var respuesta = await _httpClient.PostAsJsonAsync("reservaciones", reservacion);
            return respuesta.IsSuccessStatusCode;
        }

        public async Task<bool> ActualizarAsync(string id, Reservacion reservacion)
        {
            // Apunta a: PUT api/reservaciones/{id}
            var respuesta = await _httpClient.PutAsJsonAsync($"reservaciones/{id}", reservacion);
            return respuesta.IsSuccessStatusCode;
        }

        public async Task<string?> EliminarAsync(string id)
        {
            // Apunta a: DELETE api/reservaciones/{id}
            var respuesta = await _httpClient.DeleteAsync($"reservaciones/{id}");

            if (respuesta.IsSuccessStatusCode)
                return null; // Todo salió bien

            var errorMensaje = await respuesta.Content.ReadAsStringAsync();
            return !string.IsNullOrEmpty(errorMensaje) ? errorMensaje : "Ocurrió un error al intentar eliminar la reservacion.";
        }
    }
}