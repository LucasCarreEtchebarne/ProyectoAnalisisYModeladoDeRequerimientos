using SistemaHotelColibri.Abstract.Comun;
using SistemaHotelColibri.DataAccess.Contexto;

namespace SistemaHotelColibri.DataAccess.Reservas;

internal static class SincronizadorHabitacionReserva
{
    public static async Task AplicarAsync(HotelColibriContext contexto, int idHabitacion, string estadoReserva)
    {
        var nuevoEstado = estadoReserva switch
        {
            Estados.Reserva.CheckIn => Estados.Habitacion.Ocupada,
            Estados.Reserva.CheckOut => Estados.Habitacion.Limpieza,
            _ => null
        };

        if (nuevoEstado == null)
        {
            return;
        }

        var habitacion = await contexto.Habitacion.FindAsync(idHabitacion);
        if (habitacion != null && habitacion.EstadoHabitacion != Estados.Habitacion.Inactiva)
        {
            habitacion.EstadoHabitacion = nuevoEstado;
        }
    }

    public static async Task LiberarAsync(HotelColibriContext contexto, int idHabitacion)
    {
        var habitacion = await contexto.Habitacion.FindAsync(idHabitacion);
        if (habitacion != null && habitacion.EstadoHabitacion == Estados.Habitacion.Ocupada)
        {
            habitacion.EstadoHabitacion = Estados.Habitacion.Disponible;
        }
    }
}
