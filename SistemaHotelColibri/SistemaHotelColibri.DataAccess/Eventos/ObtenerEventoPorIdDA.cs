using Microsoft.EntityFrameworkCore;
using SistemaHotelColibri.Abstract.Interfaces.DataAccess.Eventos;
using SistemaHotelColibri.Abstract.Modelos.Eventos;
using SistemaHotelColibri.DataAccess.Contexto;

namespace SistemaHotelColibri.DataAccess.Eventos;

public class ObtenerEventoPorIdDA : IObtenerEventoPorIdDA
{
    private readonly HotelColibriContext _contexto;

    public ObtenerEventoPorIdDA(HotelColibriContext contexto)
    {
        _contexto = contexto;
    }

    public async Task<EventoDto?> ObtenerPorIdAsync(int idEvento)
    {
        var evento = await _contexto.Evento
            .AsNoTracking()
            .Include(e => e.IdClienteNavigation)
            .Include(e => e.IdEspacioNavigation)
            .FirstOrDefaultAsync(e => e.IdEvento == idEvento);

        return evento?.ADto();
    }
}
