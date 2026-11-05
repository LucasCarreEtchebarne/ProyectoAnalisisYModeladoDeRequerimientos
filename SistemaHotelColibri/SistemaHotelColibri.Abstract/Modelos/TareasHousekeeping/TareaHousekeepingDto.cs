using SistemaHotelColibri.Abstract.Comun;

namespace SistemaHotelColibri.Abstract.Modelos.TareasHousekeeping;

public class TareaHousekeepingDto
{
    public int IdTarea { get; set; }
    public int IdHabitacion { get; set; }
    public string NumeroHabitacion { get; set; } = string.Empty;
    public int IdUsuario { get; set; }
    public string NombreUsuario { get; set; } = string.Empty;
    public string TipoTarea { get; set; } = string.Empty;
    public DateTime FechaAsignacion { get; set; }
    public DateTime? FechaLimite { get; set; }
    public DateTime? FechaCompletada { get; set; }
    public string EstadoTarea { get; set; } = string.Empty;
    public string? Observaciones { get; set; }
    public string? MotivoCancelacion { get; set; }
    public DateTime? FechaCancelacion { get; set; }

    public bool EstaActiva => Estados.Tarea.Activos.Contains(EstadoTarea);

    public bool EstaVencida => EstaActiva && FechaLimite.HasValue && FechaLimite.Value < DateTime.Now;
}