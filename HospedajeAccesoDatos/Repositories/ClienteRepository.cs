using HospedajeAccesoDatos.Context;
using HospedajeAccesoDatos.Models;
using HospedajeAccesoDatos.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace HospedajeAccesoDatos.Repositories
{
    public class ClienteRepository : IClienteRepository
    {
        private readonly HospedajeContext _context;

        public ClienteRepository(HospedajeContext context)
        {
            _context = context;
        }

        // Obtener todos los clientes
        public IEnumerable<Cliente> ObtenerTodo()
        {
            return _context.Clientes.ToList();
        }

        // Obtener un cliente por ID
        public Cliente? ObtenerPorId(string clienteId)
        {
            return _context.Clientes
                .FirstOrDefault(c => c.ClienteId == clienteId);
        }

        // Agregar un cliente
        public void Add(Cliente cliente)
        {
            _context.Clientes.Add(cliente);
            _context.SaveChanges();
        }

        // Actualizar un cliente
        public void Update(Cliente clienteActualizado)
        {
            var existente = _context.Clientes.Find(clienteActualizado.ClienteId);

            if (existente != null)
            {
                existente.TipoIdentificacion = clienteActualizado.TipoIdentificacion;
                existente.NombreCliente = clienteActualizado.NombreCliente;
                existente.PrimerApellidoCliente = clienteActualizado.PrimerApellidoCliente;
                existente.SegundoApellidoCliente = clienteActualizado.SegundoApellidoCliente;
                existente.FechaNacimientoCliente = clienteActualizado.FechaNacimientoCliente;

                _context.SaveChanges();
            }
        }

        // Eliminar un cliente
        public bool Delete(string clienteId)
        {
            var cliente = _context.Clientes.Find(clienteId);

            if (cliente == null)
                return false;

            _context.Clientes.Remove(cliente);
            _context.SaveChanges();

            return true;
        }

        // Verificar si el cliente tiene reservaciones asociadas
        public bool TieneReservacionesAsociadas(string clienteId)
        {
            return _context.Reservaciones
                .Any(r => r.ClienteId == clienteId);
        }
    }
}