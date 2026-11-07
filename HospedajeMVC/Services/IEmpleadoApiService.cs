using HospedajeMvc.Models;

namespace HospedajeMVC.Services
{
    public interface IEmpleadoApiService
    {
        Task<IEnumerable<Empleado>> ObtenerTodosAsync();
        Task<Empleado?> BuscarPorIdAsync(string empleadoId);
        Task<bool> CrearAsync(Empleado empleado);
        Task<bool> ActualizarAsync(string id, Empleado empleado);
        Task<string?> EliminarAsync(string id);
    }
}