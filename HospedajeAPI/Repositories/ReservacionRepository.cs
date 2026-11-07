using HospedajeAPI.Models;
using HospedajeAPI.Repositories.Interfaces;
using HospedajeAPI.Services;

namespace HospedajeAPI.Repositories
{
    public class ReservacionRepository : IReservacionesRepository
    {
        public IEnumerable<Reservacion> ObtenerTodo()
        {
            return MemoryDatabase.Reservaciones;
        }

        public Reservacion? ObtenerPorId(string reservacionId)
        {
            if (string.IsNullOrEmpty(reservacionId)) return null;

            return MemoryDatabase.Reservaciones
                .FirstOrDefault(r => r.ReservacionId.Trim().Equals(reservacionId.Trim(), StringComparison.OrdinalIgnoreCase));
        }

        public void Add(Reservacion reservacion)
        {
            if (reservacion == null) return;

            // 1. Validar traslapes antes de insertar (Usando FechaInicio)
            if (ExisteTraslape(reservacion.HabitacionId, reservacion.FechaInicio, reservacion.FechaSalida, null))
            {
                throw new InvalidOperationException("La habitación seleccionada ya se encuentra ocupada o reservada para las fechas indicadas.");
            }

            // 2. Ejecutar los cálculos económicos y de fechas
            CalcularDetallesFinancieros(reservacion);

            MemoryDatabase.Reservaciones.Add(reservacion);
        }

        public void Update(Reservacion reservacionActualizada)
        {
            if (reservacionActualizada == null) return;

            var reservacionExistente = ObtenerPorId(reservacionActualizada.ReservacionId);
            if (reservacionExistente != null)
            {
                // 1. Validar traslapes excluyendo la misma reservación que se está editando
                if (ExisteTraslape(reservacionActualizada.HabitacionId, reservacionActualizada.FechaInicio, reservacionActualizada.FechaSalida, reservacionActualizada.ReservacionId))
                {
                    throw new InvalidOperationException("No es posible actualizar. La habitación presenta un traslape de fechas con otra reservación existente.");
                }

                // 2. Mapear datos base
                reservacionExistente.ClienteId = reservacionActualizada.ClienteId;
                reservacionExistente.HabitacionId = reservacionActualizada.HabitacionId;

                // CORREGIDO: Uso de FechaInicio
                reservacionExistente.FechaInicio = reservacionActualizada.FechaInicio;
                reservacionExistente.FechaSalida = reservacionActualizada.FechaSalida;

                reservacionExistente.CantidadPersonas = reservacionActualizada.CantidadPersonas;
                reservacionExistente.PorcentajeDescuento = reservacionActualizada.PorcentajeDescuento;
                reservacionExistente.SolicitudesEspeciales = reservacionActualizada.SolicitudesEspeciales;
                reservacionExistente.Estado = reservacionActualizada.Estado;

                // 3. Recalcular métricas automáticas tras la edición de datos
                CalcularDetallesFinancieros(reservacionExistente);
            }
        }

        public bool Delete(string reservacionId)
        {
            var reservacion = ObtenerPorId(reservacionId);
            if (reservacion != null)
            {
                return MemoryDatabase.Reservaciones.Remove(reservacion);
            }
            return false;
        }

        #region Métodos Auxiliares de Lógica de Negocio

        private bool ExisteTraslape(string habitacionId, DateTime inicio, DateTime fin, string? reservacionIdActual)
        {
            if (string.IsNullOrEmpty(habitacionId)) return false;

            return MemoryDatabase.Reservaciones.Any(r =>
                r.HabitacionId != null &&
                r.HabitacionId.Trim().Equals(habitacionId.Trim(), StringComparison.OrdinalIgnoreCase) &&
                r.ReservacionId != reservacionIdActual &&
                r.Estado != "Cancelada" &&
                ((inicio >= r.FechaInicio && inicio < r.FechaSalida) ||
                 (fin > r.FechaInicio && fin <= r.FechaSalida) ||
                 (inicio <= r.FechaInicio && fin >= r.FechaSalida))
            );
        }

        private void CalcularDetallesFinancieros(Reservacion r)
        {
            // 1. Buscamos la habitación en la base de datos en memoria para obtener su precio por noche
            var habitacion = MemoryDatabase.Habitaciones
                .FirstOrDefault(h => h.HabitacionId.Trim().Equals(r.HabitacionId.Trim(), StringComparison.OrdinalIgnoreCase));

            if (habitacion != null)
            {
                // Guardamos el precio por noche en la propiedad que tiene tu modelo
                r.TarifaReservacion = habitacion.TarifaPorNoche;
            }

            // 2. Calculamos las noches de forma local solo para hacer la matemática
            int nochesCalculadas = (r.FechaSalida.Date - r.FechaInicio.Date).Days;
            int noches = nochesCalculadas < 1 ? 1 : nochesCalculadas;

            // 3. Hacemos las operaciones usando variables locales temporales
            decimal subtotal = noches * r.TarifaReservacion;
            decimal montoDescuento = subtotal * (r.PorcentajeDescuento / 100m);
            decimal montoImpuesto = (subtotal - montoDescuento) * 0.13m; // 13% de IVA

            // 4. Asignamos el resultado final a la propiedad que sí existe en tu modelo
            r.MontoTotal = (subtotal - montoDescuento) + montoImpuesto;
        }

        #endregion
    }
}