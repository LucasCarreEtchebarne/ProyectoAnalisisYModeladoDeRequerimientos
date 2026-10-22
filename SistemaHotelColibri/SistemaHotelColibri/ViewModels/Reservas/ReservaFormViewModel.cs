using System.ComponentModel.DataAnnotations;
using SistemaHotelColibri.Abstract.Comun;
using SistemaHotelColibri.Abstract.Modelos.Reservas;

namespace SistemaHotelColibri.ViewModels.Reservas;

public class ReservaFormViewModel
{
    public int IdReserva { get; set; }

    [Display(Name = "cliente")]
    [Required(ErrorMessage = Mensajes.CampoObligatorio)]
    public int? IdCliente { get; set; }

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
