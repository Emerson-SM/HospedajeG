using HospedajeAPI.Models;

namespace HospedajeAPI.Repositories.Interfaces
{
    public interface IClienteRepository
    {
        IEnumerable<Cliente> ObtenerTodo();
        Cliente? ObtenerPorId(string clienteId);
        void Add(Cliente cliente);
        void Update(Cliente cliente);
        bool Delete(string clienteId);
        bool TieneReservacionesAsociadas(string clienteId);
    }
}