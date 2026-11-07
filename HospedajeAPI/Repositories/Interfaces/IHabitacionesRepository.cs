using HospedajeAPI.Models;

namespace HospedajeAPI.Repositories.Interfaces
{
    public interface IHabitacionesRepository
    {
        IEnumerable<Habitacion> ObtenerTodo();
        Habitacion? ObtenerPorId(string habitacionId);
        void Add(Habitacion habitacion);
        void Update(Habitacion habitacion);
        bool Delete(string habitacionId);
        bool TieneReservacionesAsociadas(string habitacionId);
    }
}
