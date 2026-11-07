using HospedajeMvc.Models;

namespace HospedajeMVC.Services
{
    public interface IClienteApiService
    {
        Task<IEnumerable<Cliente>> ObtenerTodosAsync();
        Task<Cliente?> BuscarPorIdAsync(string clienteId);
        Task<bool> CrearAsync(Cliente cliente);
        Task<bool> ActualizarAsync(string id, Cliente cliente);
        Task<string?> EliminarAsync(string id); 
    }
}