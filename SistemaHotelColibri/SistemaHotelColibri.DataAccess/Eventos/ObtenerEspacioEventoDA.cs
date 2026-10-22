using Microsoft.EntityFrameworkCore;
using SistemaHotelColibri.Abstract.Interfaces.DataAccess.Eventos;
using SistemaHotelColibri.Abstract.Modelos.Eventos;
using SistemaHotelColibri.DataAccess.Contexto;

namespace SistemaHotelColibri.DataAccess.Eventos;

public class ObtenerEspacioEventoDA : IObtenerEspacioEventoDA
{
    private readonly HotelColibriContext _contexto;

    public ObtenerEspacioEventoDA(HotelColibriContext contexto)
    {
        _contexto = contexto;
    }

    public async Task<List<EspacioEventoDto>> ObtenerAsync(string? estado)
    {
        var consulta = _contexto.EspacioEvento.AsNoTracking();

        if (!string.IsNullOrWhiteSpace(estado))
        {
            consulta = consulta.Where(e => e.EstadoEspacio == estado);
        }

        var espacios = await consulta.OrderBy(e => e.NombreEspacio).ToListAsync();
        return espacios.Select(e => e.ADto()).ToList();
    }
}
