using HospedajeAPI.Models;
using HospedajeAPI.Repositories.Interfaces;
using HospedajeAPI.Services; 

namespace HospedajeAPI.Repositories
{
    public class EmpleadoRepository : IEmpleadoRepository
    {
        public IEnumerable<Empleado> ObtenerTodo()
        {
            return MemoryDatabase.Empleados;
        }

        public Empleado? ObtenerPorId(string empleadoId)
        {
            if (string.IsNullOrEmpty(empleadoId)) return null;

            return MemoryDatabase.Empleados
                .FirstOrDefault(e => e.EmpleadoId.Trim() == empleadoId.Trim());
        }

        public void Add(Empleado empleado)
        {
            if (empleado == null) return;

            MemoryDatabase.Empleados.Add(empleado);
        }

        public void Update(Empleado empleadoActualizado)
        {
            if (empleadoActualizado == null) return;

            var empleadoExistente = ObtenerPorId(empleadoActualizado.EmpleadoId);
            if (empleadoExistente != null)
            {
                // Copiamos los valores modificados al objeto en memoria
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
            }
        }

        public bool Delete(string empleadoId)
        {
            var empleado = ObtenerPorId(empleadoId);
            if (empleado != null)
            {
                return MemoryDatabase.Empleados.Remove(empleado);
            }
            return false;
        }
    }
}