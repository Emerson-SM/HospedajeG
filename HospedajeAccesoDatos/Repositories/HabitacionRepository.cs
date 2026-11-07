using HospedajeAccesoDatos.Context;
using HospedajeAccesoDatos.Models;
using HospedajeAccesoDatos.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace HospedajeAccesoDatos.Repositories
{
    public class HabitacionRepository : IHabitacionesRepository
    {
        private readonly HospedajeContext _context;

        public HabitacionRepository(HospedajeContext context)
        {
            _context = context;
        }

        // Obtener todas las habitaciones
        public IEnumerable<Habitacion> ObtenerTodo()
        {
            return _context.Habitaciones.ToList();
        }

        // Obtener una habitación por ID
        public Habitacion? ObtenerPorId(string habitacionId)
        {
            if (string.IsNullOrWhiteSpace(habitacionId))
                return null;

            return _context.Habitaciones
                .FirstOrDefault(h => h.HabitacionId == habitacionId);
        }

        // Agregar una habitación
        public void Add(Habitacion habitacion)
        {
            if (habitacion == null)
                return;

            _context.Habitaciones.Add(habitacion);
            _context.SaveChanges();
        }

        // Actualizar una habitación
        public void Update(Habitacion habitacionActualizada)
        {
            if (habitacionActualizada == null)
                return;

            var habitacionExistente = _context.Habitaciones.Find(habitacionActualizada.HabitacionId);

            if (habitacionExistente != null)
            {
                habitacionExistente.TipoHabitacion = habitacionActualizada.TipoHabitacion;
                habitacionExistente.TarifaPorNoche = habitacionActualizada.TarifaPorNoche;
                habitacionExistente.TieneTvSatelital = habitacionActualizada.TieneTvSatelital;
                habitacionExistente.PendientesMantenimiento = habitacionActualizada.PendientesMantenimiento;

                _context.SaveChanges();
            }
        }

        // Eliminar una habitación
        public bool Delete(string habitacionId)
        {
            var habitacion = _context.Habitaciones.Find(habitacionId);

            if (habitacion == null)
                return false;

            _context.Habitaciones.Remove(habitacion);
            _context.SaveChanges();

            return true;
        }

        // Verificar si la habitación tiene reservaciones asociadas
        public bool TieneReservacionesAsociadas(string habitacionId)
        {
            if (string.IsNullOrWhiteSpace(habitacionId))
                return false;

            return _context.Reservaciones
                .Any(r => r.HabitacionId == habitacionId);
        }
    }
}