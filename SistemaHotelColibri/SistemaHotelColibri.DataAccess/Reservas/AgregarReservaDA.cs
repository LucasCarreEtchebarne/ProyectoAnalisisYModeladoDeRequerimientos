using SistemaHotelColibri.Abstract.Interfaces.DataAccess.Reservas;
using SistemaHotelColibri.Abstract.Modelos.Reservas;
using SistemaHotelColibri.DataAccess.Contexto;
using SistemaHotelColibri.DataAccess.Entidades;

namespace SistemaHotelColibri.DataAccess.Reservas;

public class AgregarReservaDA : IAgregarReservaDA
{
    private readonly HotelColibriContext _contexto;

    public AgregarReservaDA(HotelColibriContext contexto)
    {
        _contexto = contexto;
    }

    public async Task<int> AgregarAsync(ReservaDto reserva)
    {
        var entidad = new Reserva
        {
            IdCliente = reserva.IdCliente,
            IdHabitacion = reserva.IdHabitacion,
            FechaEntrada = reserva.FechaEntrada,
            FechaSalida = reserva.FechaSalida,
            CantidadHuespedes = reserva.CantidadHuespedes,
            PrecioNoche = reserva.PrecioNoche,
            EstadoReserva = reserva.EstadoReserva,
            PlacaVehiculo = reserva.PlacaVehiculo,
            Observaciones = reserva.Observaciones
        };
        _contexto.Reserva.Add(entidad);
        await SincronizadorHabitacionReserva.AplicarAsync(_contexto, entidad.IdHabitacion, entidad.EstadoReserva);
        await _contexto.SaveChangesAsync();
        return entidad.IdReserva;
    }
}
