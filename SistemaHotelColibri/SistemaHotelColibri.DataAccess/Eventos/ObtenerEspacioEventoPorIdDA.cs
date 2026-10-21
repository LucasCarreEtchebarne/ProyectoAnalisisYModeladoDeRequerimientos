using Microsoft.EntityFrameworkCore;
using SistemaHotelColibri.Abstract.Interfaces.DataAccess.Eventos;
using SistemaHotelColibri.Abstract.Modelos.Eventos;
using SistemaHotelColibri.DataAccess.Contexto;

namespace SistemaHotelColibri.DataAccess.Eventos;

public class ObtenerEspacioEventoPorIdDA : IObtenerEspacioEventoPorIdDA
{
    private readonly HotelColibriContext _contexto;

    public ObtenerEspacioEventoPorIdDA(HotelColibriContext contexto)
    {
        _contexto = contexto;
    }

    public async Task<EspacioEventoDto?> ObtenerPorIdAsync(int idEspacio)
    {
        return await _contexto.EspacioEvento
            .AsNoTracking()
            .Where(s => s.IdEspacio == idEspacio)
            .Select(s => new EspacioEventoDto
            {
                IdEspacio = s.IdEspacio,
                NombreEspacio = s.NombreEspacio,
                CapacidadMaxima = s.CapacidadMaxima,
                EstadoEspacio = s.EstadoEspacio
            })
            .FirstOrDefaultAsync();
    }
}
