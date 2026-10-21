using System.ComponentModel.DataAnnotations;
using SistemaHotelColibri.Abstract.Comun;
using SistemaHotelColibri.Abstract.Modelos.Habitaciones;

namespace SistemaHotelColibri.ViewModels.Habitaciones;

public class HabitacionFormViewModel
{
    public int IdHabitacion { get; set; }

    [Display(Name = "número de habitación")]
    [Required(ErrorMessage = Mensajes.CampoObligatorio)]
    [StringLength(10, ErrorMessage = Mensajes.LongitudMaxima)]
    public string? NumeroHabitacion { get; set; }

    [Display(Name = "tipo de habitación")]
    [Required(ErrorMessage = Mensajes.CampoObligatorio)]
    public string? TipoHabitacion { get; set; } = "Estándar";

    [Display(Name = "estado de habitación")]
    [Required(ErrorMessage = Mensajes.CampoObligatorio)]
    public string? EstadoHabitacion { get; set; } = Estados.Habitacion.Disponible;

    [Display(Name = "capacidad")]
    [Required(ErrorMessage = Mensajes.CampoObligatorio)]
    [Range(1, 20, ErrorMessage = Mensajes.RangoValores)]
    public int? Capacidad { get; set; }

    [Display(Name = "precio")]
    [Required(ErrorMessage = Mensajes.CampoObligatorio)]
    [Range(0, 999999999.999, ErrorMessage = Mensajes.ValorNoNegativo)]
    public decimal? Precio { get; set; }

    [Display(Name = "piso")]
    [Required(ErrorMessage = Mensajes.CampoObligatorio)]
    [Range(0, 100, ErrorMessage = Mensajes.RangoValores)]
    public int? Piso { get; set; }

    [Display(Name = "cantidad de camas")]
    [Required(ErrorMessage = Mensajes.CampoObligatorio)]
    [Range(1, 20, ErrorMessage = Mensajes.RangoValores)]
    public int? CantidadCamas { get; set; } = 1;

    [Display(Name = "tipo de cama")]
    [Required(ErrorMessage = Mensajes.CampoObligatorio)]
    public string? TipoCama { get; set; } = "Matrimonial";

    [Display(Name = "vista")]
    public string? Vista { get; set; }

    [Display(Name = "baño privado")]
    public bool BanoPrivado { get; set; }

    [Display(Name = "aire acondicionado")]
    public bool AireAcondicionado { get; set; }

    [Display(Name = "televisión")]
    public bool Television { get; set; }

    [Display(Name = "wifi")]
    public bool Wifi { get; set; }

    [Display(Name = "descripción")]
    [StringLength(250, ErrorMessage = Mensajes.LongitudMaxima)]
    public string? Descripcion { get; set; }

    public static HabitacionFormViewModel DesdeDto(HabitacionDto habitacion) => new()
    {
        IdHabitacion = habitacion.IdHabitacion,
        NumeroHabitacion = habitacion.NumeroHabitacion,
        TipoHabitacion = habitacion.TipoHabitacion,
        EstadoHabitacion = habitacion.EstadoHabitacion,
        Capacidad = habitacion.Capacidad,
        Precio = habitacion.Precio,
        Piso = habitacion.Piso,
        CantidadCamas = habitacion.CantidadCamas,
        TipoCama = habitacion.TipoCama,
        Vista = habitacion.Vista,
        BanoPrivado = habitacion.BanoPrivado,
        AireAcondicionado = habitacion.AireAcondicionado,
        Television = habitacion.Television,
        Wifi = habitacion.Wifi,
        Descripcion = habitacion.Descripcion
    };

    public HabitacionDto ADto() => new()
    {
        IdHabitacion = IdHabitacion,
        NumeroHabitacion = NumeroHabitacion ?? string.Empty,
        TipoHabitacion = TipoHabitacion ?? string.Empty,
        EstadoHabitacion = EstadoHabitacion ?? string.Empty,
        Capacidad = Capacidad ?? 0,
        Precio = Precio ?? 0,
        Piso = Piso ?? 0,
        CantidadCamas = CantidadCamas ?? 0,
        TipoCama = TipoCama ?? string.Empty,
        Vista = Vista,
        BanoPrivado = BanoPrivado,
        AireAcondicionado = AireAcondicionado,
        Television = Television,
        Wifi = Wifi,
        Descripcion = Descripcion
    };
}
