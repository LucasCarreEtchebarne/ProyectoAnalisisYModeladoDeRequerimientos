using System.ComponentModel.DataAnnotations;
using SistemaHotelColibri.Abstract.Comun;
using SistemaHotelColibri.Abstract.Modelos.Mesas;

namespace SistemaHotelColibri.ViewModels.Mesas;

public class MesaFormViewModel
{
    public int IdMesa { get; set; }

    [Display(Name = "número de mesa")]
    [Required(ErrorMessage = Mensajes.CampoObligatorio)]
    [Range(1, 9999, ErrorMessage = Mensajes.RangoValores)]
    public int? NumeroMesa { get; set; }

    [Display(Name = "capacidad")]
    [Required(ErrorMessage = Mensajes.CampoObligatorio)]
    [Range(1, 50, ErrorMessage = Mensajes.RangoValores)]
    public int? Capacidad { get; set; }

    [Display(Name = "estado de mesa")]
    [Required(ErrorMessage = Mensajes.CampoObligatorio)]
    public string? EstadoMesa { get; set; } = Estados.Mesa.Disponible;

    [Display(Name = "descripción")]
    [StringLength(250, ErrorMessage = Mensajes.LongitudMaxima)]
    public string? Descripcion { get; set; }

    public static MesaFormViewModel DesdeDto(MesaDto mesa) => new()
    {
        IdMesa = mesa.IdMesa,
        NumeroMesa = mesa.NumeroMesa,
        Capacidad = mesa.Capacidad,
        EstadoMesa = mesa.EstadoMesa,
        Descripcion = mesa.Descripcion
    };

    public MesaDto ADto() => new()
    {
        IdMesa = IdMesa,
        NumeroMesa = NumeroMesa ?? 0,
        Capacidad = Capacidad ?? 0,
        EstadoMesa = EstadoMesa ?? string.Empty,
        Descripcion = Descripcion
    };
}
