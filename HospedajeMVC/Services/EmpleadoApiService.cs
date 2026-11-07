using HospedajeMvc.Models;
using System.Net.Http.Json;

namespace HospedajeMVC.Services
{
    public class EmpleadoApiService : IEmpleadoApiService
    {
        private readonly HttpClient _httpClient;

        public EmpleadoApiService(HttpClient httpClient, IConfiguration configuration)
        {
            _httpClient = httpClient;
            // Lee de forma automática la URL base configurada en el appsettings
            _httpClient.BaseAddress = new Uri(configuration["ApiSettings:BaseUrl"] ?? throw new ArgumentNullException("API URL no configurada"));
        }

        public async Task<IEnumerable<Empleado>> ObtenerTodosAsync()
        {
            var respuesta = await _httpClient.GetAsync("empleados");
            if (respuesta.IsSuccessStatusCode)
            {
                return await respuesta.Content.ReadFromJsonAsync<IEnumerable<Empleado>>() ?? Enumerable.Empty<Empleado>();
            }
            return Enumerable.Empty<Empleado>();
        }

        public async Task<Empleado?> BuscarPorIdAsync(string empleadoId)
        {
            var respuesta = await _httpClient.GetAsync($"empleados/{empleadoId}");
            if (respuesta.IsSuccessStatusCode)
            {
                return await respuesta.Content.ReadFromJsonAsync<Empleado>();
            }
            return null;
        }

        public async Task<bool> CrearAsync(Empleado empleado)
        {
            var respuesta = await _httpClient.PostAsJsonAsync("empleados", empleado);
            return respuesta.IsSuccessStatusCode;
        }

        public async Task<bool> ActualizarAsync(string id, Empleado empleado)
        {
            var respuesta = await _httpClient.PutAsJsonAsync($"empleados/{id}", empleado);
            return respuesta.IsSuccessStatusCode;
        }

        public async Task<string?> EliminarAsync(string id)
        {
            var respuesta = await _httpClient.DeleteAsync($"empleados/{id}");

            if (respuesta.IsSuccessStatusCode)
                return null; // Todo salió bien

            var errorMensaje = await respuesta.Content.ReadAsStringAsync();
            return !string.IsNullOrEmpty(errorMensaje) ? errorMensaje : "Ocurrió un error al intentar eliminar el empleado.";
        }
    }
}