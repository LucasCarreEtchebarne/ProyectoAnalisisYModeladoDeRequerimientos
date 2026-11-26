using SistemaHotelColibri.Abstract.Modelos.Reportes;

namespace SistemaHotelColibri.BusinessLogic.Reportes;

internal static class ReglasReporte
{
    // Máximo de días que se pueden pedir de una vez, para que el reporte no se vuelva muy pesado.
    public const int MaximoDias = 366;

    public static void Normalizar(FiltroReporte filtro)
    {
        if (filtro.Hasta < filtro.Desde)
        {
            (filtro.Desde, filtro.Hasta) = (filtro.Hasta, filtro.Desde);
        }

        if (filtro.Dias > MaximoDias)
        {
            filtro.Desde = filtro.Hasta.AddDays(-(MaximoDias - 1));
        }

        filtro.Estado = Limpio(filtro.Estado);
        filtro.TipoHabitacion = Limpio(filtro.TipoHabitacion);
        filtro.Area = Limpio(filtro.Area);
        filtro.Busqueda = Limpio(filtro.Busqueda);
    }

    public static decimal Porcentaje(decimal parte, decimal total) =>
        total == 0 ? 0 : Math.Round(parte * 100 / total, 1, MidpointRounding.AwayFromZero);

    private static string? Limpio(string? texto) => string.IsNullOrWhiteSpace(texto) ? null : texto.Trim();
}
