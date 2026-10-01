using System.ComponentModel.DataAnnotations;
using SistemaHotelColibri.Abstract.Comun;
using SistemaHotelColibri.Abstract.Modelos.Clientes;

namespace SistemaHotelColibri.ViewModels.Clientes;

public class ClienteFormViewModel
{
    private const string PatronIdentificacion = @"^[A-Za-z0-9-]{5,30}$";
    private const string PatronNombre = @"^[A-Za-zÁÉÍÓÚÜÑáéíóúüñ' .-]+$";
    private const string PatronTelefono = @"^[0-9+()\s-]{8,20}$";

    public int IdCliente { get; set; }

    [Display(Name = "identificación")]
    [Required(ErrorMessage = Mensajes.CampoObligatorio)]
    [StringLength(30, ErrorMessage = Mensajes.LongitudMaxima)]
    [RegularExpression(PatronIdentificacion, ErrorMessage = Mensajes.FormatoInvalido)]
    public string? Identificacion { get; set; }

    [Display(Name = "nombre")]
    [Required(ErrorMessage = Mensajes.CampoObligatorio)]
    [StringLength(100, ErrorMessage = Mensajes.LongitudMaxima)]
    [RegularExpression(PatronNombre, ErrorMessage = Mensajes.FormatoInvalido)]
    public string? Nombre { get; set; }

    [Display(Name = "primer apellido")]
    [Required(ErrorMessage = Mensajes.CampoObligatorio)]
    [StringLength(50, ErrorMessage = Mensajes.LongitudMaxima)]
    [RegularExpression(PatronNombre, ErrorMessage = Mensajes.FormatoInvalido)]
    public string? PrimerApellido { get; set; }

    [Display(Name = "segundo apellido")]
    [StringLength(50, ErrorMessage = Mensajes.LongitudMaxima)]
    [RegularExpression(PatronNombre, ErrorMessage = Mensajes.FormatoInvalido)]
    public string? SegundoApellido { get; set; }

    [Display(Name = "teléfono")]
    [StringLength(20, ErrorMessage = Mensajes.LongitudMaxima)]
    [RegularExpression(PatronTelefono, ErrorMessage = Mensajes.FormatoInvalido)]
    public string? Telefono { get; set; }

    [Display(Name = "correo electrónico")]
    [StringLength(150, ErrorMessage = Mensajes.LongitudMaxima)]
    [EmailAddress(ErrorMessage = Mensajes.CorreoInvalido)]
    public string? CorreoElectronico { get; set; }

    [Display(Name = "dirección")]
    [StringLength(250, ErrorMessage = Mensajes.LongitudMaxima)]
    public string? Direccion { get; set; }

    [Display(Name = "estado")]
    [Required(ErrorMessage = Mensajes.CampoObligatorio)]
    public string? EstadoCliente { get; set; } = Estados.Cliente.Activo;

    public bool EsEdicion => IdCliente > 0;

    public static ClienteFormViewModel DesdeDto(ClienteDto cliente) => new()
    {
        IdCliente = cliente.IdCliente,
        Identificacion = cliente.Identificacion,
        Nombre = cliente.Nombre,
        PrimerApellido = cliente.PrimerApellido,
        SegundoApellido = cliente.SegundoApellido,
        Telefono = cliente.Telefono,
        CorreoElectronico = cliente.CorreoElectronico,
        Direccion = cliente.Direccion,
        EstadoCliente = cliente.EstadoCliente
    };

    public ClienteDto ADto() => new()
    {
        IdCliente = IdCliente,
        Identificacion = Identificacion ?? string.Empty,
        Nombre = Nombre ?? string.Empty,
        PrimerApellido = PrimerApellido ?? string.Empty,
        SegundoApellido = SegundoApellido,
        Telefono = Telefono,
        CorreoElectronico = CorreoElectronico,
        Direccion = Direccion,
        EstadoCliente = EstadoCliente ?? string.Empty
    };
}