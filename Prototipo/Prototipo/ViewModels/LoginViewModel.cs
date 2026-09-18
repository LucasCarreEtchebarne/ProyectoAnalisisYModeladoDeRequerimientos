using System.ComponentModel.DataAnnotations;

namespace Prototipo.ViewModels
{
    public class LoginViewModel
    {
        [Required(ErrorMessage = "El nombre de usuario es obligatorio.")]
        [Display(Name = "Nombre de usuario")]
        public string NombreUsuario { get; set; }

        [Required(ErrorMessage = "La contrasena es obligatoria.")]
        [DataType(DataType.Password)]
        [Display(Name = "Contrasena")]
        public string Contrasena { get; set; }

        public string ReturnUrl { get; set; }
    }
}
