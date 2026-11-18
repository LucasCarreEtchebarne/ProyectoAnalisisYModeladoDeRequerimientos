using System.ComponentModel.DataAnnotations;
using SistemaHotelColibri.Abstract.Comun;
using SistemaHotelColibri.Abstract.Modelos.Clientes;
using SistemaHotelColibri.Abstract.Modelos.Reservas;

namespace SistemaHotelColibri.ViewModels.Reservas;

public class ReservaFormViewModel : IValidatableObject
{
    private const string PatronIdentificacion = @"^[A-Za-z0-9-]{5,30}$";
    private const string PatronNombre = @"^[A-Za-zÁÉÍÓÚÜÑáéíóúüñ' .-]+$";
    private const string PatronTelefono = @"^[0-9+()\s-]{8,20}$";

    public int IdReserva { get; set; }

    // Obligatorio solo cuando se elige un cliente ya registrado (ver Validate).
    [Display(Name = "cliente")]
    public int? IdCliente { get; set; }

    // ----- Cliente nuevo escrito en la misma reserva -----
    public bool ClienteNuevo { get; set; }

    // Respuesta a "¿Guardar la información del cliente en Clientes?"
    public bool GuardarCliente { get; set; }

    [Display(Name = "identificación")]
    [StringLength(30, ErrorMessage = Mensajes.LongitudMaxima)]
    [RegularExpression(PatronIdentificacion, ErrorMessage = Mensajes.FormatoInvalido)]
    public string? ClienteIdentificacion { get; set; }

    [Display(Name = "nombre")]
    [StringLength(100, ErrorMessage = Mensajes.LongitudMaxima)]
    [RegularExpression(PatronNombre, ErrorMessage = Mensajes.FormatoInvalido)]
    public string? ClienteNombre { get; set; }

    [Display(Name = "primer apellido")]
    [StringLength(50, ErrorMessage = Mensajes.LongitudMaxima)]
    [RegularExpression(PatronNombre, ErrorMessage = Mensajes.FormatoInvalido)]
    public string? ClientePrimerApellido { get; set; }

    [Display(Name = "segundo apellido")]
    [StringLength(50, ErrorMessage = Mensajes.LongitudMaxima)]
    [RegularExpression(PatronNombre, ErrorMessage = Mensajes.FormatoInvalido)]
    public string? ClienteSegundoApellido { get; set; }

    [Display(Name = "teléfono")]
    [StringLength(20, ErrorMessage = Mensajes.LongitudMaxima)]
    [RegularExpression(PatronTelefono, ErrorMessage = Mensajes.FormatoInvalido)]
    public string? ClienteTelefono { get; set; }

    [Display(Name = "correo electrónico")]
    [StringLength(150, ErrorMessage = Mensajes.LongitudMaxima)]
    [EmailAddress(ErrorMessage = Mensajes.CorreoInvalido)]
    public string? ClienteCorreo { get; set; }

    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        if (!ClienteNuevo)
        {
            if (IdCliente is not > 0)
            {
                yield return new ValidationResult("Seleccioná un cliente o elegí «Cliente nuevo».", [nameof(IdCliente)]);
            }

            yield break;
        }

        if (string.IsNullOrWhiteSpace(ClienteIdentificacion))
        {
            yield return new ValidationResult("Escribí la identificación del cliente.", [nameof(ClienteIdentificacion)]);
        }

        if (string.IsNullOrWhiteSpace(ClienteNombre))
        {
            yield return new ValidationResult("Escribí el nombre del cliente.", [nameof(ClienteNombre)]);
        }

        if (string.IsNullOrWhiteSpace(ClientePrimerApellido))
        {
            yield return new ValidationResult("Escribí el primer apellido del cliente.", [nameof(ClientePrimerApellido)]);
        }
    }

    public ClienteDto ClienteADto() => new()
    {
        Identificacion = ClienteIdentificacion ?? string.Empty,
        Nombre = ClienteNombre ?? string.Empty,
        PrimerApellido = ClientePrimerApellido ?? string.Empty,
        SegundoApellido = ClienteSegundoApellido,
        Telefono = ClienteTelefono,
        CorreoElectronico = ClienteCorreo
    };

    [Display(Name = "habitación")]
    [Required(ErrorMessage = Mensajes.CampoObligatorio)]
    public int? IdHabitacion { get; set; }

    [Display(Name = "fecha de entrada")]
    [Required(ErrorMessage = Mensajes.CampoObligatorio)]
    [DataType(DataType.Date)]
    public DateOnly? FechaEntrada { get; set; }

    [Display(Name = "fecha de salida")]
    [Required(ErrorMessage = Mensajes.CampoObligatorio)]
    [DataType(DataType.Date)]
    public DateOnly? FechaSalida { get; set; }

    [Display(Name = "cantidad de huéspedes")]
    [Required(ErrorMessage = Mensajes.CampoObligatorio)]
    [Range(1, 20, ErrorMessage = Mensajes.RangoValores)]
    public int? CantidadHuespedes { get; set; } = 1;

    [Display(Name = "precio por noche")]
    [Required(ErrorMessage = Mensajes.CampoObligatorio)]
    [Range(0, 999999999.99, ErrorMessage = Mensajes.ValorNoNegativo)]
    public decimal? PrecioNoche { get; set; }

    [Display(Name = "estado de reserva")]
    [Required(ErrorMessage = Mensajes.CampoObligatorio)]
    public string? EstadoReserva { get; set; } = Estados.Reserva.Pendiente;

    [Display(Name = "placa del vehículo")]
    [StringLength(10, ErrorMessage = Mensajes.LongitudMaxima)]
    public string? PlacaVehiculo { get; set; }

    [Display(Name = "observaciones")]
    [StringLength(250, ErrorMessage = Mensajes.LongitudMaxima)]
    public string? Observaciones { get; set; }

    public static ReservaFormViewModel DesdeDto(ReservaDto d) => new()
    {
        IdReserva = d.IdReserva,
        IdCliente = d.IdCliente,
        IdHabitacion = d.IdHabitacion,
        FechaEntrada = d.FechaEntrada,
        FechaSalida = d.FechaSalida,
        CantidadHuespedes = d.CantidadHuespedes,
        PrecioNoche = d.PrecioNoche,
        EstadoReserva = d.EstadoReserva,
        PlacaVehiculo = d.PlacaVehiculo,
        Observaciones = d.Observaciones
    };

    public ReservaDto ADto() => new()
    {
        IdReserva = IdReserva,
        IdCliente = IdCliente ?? 0,
        IdHabitacion = IdHabitacion ?? 0,
        FechaEntrada = FechaEntrada ?? default,
        FechaSalida = FechaSalida ?? default,
        CantidadHuespedes = CantidadHuespedes ?? 0,
        PrecioNoche = PrecioNoche ?? 0,
        EstadoReserva = EstadoReserva ?? string.Empty,
        PlacaVehiculo = PlacaVehiculo,
        Observaciones = Observaciones
    };
}
