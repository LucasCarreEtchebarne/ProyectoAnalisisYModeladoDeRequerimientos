using Microsoft.EntityFrameworkCore;
using SistemaHotelColibri.Abstract.Comun;
using SistemaHotelColibri.Abstract.Interfaces.DataAccess.Reservas;
using SistemaHotelColibri.DataAccess.Contexto;

namespace SistemaHotelColibri.DataAccess.Reservas;

public class HabitacionDisponibleReservaDA : IHabitacionDisponibleReservaDA
{
    private readonly HotelColibriContext _contexto;

    public HabitacionDisponibleReservaDA(HotelColibriContext contexto)
    {
        _contexto = contexto;
    }

    public async Task<bool> EstaDisponibleAsync(int idHabitacion, DateOnly fechaEntrada, DateOnly fechaSalida, int idReservaExcluida)
    {
        var haySolape = await _contexto.Reserva.AnyAsync(r =>
            r.IdHabitacion == idHabitacion
            && r.IdReserva != idReservaExcluida
            && r.EstadoReserva != Estados.Reserva.Cancelada
            && r.EstadoReserva != Estados.Reserva.CheckOut
            && r.FechaEntrada < fechaSalida
            && r.FechaSalida > fechaEntrada);

        return !haySolape;
    }
}
