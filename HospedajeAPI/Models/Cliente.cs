using System.ComponentModel.DataAnnotations;

namespace HospedajeAPI.Models
{
    public class Cliente
    {
        [Required(ErrorMessage = "El tipo de identificación es obligatorio.")]
        [Display(Name = "Tipo de identificación")]
        public string TipoIdentificacion { get; set; } = string.Empty;

        [Required(ErrorMessage = "La identificación es obligatoria.")]
        [Display(Name = "Número de identificación")]
        public string ClienteId { get; set; } = string.Empty;

        [Required(ErrorMessage = "El nombre es obligatorio.")]
        [Display(Name = "Nombre")]
        [StringLength(50, MinimumLength = 3, ErrorMessage = "El nombre debe tener entre 3 y 50 caracteres.")]
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
