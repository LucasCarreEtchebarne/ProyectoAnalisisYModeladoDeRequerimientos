using SistemaHotelColibri.Abstract.Comun;
using SistemaHotelColibri.Abstract.Modelos.Habitaciones;

namespace SistemaHotelColibri.BusinessLogic.Habitaciones;

internal static class ReglasHabitacion
{
    public static void Normalizar(HabitacionDto habitacion)
    {
        habitacion.NumeroHabitacion = habitacion.NumeroHabitacion.Trim();
        habitacion.TipoHabitacion = habitacion.TipoHabitacion.Trim();
        habitacion.TipoCama = habitacion.TipoCama.Trim();
        habitacion.Vista = string.IsNullOrWhiteSpace(habitacion.Vista) ? null : habitacion.Vista.Trim();
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

        if (habitacion.CantidadCamas <= 0)
        {
            return Mensajes.CantidadCamasInvalida;
        }

        if (!Catalogos.TiposCama.Contains(habitacion.TipoCama))
        {
            return Mensajes.TipoCamaInvalido;
        }

        if (habitacion.Vista != null && !Catalogos.VistasHabitacion.Contains(habitacion.Vista))
        {
            return Mensajes.VistaInvalida;
        }

        return null;
    }
}
