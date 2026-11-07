using HospedajeAPI.Models;
using HospedajeAPI.Services;
using HospedajeAPI.Repositories.Interfaces;

namespace HospedajeAPI.Repositories
{
    public class ClienteRepository : IClienteRepository
    {
        public IEnumerable<Cliente> ObtenerTodo()
        {
            return MemoryDatabase.Clientes;
        }

        public Cliente? ObtenerPorId(string clienteId)
        {
            return MemoryDatabase.Clientes
                .FirstOrDefault(c => c.ClienteId.Equals(clienteId, StringComparison.OrdinalIgnoreCase));
        }

        public void Add(Cliente cliente)
        {
            MemoryDatabase.Clientes.Add(cliente);
        }

        public void Update(Cliente clienteActualizado)
        {
            var existente = ObtenerPorId(clienteActualizado.ClienteId);
            if (existente != null)
            {
                existente.NombreCliente = clienteActualizado.NombreCliente;
                existente.PrimerApellidoCliente = clienteActualizado.PrimerApellidoCliente;
                existente.SegundoApellidoCliente = clienteActualizado.SegundoApellidoCliente;
                existente.FechaNacimientoCliente = clienteActualizado.FechaNacimientoCliente;
            }
        }

        public bool Delete(string clienteId)
        {
            var existente = ObtenerPorId(clienteId);
            if (existente != null)
            {
                return MemoryDatabase.Clientes.Remove(existente);
            }
            return false;
        }

        public bool TieneReservacionesAsociadas(string clienteId)
        {
            return MemoryDatabase.Reservaciones.Any(r =>
                r.ClienteId.Equals(clienteId, StringComparison.OrdinalIgnoreCase));
        }
    }
}