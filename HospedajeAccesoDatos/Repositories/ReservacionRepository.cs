using HospedajeAccesoDatos.Context;
using HospedajeAccesoDatos.Models;
using HospedajeAccesoDatos.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace HospedajeAccesoDatos.Repositories
{
    public class ReservacionRepository : IReservacionesRepository
    {
        private readonly HospedajeContext _context;

        public ReservacionRepository(HospedajeContext context)
        {
            _context = context;
        }

        public IEnumerable<Reservacion> ObtenerTodo()
        {
            return _context.Reservaciones.ToList();
        }

        public Reservacion? ObtenerPorId(string reservacionId)
        {
            if (string.IsNullOrWhiteSpace(reservacionId))
                return null;

            return _context.Reservaciones
                .FirstOrDefault(r => r.ReservacionId == reservacionId);
        }

        public void Add(Reservacion reservacion)
        {
            if (reservacion == null)
                return;

            // Validar traslapes
            if (ExisteTraslape(
                reservacion.HabitacionId,
                reservacion.FechaInicio,
                reservacion.FechaSalida,
                null))
            {
                throw new InvalidOperationException(
                    "La habitación seleccionada ya se encuentra ocupada o reservada para las fechas indicadas.");
            }

            CalcularDetallesFinancieros(reservacion);

            _context.Reservaciones.Add(reservacion);
            _context.SaveChanges();
        }

        public void Update(Reservacion reservacionActualizada)
        {
            if (reservacionActualizada == null)
                return;

            var reservacionExistente = _context.Reservaciones.Find(reservacionActualizada.ReservacionId);

            if (reservacionExistente == null)
                throw new InvalidOperationException("La reservación no existe.");

            // Validar que el cliente exista
            bool clienteExiste = _context.Clientes.Any(c =>
                c.ClienteId == reservacionActualizada.ClienteId);

            if (!clienteExiste)
            {
                throw new InvalidOperationException(
                    "El cliente seleccionado no existe.");
            }

            // Validar que la habitación exista
            bool habitacionExiste = _context.Habitaciones.Any(h =>
                h.HabitacionId == reservacionActualizada.HabitacionId);

            if (!habitacionExiste)
            {
                throw new InvalidOperationException(
                    "La habitación seleccionada no existe.");
            }

            // Validar traslape de fechas
            if (ExisteTraslape(
                reservacionActualizada.HabitacionId,
                reservacionActualizada.FechaInicio,
                reservacionActualizada.FechaSalida,
                reservacionActualizada.ReservacionId))
            {
                throw new InvalidOperationException(
                    "No es posible actualizar. La habitación presenta un traslape de fechas con otra reservación existente.");
            }

            // Actualizar datos
            reservacionExistente.ClienteId = reservacionActualizada.ClienteId;
            reservacionExistente.HabitacionId = reservacionActualizada.HabitacionId;
            reservacionExistente.FechaInicio = reservacionActualizada.FechaInicio;
            reservacionExistente.FechaSalida = reservacionActualizada.FechaSalida;
            reservacionExistente.CantidadPersonas = reservacionActualizada.CantidadPersonas;
            reservacionExistente.PorcentajeDescuento = reservacionActualizada.PorcentajeDescuento;
            reservacionExistente.SolicitudesEspeciales = reservacionActualizada.SolicitudesEspeciales;
            reservacionExistente.Estado = reservacionActualizada.Estado;

            // Recalcular montos
            CalcularDetallesFinancieros(reservacionExistente);

            _context.SaveChanges();
        }

        public bool Delete(string reservacionId)
        {
            var reservacion = _context.Reservaciones.Find(reservacionId);

            if (reservacion == null)
                return false;

            _context.Reservaciones.Remove(reservacion);
            _context.SaveChanges();

            return true;
        }

        #region Métodos Auxiliares

        private bool ExisteTraslape(
            string habitacionId,
            DateTime inicio,
            DateTime fin,
            string? reservacionIdActual)
        {
            if (string.IsNullOrWhiteSpace(habitacionId))
                return false;

            return _context.Reservaciones.Any(r =>
                r.HabitacionId == habitacionId &&
                r.ReservacionId != reservacionIdActual &&
                r.Estado != "Cancelada" &&
                (
                    (inicio >= r.FechaInicio && inicio < r.FechaSalida) ||
                    (fin > r.FechaInicio && fin <= r.FechaSalida) ||
                    (inicio <= r.FechaInicio && fin >= r.FechaSalida)
                ));
        }

        private void CalcularDetallesFinancieros(Reservacion r)
        {
            var habitacion = _context.Habitaciones
                .FirstOrDefault(h => h.HabitacionId == r.HabitacionId);

            if (habitacion != null)
            {
                r.TarifaReservacion = habitacion.TarifaPorNoche;
            }

            int nochesCalculadas = (r.FechaSalida.Date - r.FechaInicio.Date).Days;
            int noches = nochesCalculadas < 1 ? 1 : nochesCalculadas;

            decimal subtotal = noches * r.TarifaReservacion;
            decimal montoDescuento = subtotal * (r.PorcentajeDescuento / 100m);
            decimal montoImpuesto = (subtotal - montoDescuento) * 0.13m;

            r.MontoTotal = (subtotal - montoDescuento) + montoImpuesto;
        }

        #endregion
    }
}