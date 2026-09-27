using System;
using System.Collections.Generic;

namespace SistemaHotelColibri.DataAccess.Entidades;

public partial class Housekeeping
{
    public int IdTarea { get; set; }

    public int IdHabitacion { get; set; }

    public int IdUsuario { get; set; }

    public string TipoTarea { get; set; } = null!;

    public DateTime FechaAsignacion { get; set; }

    public DateTime? FechaLimite { get; set; }

    public DateTime? FechaCompletada { get; set; }

    public string EstadoTarea { get; set; } = null!;

    public string? Observaciones { get; set; }

    public virtual Habitacion IdHabitacionNavigation { get; set; } = null!;
}
