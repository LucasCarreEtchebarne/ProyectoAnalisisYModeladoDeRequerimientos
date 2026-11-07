using SistemaHotelColibri.Abstract.Comun;
using SistemaHotelColibri.Abstract.Modelos.Mesas;

namespace SistemaHotelColibri.BusinessLogic.Mesas;

public static class ReglasMesa
{
    public static void Normalizar(MesaDto mesa)
    {
        mesa.EstadoMesa = mesa.EstadoMesa?.Trim() ?? string.Empty;
        mesa.Descripcion = string.IsNullOrWhiteSpace(mesa.Descripcion) ? null : mesa.Descripcion.Trim();
    }

    public static string? Validar(MesaDto mesa)
    {
        if (mesa.NumeroMesa <= 0)
        {
            return Mensajes.NumeroMesaInvalido;
        }

        if (mesa.Capacidad <= 0)
        {
            return Mensajes.CapacidadMesaInvalida;
        }

        if (!Estados.Mesa.Todos.Contains(mesa.EstadoMesa))
        {
            return Mensajes.EstadoMesaInvalido;
        }

        if (mesa.Descripcion is { Length: > 250 })
        {
            return string.Format(Mensajes.LongitudMaxima, "Descripción", 250);
        }

        return null;
    }
}
