using System.ComponentModel.DataAnnotations;

namespace HospedajeAPI.Models
{
    public class Reservacion
    {
        [Required(ErrorMessage = "El código de reservación es obligatorio.")]
        [StringLength(20, MinimumLength = 1, ErrorMessage = "El código debe tener entre 1 y 20 caracteres.")]
        [Display(Name = "Número de reservación")]
        public string ReservacionId { get; set; } = string.Empty;

        [Required(ErrorMessage = "La identificación del cliente es obligatoria.")]
        [Display(Name = "Identificación del cliente")]
        public string ClienteId { get; set; } = string.Empty;

        [Required(ErrorMessage = "El número de habitación es obligatorio.")]
        [Display(Name = "Número de habitación")]
        public string HabitacionId { get; set; } = string.Empty;

        [Required(ErrorMessage = "La fecha de ingreso es obligatoria.")]
        [DataType(DataType.Date)]
        [Display(Name = "Fecha de ingreso")]
        public DateTime FechaIngreso { get; set; }

        [Required(ErrorMessage = "La fecha de salida es obligatoria.")]
        [DataType(DataType.Date)]
        [Display(Name = "Fecha de salida")]
        public DateTime FechaSalida { get; set; }

        [Required(ErrorMessage = "La cantidad de personas es obligatoria.")]
        [Range(1, 10, ErrorMessage = "La cantidad de personas debe ser entre 1 y 10.")]
        [Display(Name = "Cantidad de personas")]
        public int CantidadPersonas { get; set; }

        [Required(ErrorMessage = "El estado es obligatorio.")]
        public string Estado { get; set; } = string.Empty;
    }
}
