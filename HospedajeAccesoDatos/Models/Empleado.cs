using System.ComponentModel.DataAnnotations;

namespace HospedajeAccesoDatos.Models
{
    public class Empleado
    {
        [Required(ErrorMessage = "El tipo de identificación es obligatorio.")]
        [Display(Name = "Tipo de identificación")]
        public string TipoIdentificacion { get; set; } = string.Empty;

        [Required(ErrorMessage = "La identificación es obligatoria.")]
        [Display(Name = "Número de identificación")]
        public string EmpleadoId { get; set; } = string.Empty;

        [Required(ErrorMessage = "El nombre es obligatorio.")]
        [Display(Name = "Nombre")]
        [StringLength(50, MinimumLength = 3, ErrorMessage = "El nombre debe tener entre 3 y 50 caracteres.")]
        public string NombreEmpleado { get; set; } = string.Empty;

        [Required(ErrorMessage = "El primer apellido es obligatorio.")]
        [Display(Name = "Primer apellido")]
        [StringLength(75, MinimumLength = 3, ErrorMessage = "El primer apellido debe tener entre 3 y 75 caracteres.")]
        public string PrimerApellidoEmpleado { get; set; } = string.Empty;

        [Required(ErrorMessage = "El segundo apellido es obligatorio.")]
        [Display(Name = "Segundo apellido")]
        [StringLength(75, MinimumLength = 3, ErrorMessage = "El segundo apellido debe tener entre 3 y 75 caracteres.")]
        public string SegundoApellidoEmpleado { get; set; } = string.Empty;

        [Required(ErrorMessage = "La fecha de nacimiento es obligatoria.")]
        [DataType(DataType.Date)]
        [Display(Name = "Fecha de nacimiento")]
        public DateTime FechaNacimientoEmpleado { get; set; }

        [Required(ErrorMessage = "El salario mensual es obligatorio.")]
        [Range(0, 5000000, ErrorMessage = "El salario debe estar entre 0 y 5,000,000.")]
        [Display(Name = "Salario mensual")]
        public int SalarioMensual { get; set; }

        [Required(ErrorMessage = "La fecha de ingreso es obligatoria.")]
        [DataType(DataType.Date)]
        [Display(Name = "Fecha de ingreso")]
        public DateTime FechaIngreso { get; set; }

        [Required(ErrorMessage = "La categoría es obligatoria.")]
        public string Categoria { get; set; } = string.Empty;

        // Ubicación territorial de Costa Rica
        [Required(ErrorMessage = "La provincia es obligatoria.")]
        public string Provincia { get; set; } = string.Empty;

        [Required(ErrorMessage = "El cantón es obligatorio.")]
        [Display(Name = "Cantón")]
        public string Canton { get; set; } = string.Empty;

        [Required(ErrorMessage = "El distrito es obligatorio.")]
        public string Distrito { get; set; } = string.Empty;

        [Required(ErrorMessage = "La dirección exacta es obligatoria.")]
        [Display(Name = "Dirección exacta")]
        [StringLength(150, MinimumLength = 1, ErrorMessage = "La dirección no puede superar los 150 caracteres.")]
        public string DireccionExacta { get; set; } = string.Empty;
    }
}