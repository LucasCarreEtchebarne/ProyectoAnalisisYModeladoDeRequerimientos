using System.ComponentModel.DataAnnotations;
using SistemaHotelColibri.Abstract.Comun;
using SistemaHotelColibri.Abstract.Modelos.Usuarios;

namespace SistemaHotelColibri.ViewModels.Usuarios;

public class UsuarioFormViewModel : IValidatableObject
{
    public int IdUsuario { get; set; }

    [Display(Name = "nombre completo")]
    [Required(ErrorMessage = Mensajes.CampoObligatorio)]
    [StringLength(150, ErrorMessage = Mensajes.LongitudMaxima)]
    public string? NombreCompleto { get; set; }

    [Display(Name = "nombre de usuario")]
    [Required(ErrorMessage = Mensajes.CampoObligatorio)]
    [StringLength(50, ErrorMessage = Mensajes.LongitudMaxima)]
    [RegularExpression(@"^[A-Za-z0-9._-]+$", ErrorMessage = Mensajes.NombreUsuarioInvalido)]
    public string? NombreUsuario { get; set; }

    [Display(Name = "correo electrónico")]
    [Required(ErrorMessage = Mensajes.CampoObligatorio)]
    [StringLength(150, ErrorMessage = Mensajes.LongitudMaxima)]
    [EmailAddress(ErrorMessage = Mensajes.CorreoInvalido)]
    public string? CorreoElectronico { get; set; }

    [Display(Name = "rol")]
    [Required(ErrorMessage = Mensajes.CampoObligatorio)]
    public string? Rol { get; set; }

    [Display(Name = "contraseña inicial")]
    [StringLength(100, MinimumLength = 8, ErrorMessage = "La contraseña debe tener al menos {2} caracteres.")]
    [DataType(DataType.Password)]
    public string? Contrasena { get; set; }

    [Display(Name = "confirmación")]
    [Compare(nameof(Contrasena), ErrorMessage = "La confirmación no coincide con la contraseña.")]
    [DataType(DataType.Password)]
    public string? ConfirmarContrasena { get; set; }

    public bool EsEdicion => IdUsuario > 0;

    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        if (!EsEdicion && string.IsNullOrEmpty(Contrasena))
        {
            yield return new ValidationResult(string.Format(Mensajes.CampoObligatorio, "contraseña inicial"), [nameof(Contrasena)]);
        }
    }

    public static UsuarioFormViewModel DesdeDto(UsuarioDto usuario) => new()
    {
        IdUsuario = usuario.IdUsuario,
        NombreCompleto = usuario.NombreCompleto,
        NombreUsuario = usuario.NombreUsuario,
        CorreoElectronico = usuario.CorreoElectronico,
        Rol = usuario.Rol
    };

    public UsuarioDto ADto() => new()
    {
        IdUsuario = IdUsuario,
        NombreCompleto = NombreCompleto ?? string.Empty,
        NombreUsuario = NombreUsuario ?? string.Empty,
        CorreoElectronico = CorreoElectronico ?? string.Empty,
        Rol = Rol ?? string.Empty
    };
}
