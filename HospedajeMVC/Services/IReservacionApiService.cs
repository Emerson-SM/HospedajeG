using HospedajeMvc.Models;

namespace HospedajeMVC.Services
{
    public interface IReservacionApiService
    {
        Task<IEnumerable<Reservacion>> ObtenerTodosAsync();
        Task<Reservacion?> BuscarPorIdAsync(string reservacionId);
        Task<bool> CrearAsync(Reservacion reservacion);
        Task<bool> ActualizarAsync(string id, Reservacion reservacion);
        Task<string?> EliminarAsync(string id);
    }
}