namespace SistemaHotelColibri.Abstract.Modelos.Reportes;

// Filtros comunes de los reportes. Cada reporte usa solo los que le sirven.
public class FiltroReporte
{
    public DateOnly Desde { get; set; }
    public DateOnly Hasta { get; set; }
    public string? Estado { get; set; }
    public string? TipoHabitacion { get; set; }
    public string? Area { get; set; }
    public string? Busqueda { get; set; }
    public bool SoloStockBajo { get; set; }

    // Cantidad de días del rango, contando el primero y el último.
    public int Dias => Math.Max(0, Hasta.DayNumber - Desde.DayNumber + 1);
}
