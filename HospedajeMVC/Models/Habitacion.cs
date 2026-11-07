using System.ComponentModel.DataAnnotations;

namespace HospedajeMvc.Models
{
    public class Habitacion
    {
        [Required(ErrorMessage = "El número de habitación es obligatorio.")]
        [Display(Name = "Número de habitación")]
        [Range(1, 500, ErrorMessage = "El número de habitación debe estar en el rango de 1 a 500.")]
        public string HabitacionId { get; set; } = string.Empty;

        [Required(ErrorMessage = "El tipo de habitación es obligatorio.")]
        [Display(Name = "Tipo de habitación")]
        public string TipoHabitacion { get; set; } = string.Empty;

        [Required(ErrorMessage = "La tarifa por noche es obligatoria.")]
        [Display(Name = "Tarifa por noche")]
        [Range(50, 800, ErrorMessage = "La tarifa debe estar entre 50 y 800 dólares.")]
        public int TarifaPorNoche { get; set; }

        [Required]
        [Display(Name = "TV Satelital")]
        public bool TieneTvSatelital { get; set; }

        [StringLength(500, ErrorMessage = "Los pendientes de mantenimiento no pueden superar los 500 caracteres.")]
        [Display(Name = "¿Necesita mantenimiento?")]
        public string PendientesMantenimiento { get; set; } = string.Empty;
    }
}
