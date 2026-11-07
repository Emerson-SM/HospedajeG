using System;
using System.ComponentModel.DataAnnotations;

namespace HospedajeAPI.Models
{
    public class Reservacion
    {
        [Display(Name = "Código de reservación")]
        public string ReservacionId { get; set; } = string.Empty;

        [Required(ErrorMessage = "La identificación del cliente es obligatoria.")]
        [Display(Name = "Identificación del cliente")]
        public string ClienteId { get; set; } = string.Empty;

        [Required(ErrorMessage = "El número de habitación es obligatorio.")]
        [Display(Name = "Número de habitación")]
        public string HabitacionId { get; set; } = string.Empty;

        [Required(ErrorMessage = "La fecha de registro es obligatoria.")]
        [DataType(DataType.Date)]
        [Display(Name = "Fecha de registro")]
        public DateTime FechaRegistro { get; set; } = DateTime.Now;

        [Required(ErrorMessage = "La fecha de inicio es obligatoria.")]
        [DataType(DataType.Date)]
        [Display(Name = "Fecha de inicio")]
        public DateTime FechaInicio { get; set; }

        [Required(ErrorMessage = "La fecha de salida es obligatoria.")]
        [DataType(DataType.Date)]
        [Display(Name = "Fecha de salida")]
        public DateTime FechaSalida { get; set; }

        [Required(ErrorMessage = "La cantidad de personas es obligatoria.")]
        [Range(1, 10, ErrorMessage = "La cantidad de personas debe ser entre 1 y 10.")]
        [Display(Name = "Cantidad de personas")]
        public int CantidadPersonas { get; set; }

        [StringLength(300, ErrorMessage = "Las solicitudes especiales no pueden superar los 300 caracteres.")]
        [Display(Name = "Solicitudes especiales")]
        public string SolicitudesEspeciales { get; set; } = string.Empty;

        [Required(ErrorMessage = "El porcentaje de descuento es obligatorio.")]
        [Range(0, 100, ErrorMessage = "El descuento debe estar entre 0 y 100.")]
        [Display(Name = "Porcentaje de descuento")]
        public decimal PorcentajeDescuento { get; set; }

        [Required(ErrorMessage = "El estado es obligatorio.")]
        public string Estado { get; set; } = "Reservada";

        [Display(Name = "Tarifa de la reservación")]
        public decimal TarifaReservacion { get; set; }

        [Display(Name = "Monto total")]
        public decimal MontoTotal { get; set; }
    }
}