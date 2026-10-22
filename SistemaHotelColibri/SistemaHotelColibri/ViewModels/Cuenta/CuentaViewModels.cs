using System.ComponentModel.DataAnnotations;
using SistemaHotelColibri.Abstract.Comun;

namespace SistemaHotelColibri.ViewModels.Cuenta;

public class IniciarSesionViewModel
{
    [Display(Name = "usuario o correo")]
    [Required(ErrorMessage = Mensajes.CampoObligatorio)]
    public string? UsuarioOCorreo { get; set; }

    [Display(Name = "contraseña")]
    [Required(ErrorMessage = Mensajes.CampoObligatorio)]
    [DataType(DataType.Password)]
    public string? Contrasena { get; set; }

    public bool Recordarme { get; set; }

    public string? ReturnUrl { get; set; }
}

public class CambiarContrasenaViewModel
{
    [Display(Name = "contraseña actual")]
    [Required(ErrorMessage = Mensajes.CampoObligatorio)]
    [DataType(DataType.Password)]
    public string? ContrasenaActual { get; set; }

    [Display(Name = "nueva contraseña")]
    [Required(ErrorMessage = Mensajes.CampoObligatorio)]
    [StringLength(100, MinimumLength = 8, ErrorMessage = "La contraseña debe tener al menos {2} caracteres.")]
    [DataType(DataType.Password)]
    public string? ContrasenaNueva { get; set; }

    [Display(Name = "confirmación")]
    [Required(ErrorMessage = Mensajes.CampoObligatorio)]
    [Compare(nameof(ContrasenaNueva), ErrorMessage = "La confirmación no coincide con la nueva contraseña.")]
    [DataType(DataType.Password)]
    public string? ConfirmarContrasena { get; set; }
}

public class OlvideContrasenaViewModel
{
    [Display(Name = "correo electrónico")]
    [Required(ErrorMessage = Mensajes.CampoObligatorio)]
    [EmailAddress(ErrorMessage = Mensajes.CorreoInvalido)]
    public string? CorreoElectronico { get; set; }
}

public class RestablecerContrasenaViewModel
{
    [Required]
    public string? Correo { get; set; }

    [Required]
    public string? Token { get; set; }

    [Display(Name = "nueva contraseña")]
    [Required(ErrorMessage = Mensajes.CampoObligatorio)]
    [StringLength(100, MinimumLength = 8, ErrorMessage = "La contraseña debe tener al menos {2} caracteres.")]
    [DataType(DataType.Password)]
    public string? ContrasenaNueva { get; set; }

    [Display(Name = "confirmación")]
    [Required(ErrorMessage = Mensajes.CampoObligatorio)]
    [Compare(nameof(ContrasenaNueva), ErrorMessage = "La confirmación no coincide con la nueva contraseña.")]
    [DataType(DataType.Password)]
    public string? ConfirmarContrasena { get; set; }
}
