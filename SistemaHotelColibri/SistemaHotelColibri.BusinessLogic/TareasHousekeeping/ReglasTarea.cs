using SistemaHotelColibri.Abstract.Comun;
using SistemaHotelColibri.Abstract.Modelos.TareasHousekeeping;

namespace SistemaHotelColibri.BusinessLogic.TareasHousekeeping;

internal static class ReglasTarea
{
    // Flujo de estados permitido (HSK-005). La cancelación va aparte (HSK-006).
    private static readonly Dictionary<string, string[]> Transiciones = new()
    {
        [Estados.Tarea.Pendiente] = new[] { Estados.Tarea.EnProceso, Estados.Tarea.Completada },
        [Estados.Tarea.EnProceso] = new[] { Estados.Tarea.Completada, Estados.Tarea.Pendiente }
    };

    public static void Normalizar(TareaHousekeepingDto tarea)
    {
        tarea.TipoTarea = tarea.TipoTarea.Trim();
        tarea.Observaciones = string.IsNullOrWhiteSpace(tarea.Observaciones) ? null : tarea.Observaciones.Trim();
    }

    public static string? Validar(TareaHousekeepingDto tarea)
    {
        if (!Catalogos.TiposTareaHousekeeping.Contains(tarea.TipoTarea))
        {
            return Mensajes.TipoTareaInvalido;
        }

        if (tarea.FechaLimite.HasValue && tarea.FechaLimite.Value <= tarea.FechaAsignacion)
        {
            return Mensajes.FechaLimiteInvalida;
        }

        return null;
    }

    public static bool PuedeCambiar(string estadoActual, string nuevoEstado) =>
        Transiciones.TryGetValue(estadoActual, out var permitidos) && permitidos.Contains(nuevoEstado);
}