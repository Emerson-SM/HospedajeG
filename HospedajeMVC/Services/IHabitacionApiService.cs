using HospedajeMvc.Models;

namespace HospedajeMVC.Services
{
    public interface IHabitacionApiService
    {
        Task<IEnumerable<Habitacion>> ObtenerTodosAsync();
        Task<Habitacion?> BuscarPorIdAsync(string habitacionId);
        Task<bool> CrearAsync(Habitacion habitacion);
        Task<bool> ActualizarAsync(string id, Habitacion habitacion);
        Task<string?> EliminarAsync(string id);
    }
}