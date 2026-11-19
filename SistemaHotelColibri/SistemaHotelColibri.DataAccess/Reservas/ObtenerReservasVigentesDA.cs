using Microsoft.EntityFrameworkCore;
using SistemaHotelColibri.Abstract.Comun;
using SistemaHotelColibri.Abstract.Interfaces.DataAccess.Reservas;
using SistemaHotelColibri.Abstract.Modelos.Reservas;
using SistemaHotelColibri.DataAccess.Contexto;

namespace SistemaHotelColibri.DataAccess.Reservas;

public class ObtenerReservasVigentesDA : IObtenerReservasVigentesDA
{
    private readonly HotelColibriContext _contexto;

    public ObtenerReservasVigentesDA(HotelColibriContext contexto)
    {
        _contexto = contexto;
    }

    public async Task<List<ReservaDto>> ObtenerAsync(DateOnly hoy)
    {
        var reservas = await _contexto.Reserva
            .AsNoTracking()
            .Include(r => r.IdClienteNavigation)
            .Include(r => r.IdHabitacionNavigation)
            .Where(r => r.EstadoReserva == Estados.Reserva.CheckIn
                     || ((r.EstadoReserva == Estados.Reserva.Pendiente || r.EstadoReserva == Estados.Reserva.Confirmada)
                         && r.FechaSalida > hoy))
            .OrderBy(r => r.FechaEntrada)
            .ToListAsync();

        return reservas.Select(r => r.ADto()).ToList();
    }
}
