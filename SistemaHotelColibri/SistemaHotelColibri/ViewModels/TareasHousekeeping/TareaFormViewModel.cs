using System.ComponentModel.DataAnnotations;
using SistemaHotelColibri.Abstract.Comun;
using SistemaHotelColibri.Abstract.Modelos.TareasHousekeeping;

namespace SistemaHotelColibri.ViewModels.TareasHousekeeping;

public class TareaFormViewModel
{
    public int IdTarea { get; set; }

    public string? EstadoTarea { get; set; }

    [Display(Name = "habitación")]
    [Required(ErrorMessage = Mensajes.CampoObligatorio)]
    public int? IdHabitacion { get; set; }

    [Display(Name = "colaborador asignado")]
    [Required(ErrorMessage = Mensajes.CampoObligatorio)]
    public int? IdUsuario { get; set; }

    [Display(Name = "tipo de tarea")]
    [Required(ErrorMessage = Mensajes.CampoObligatorio)]
    [StringLength(50, ErrorMessage = Mensajes.LongitudMaxima)]
    public string? TipoTarea { get; set; }

    [Display(Name = "fecha límite")]
    [DataType(DataType.DateTime)]
    public DateTime? FechaLimite { get; set; }

    [Display(Name = "observaciones")]
    [StringLength(250, ErrorMessage = Mensajes.LongitudMaxima)]
    public string? Observaciones { get; set; }

    public static TareaFormViewModel DesdeDto(TareaHousekeepingDto tarea) => new()
    {
        IdTarea = tarea.IdTarea,
        EstadoTarea = tarea.EstadoTarea,
        IdHabitacion = tarea.IdHabitacion,
        IdUsuario = tarea.IdUsuario,
        TipoTarea = tarea.TipoTarea,
        FechaLimite = tarea.FechaLimite,
        Observaciones = tarea.Observaciones
    };

    public TareaHousekeepingDto ADto() => new()
    {
        IdTarea = IdTarea,
        IdHabitacion = IdHabitacion ?? 0,
        IdUsuario = IdUsuario ?? 0,
        TipoTarea = TipoTarea ?? string.Empty,
        FechaLimite = FechaLimite,
        Observaciones = Observaciones
    };
}