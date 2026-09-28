using SistemaHotelColibri.Abstract.Comun;
using SistemaHotelColibri.Abstract.Modelos.Habitaciones;

namespace SistemaHotelColibri.BusinessLogic.Habitaciones;

internal static class ReglasHabitacion
{
    public static void Normalizar(HabitacionDto habitacion)
    {
        habitacion.NumeroHabitacion = habitacion.NumeroHabitacion.Trim();
        habitacion.TipoHabitacion = habitacion.TipoHabitacion.Trim();
        habitacion.Descripcion = string.IsNullOrWhiteSpace(habitacion.Descripcion) ? null : habitacion.Descripcion.Trim();
    }

    public static string? Validar(HabitacionDto habitacion)
    {
        if (habitacion.Capacidad <= 0)
        {
            return Mensajes.CapacidadInvalida;
        }

        if (habitacion.Precio < 0)
        {
            return Mensajes.PrecioInvalido;
        }

        if (habitacion.Piso < 0)
        {
            return Mensajes.PisoInvalido;
        }

        if (!Catalogos.TiposHabitacion.Contains(habitacion.TipoHabitacion))
        {
            return Mensajes.TipoHabitacionInvalido;
        }

        if (!Estados.Habitacion.Todos.Contains(habitacion.EstadoHabitacion))
        {
            return Mensajes.EstadoHabitacionInvalido;
        }

        return null;
    }
}
