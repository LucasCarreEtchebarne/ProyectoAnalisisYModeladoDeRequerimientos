using SistemaHotelColibri.Abstract.Comun;
using SistemaHotelColibri.Abstract.Interfaces.DataAccess.Eventos;
using SistemaHotelColibri.DataAccess.Contexto;

namespace SistemaHotelColibri.DataAccess.Eventos;

public class EliminarEventoDA : IEliminarEventoDA
{
    private readonly HotelColibriContext _contexto;

    public EliminarEventoDA(HotelColibriContext contexto)
    {
        _contexto = contexto;
    }

    public async Task<bool> EliminarAsync(int idEvento)
    {
        var entidad = await _contexto.Evento.FindAsync(idEvento);
        if (entidad == null)
        {
            return false;
        }

        entidad.EstadoEvento = Estados.Evento.Cancelado;
        await _contexto.SaveChangesAsync();
        return true;
    }
}
