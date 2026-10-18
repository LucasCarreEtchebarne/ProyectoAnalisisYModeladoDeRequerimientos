using System;
using System.Collections.Generic;

namespace SistemaHotelColibri.DataAccess.Entidades;

public partial class HabitacionImagen
{
    public int IdImagen { get; set; }

    public int IdHabitacion { get; set; }

    public string RutaArchivo { get; set; } = null!;

    public string? NombreOriginal { get; set; }

    public bool EsPrincipal { get; set; }

    public int Orden { get; set; }

    public DateTime FechaSubida { get; set; }

    public virtual Habitacion IdHabitacionNavigation { get; set; } = null!;
}
