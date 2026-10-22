using Microsoft.EntityFrameworkCore;
using SistemaHotelColibri.Abstract.Comun;
using SistemaHotelColibri.Abstract.Interfaces.DataAccess.Eventos;
using SistemaHotelColibri.DataAccess.Contexto;

namespace SistemaHotelColibri.DataAccess.Eventos;

public class EspacioDisponibleEventoDA : IEspacioDisponibleEventoDA
{
    private readonly HotelColibriContext _contexto;

    public EspacioDisponibleEventoDA(HotelColibriContext contexto)
    {
        _contexto = contexto;
    }

    public async Task<bool> EstaDisponibleAsync(int idEspacio, DateOnly fecha, TimeOnly horaInicio, TimeOnly horaFin, int idEventoExcluido)
    {
        var haySolape = await _contexto.Evento.AnyAsync(e =>
            e.IdEspacio == idEspacio
            && e.IdEvento != idEventoExcluido
            && e.EstadoEvento != Estados.Evento.Cancelado
            && e.FechaEvento == fecha
            && e.HoraInicio < horaFin
            && e.HoraFin > horaInicio);

        return !haySolape;
    }
}
