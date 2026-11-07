using HospedajeAPI.Models;
using HospedajeAPI.Repositories.Interfaces;
using HospedajeAPI.Services;

namespace HospedajeAPI.Repositories
{
    public class HabitacionRepository : IHabitacionesRepository
    {
        public IEnumerable<Habitacion> ObtenerTodo()
        {
            // Retorna la lista global de habitaciones en la API
            return MemoryDatabase.Habitaciones;
        }

        public Habitacion? ObtenerPorId(string habitacionId)
        {
            if (string.IsNullOrEmpty(habitacionId)) return null;

            return MemoryDatabase.Habitaciones
                .FirstOrDefault(h => h.HabitacionId.Trim() == habitacionId.Trim());
        }

        public void Add(Habitacion habitacion)
        {
            if (habitacion == null) return;

            MemoryDatabase.Habitaciones.Add(habitacion);
        }

        public void Update(Habitacion habitacionActualizada)
        {
            if (habitacionActualizada == null) return;

            var habitacionExistente = ObtenerPorId(habitacionActualizada.HabitacionId);
            if (habitacionExistente != null)
            {
                // Copiamos las propiedades modificadas según tu modelo de Habitación
                habitacionExistente.TipoHabitacion = habitacionActualizada.TipoHabitacion;
                habitacionExistente.TarifaPorNoche = habitacionActualizada.TarifaPorNoche;
                habitacionExistente.TieneTvSatelital = habitacionActualizada.TieneTvSatelital;
                habitacionExistente.PendientesMantenimiento = habitacionActualizada.PendientesMantenimiento;
            }
        }

        public bool Delete(string habitacionId)
        {
            var habitacion = ObtenerPorId(habitacionId);
            if (habitacion != null)
            {
                return MemoryDatabase.Habitaciones.Remove(habitacion);
            }
            return false;
        }

        public bool TieneReservacionesAsociadas(string habitacionId)
        {
            if (string.IsNullOrEmpty(habitacionId)) return false;

            // Busca en la lista global de reservaciones si alguna coincide con esta habitación
            return MemoryDatabase.Reservaciones
                .Any(r => r.HabitacionId.Trim() == habitacionId.Trim());
        }
    }
}