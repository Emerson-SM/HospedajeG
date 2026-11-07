using System.ComponentModel.DataAnnotations;

namespace HospedajeAPI.Models
{
    public class Cliente
    {
        [Required(ErrorMessage = "La identificación es obligatoria.")]
        [Display(Name = "Cédula o Pasaporte")]
        [RegularExpression(@"^(\d-\d{4}-\d{4}|\d{12}|[a-zA-Z0-9]{6,15})$",
        ErrorMessage = "El formato de identificación no es válido. Use X-XXXX-XXXX para cédula, 12 dígitos para DIMEX o formato alfanumérico para pasaporte.")]
        public string ClienteId { get; set; } = string.Empty;

        [Required(ErrorMessage = "El nombre es obligatorio.")]
        [Display(Name = "Nombre")]
        [StringLength(50, MinimumLength = 3, ErrorMessage = "El nombre must tener entre 3 y 50 caracteres.")]
        public string NombreCliente { get; set; } = string.Empty;

        [Required(ErrorMessage = "El primer apellido es obligatorio.")]
        [Display(Name = "Primer apellido")]
        [StringLength(75, MinimumLength = 3, ErrorMessage = "El primer apellido debe tener entre 3 y 75 caracteres.")]
        public string PrimerApellidoCliente { get; set; } = string.Empty;

        [Required(ErrorMessage = "El segundo apellido es obligatorio.")]
        [Display(Name = "Segundo apellido")]
        [StringLength(75, MinimumLength = 3, ErrorMessage = "El segundo apellido debe tener entre 3 y 75 caracteres.")]
        public string SegundoApellidoCliente { get; set; } = string.Empty;

        [Required(ErrorMessage = "La fecha de nacimiento es obligatoria.")]
        [Display(Name = "Fecha de nacimiento")]
        [DataType(DataType.Date)]
        public DateTime FechaNacimientoCliente { get; set; }
    }
}
