using HospedajeAccesoDatos.Context;
using HospedajeAccesoDatos.Models;
using HospedajeAccesoDatos.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace HospedajeAccesoDatos.Repositories
{
    public class EmpleadoRepository : IEmpleadoRepository
    {
        private readonly HospedajeContext _context;

        public EmpleadoRepository(HospedajeContext context)
        {
            _context = context;
        }

        // Obtener todos los empleados
        public IEnumerable<Empleado> ObtenerTodo()
        {
            return _context.Empleados.ToList();
        }

        // Obtener un empleado por ID
        public Empleado? ObtenerPorId(string empleadoId)
        {
            if (string.IsNullOrWhiteSpace(empleadoId))
                return null;

            return _context.Empleados
                .FirstOrDefault(e => e.EmpleadoId == empleadoId);
        }

        // Agregar un empleado
        public void Add(Empleado empleado)
        {
            if (empleado == null)
                return;

            _context.Empleados.Add(empleado);
            _context.SaveChanges();
        }

        // Actualizar un empleado
        public void Update(Empleado empleadoActualizado)
        {
            if (empleadoActualizado == null)
                return;

            var empleadoExistente = _context.Empleados.Find(empleadoActualizado.EmpleadoId);

            if (empleadoExistente != null)
            {
                empleadoExistente.TipoIdentificacion = empleadoActualizado.TipoIdentificacion;
                empleadoExistente.NombreEmpleado = empleadoActualizado.NombreEmpleado;
                empleadoExistente.PrimerApellidoEmpleado = empleadoActualizado.PrimerApellidoEmpleado;
                empleadoExistente.SegundoApellidoEmpleado = empleadoActualizado.SegundoApellidoEmpleado;
                empleadoExistente.FechaNacimientoEmpleado = empleadoActualizado.FechaNacimientoEmpleado;
                empleadoExistente.SalarioMensual = empleadoActualizado.SalarioMensual;
                empleadoExistente.FechaIngreso = empleadoActualizado.FechaIngreso;
                empleadoExistente.Categoria = empleadoActualizado.Categoria;
                empleadoExistente.Provincia = empleadoActualizado.Provincia;
                empleadoExistente.Canton = empleadoActualizado.Canton;
                empleadoExistente.Distrito = empleadoActualizado.Distrito;
                empleadoExistente.DireccionExacta = empleadoActualizado.DireccionExacta;

                _context.SaveChanges();
            }
        }

        // Eliminar un empleado
        public bool Delete(string empleadoId)
        {
            var empleado = _context.Empleados.Find(empleadoId);

            if (empleado == null)
                return false;

            _context.Empleados.Remove(empleado);
            _context.SaveChanges();

            return true;
        }
    }
}