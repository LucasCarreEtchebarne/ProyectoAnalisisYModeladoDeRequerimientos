using Microsoft.EntityFrameworkCore;
using SistemaHotelColibri.Abstract.Comun;
using SistemaHotelColibri.Abstract.Interfaces.DataAccess.Eventos;
using SistemaHotelColibri.Abstract.Modelos.Eventos;
using SistemaHotelColibri.DataAccess.Comun;
using SistemaHotelColibri.DataAccess.Contexto;

namespace SistemaHotelColibri.DataAccess.Eventos;

public class ObtenerEventoDA : IObtenerEventoDA
{
    private readonly HotelColibriContext _contexto;

    public ObtenerEventoDA(HotelColibriContext contexto)
    {
        _contexto = contexto;
    }

    public async Task<ResultadoPaginado<EventoDto>> ObtenerAsync(FiltroListado filtro)
    {
        var consulta = _contexto.Evento
            .AsNoTracking()
            .Include(e => e.IdClienteNavigation)
            .Include(e => e.IdEspacioNavigation)
            .AsQueryable();

        if (!string.IsNullOrWhiteSpace(filtro.Estado))
        {
            consulta = consulta.Where(e => e.EstadoEvento == filtro.Estado);
        }

        if (!string.IsNullOrWhiteSpace(filtro.Busqueda))
        {
            var texto = filtro.Busqueda;
            consulta = consulta.Where(e => e.NombreEvento.Contains(texto)
                                        || e.IdClienteNavigation.Nombre.Contains(texto)
                                        || e.IdClienteNavigation.PrimerApellido.Contains(texto)
                                        || e.IdClienteNavigation.Identificacion.Contains(texto)
                                        || e.IdEspacioNavigation.NombreEspacio.Contains(texto));
        }

        var pagina = await consulta
            .OrderByDescending(e => e.FechaEvento)
            .ThenBy(e => e.HoraInicio)
            .PaginarAsync(filtro.Pagina, filtro.TamanoPagina);

        return pagina.Mapear(e => e.ADto());
    }
}
