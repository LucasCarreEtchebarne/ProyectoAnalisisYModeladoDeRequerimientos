using SistemaHotelColibri.Abstract.Comun;
using SistemaHotelColibri.Abstract.Interfaces.DataAccess.Reservas;
using SistemaHotelColibri.Abstract.Modelos.Reservas;
using SistemaHotelColibri.DataAccess.Contexto;

namespace SistemaHotelColibri.DataAccess.Reservas;

public class EditarReservaDA : IEditarReservaDA
{
    private readonly HotelColibriContext _contexto;

    public EditarReservaDA(HotelColibriContext contexto)
    {
        _contexto = contexto;
    }

    public async Task<bool> EditarAsync(ReservaDto reserva)
    {
        var entidad = await _contexto.Reserva.FindAsync(reserva.IdReserva);
        if (entidad == null)
        {
            return false;
        }

        var estadoAnterior = entidad.EstadoReserva;
        var habitacionAnterior = entidad.IdHabitacion;

        entidad.IdCliente = reserva.IdCliente;
        entidad.IdHabitacion = reserva.IdHabitacion;
        entidad.FechaEntrada = reserva.FechaEntrada;
        entidad.FechaSalida = reserva.FechaSalida;
        entidad.CantidadHuespedes = reserva.CantidadHuespedes;
        entidad.PrecioNoche = reserva.PrecioNoche;
        entidad.EstadoReserva = reserva.EstadoReserva;
        entidad.PlacaVehiculo = reserva.PlacaVehiculo;
        entidad.Observaciones = reserva.Observaciones;

        var dejoCheckIn = estadoAnterior == Estados.Reserva.CheckIn
            && (entidad.EstadoReserva != Estados.Reserva.CheckIn || habitacionAnterior != entidad.IdHabitacion);
        if (dejoCheckIn && entidad.EstadoReserva != Estados.Reserva.CheckOut)
        {
            await SincronizadorHabitacionReserva.LiberarAsync(_contexto, habitacionAnterior);
        }

        if (estadoAnterior != entidad.EstadoReserva || habitacionAnterior != entidad.IdHabitacion)
        {
            await SincronizadorHabitacionReserva.AplicarAsync(_contexto, entidad.IdHabitacion, entidad.EstadoReserva);
        }

        await _contexto.SaveChangesAsync();
        return true;
    }
}
