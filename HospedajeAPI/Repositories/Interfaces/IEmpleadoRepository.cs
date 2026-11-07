using HospedajeAPI.Models;

namespace HospedajeAPI.Repositories.Interfaces
{
    public interface IEmpleadoRepository
    {
        IEnumerable<Empleado> ObtenerTodo();
        Empleado? ObtenerPorId(string empleadoId);
        void Add(Empleado empleado);
        void Update(Empleado empleado);
        bool Delete(string empleadoId);
    }
}
