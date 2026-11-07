using HospedajeAPI.Models;

namespace HospedajeAPI.Repositories.Interfaces
{
    public interface IReservacionesRepository
    {
        IEnumerable<Reservacion> ObtenerTodo();
        Reservacion? ObtenerPorId(string reservacionId);
        void Add(Reservacion reservacion);
        void Update(Reservacion reservacion);
        bool Delete(string reservacionId);
    }
}
