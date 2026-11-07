using HospedajeMvc.Models;
using System.Net.Http.Json;

namespace HospedajeMVC.Services
{
    public class ClienteApiService : IClienteApiService
    {
        private readonly HttpClient _httpClient;

        public ClienteApiService(HttpClient httpClient, IConfiguration configuration)
        {
            _httpClient = httpClient;
            // Lee de forma automática la URL base configurada en el appsettings
            _httpClient.BaseAddress = new Uri(configuration["ApiSettings:BaseUrl"] ?? throw new ArgumentNullException("API URL no configurada"));
        }

        public async Task<IEnumerable<Cliente>> ObtenerTodosAsync()
        {
            var respuesta = await _httpClient.GetAsync("clientes");
            if (respuesta.IsSuccessStatusCode)
            {
                return await respuesta.Content.ReadFromJsonAsync<IEnumerable<Cliente>>() ?? Enumerable.Empty<Cliente>();
            }
            return Enumerable.Empty<Cliente>();
        }

        public async Task<Cliente?> BuscarPorIdAsync(string clienteId)
        {
            var respuesta = await _httpClient.GetAsync($"clientes/{clienteId}");
            if (respuesta.IsSuccessStatusCode)
            {
                return await respuesta.Content.ReadFromJsonAsync<Cliente>();
            }
            return null;
        }

        public async Task<bool> CrearAsync(Cliente cliente)
        {
            var respuesta = await _httpClient.PostAsJsonAsync("clientes", cliente);
            return respuesta.IsSuccessStatusCode;
        }

        public async Task<bool> ActualizarAsync(string id, Cliente cliente)
        {
            var respuesta = await _httpClient.PutAsJsonAsync($"clientes/{id}", cliente);
            return respuesta.IsSuccessStatusCode;
        }

        public async Task<string?> EliminarAsync(string id)
        {
            var respuesta = await _httpClient.DeleteAsync($"clientes/{id}");

            if (respuesta.IsSuccessStatusCode)
                return null; // Todo salió bien

            var errorMensaje = await respuesta.Content.ReadAsStringAsync();
            return !string.IsNullOrEmpty(errorMensaje) ? errorMensaje : "Ocurrió un error al intentar eliminar el cliente.";
        }
    }
}