using Microsoft.EntityFrameworkCore;
using SistemaHotelColibri.Abstract.Interfaces.DataAccess.Reservas;
using SistemaHotelColibri.Abstract.Modelos.Reservas;
using SistemaHotelColibri.DataAccess.Contexto;

namespace SistemaHotelColibri.DataAccess.Reservas;

public class ObtenerReservaPorIdDA : IObtenerReservaPorIdDA
{
    private readonly HotelColibriContext _contexto;

    public ObtenerReservaPorIdDA(HotelColibriContext contexto)
    {
        _contexto = contexto;
    }

    public async Task<ReservaDto?> ObtenerPorIdAsync(int idReserva)
    {
        var reserva = await _contexto.Reserva
            .AsNoTracking()
            .Include(r => r.IdClienteNavigation)
            .Include(r => r.IdHabitacionNavigation)
            .FirstOrDefaultAsync(r => r.IdReserva == idReserva);

        return reserva?.ADto();
    }
}
