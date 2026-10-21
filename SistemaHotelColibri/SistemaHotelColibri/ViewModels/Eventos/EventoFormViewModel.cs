using System.ComponentModel.DataAnnotations;
using SistemaHotelColibri.Abstract.Comun;
using SistemaHotelColibri.Abstract.Modelos.Eventos;

namespace SistemaHotelColibri.ViewModels.Eventos;

public class EventoFormViewModel
{
    public int IdEvento { get; set; }

    [Display(Name = "cliente")]
    [Required(ErrorMessage = Mensajes.CampoObligatorio)]
    public int? IdCliente { get; set; }

    [Display(Name = "espacio")]
    [Required(ErrorMessage = Mensajes.CampoObligatorio)]
    public int? IdEspacio { get; set; }

    [Display(Name = "nombre del evento")]
    [Required(ErrorMessage = Mensajes.CampoObligatorio)]
    [StringLength(100, ErrorMessage = Mensajes.LongitudMaxima)]
    public string NombreEvento { get; set; } = string.Empty;

    [Display(Name = "fecha del evento")]
    [Required(ErrorMessage = Mensajes.CampoObligatorio)]
    [DataType(DataType.Date)]
    public DateOnly? FechaEvento { get; set; }

    [Display(Name = "hora de inicio")]
    [Required(ErrorMessage = Mensajes.CampoObligatorio)]
    [DisplayFormat(DataFormatString = "{0:HH\\:mm}", ApplyFormatInEditMode = true)]
    public TimeOnly? HoraInicio { get; set; }

    [Display(Name = "hora de fin")]
    [Required(ErrorMessage = Mensajes.CampoObligatorio)]
    [DisplayFormat(DataFormatString = "{0:HH\\:mm}", ApplyFormatInEditMode = true)]
    public TimeOnly? HoraFin { get; set; }

    [Display(Name = "participantes")]
    [Required(ErrorMessage = Mensajes.CampoObligatorio)]
    [Range(1, 100000, ErrorMessage = Mensajes.RangoValores)]
    public int? Participantes { get; set; } = 1;

    [Display(Name = "monto acordado")]
    [Required(ErrorMessage = Mensajes.CampoObligatorio)]
    [Range(0, 999999999.99, ErrorMessage = Mensajes.ValorNoNegativo)]
    public decimal? MontoAcordado { get; set; } = 0;

    [Display(Name = "estado del evento")]
    [Required(ErrorMessage = Mensajes.CampoObligatorio)]
    public string? EstadoEvento { get; set; } = Estados.Evento.Programado;

    [Display(Name = "descripción")]
    [StringLength(250, ErrorMessage = Mensajes.LongitudMaxima)]
    public string? Descripcion { get; set; }

    public static EventoFormViewModel DesdeDto(EventoDto d) => new()
    {
        IdEvento = d.IdEvento,
        IdCliente = d.IdCliente,
        IdEspacio = d.IdEspacio,
        NombreEvento = d.NombreEvento,
        FechaEvento = d.FechaEvento,
        HoraInicio = d.HoraInicio,
        HoraFin = d.HoraFin,
        Participantes = d.Participantes,
        MontoAcordado = d.MontoAcordado,
        EstadoEvento = d.EstadoEvento,
        Descripcion = d.Descripcion
    };

    public EventoDto ADto() => new()
    {
        IdEvento = IdEvento,
        IdCliente = IdCliente ?? 0,
        IdEspacio = IdEspacio ?? 0,
        NombreEvento = NombreEvento,
        FechaEvento = FechaEvento ?? default,
        HoraInicio = HoraInicio ?? default,
        HoraFin = HoraFin ?? default,
        Participantes = Participantes ?? 0,
        MontoAcordado = MontoAcordado ?? 0,
        EstadoEvento = EstadoEvento ?? string.Empty,
        Descripcion = Descripcion
    };
}
