using Microsoft.EntityFrameworkCore;
using SistemaHotelColibri.Abstract.Comun;
using SistemaHotelColibri.Abstract.Interfaces.DataAccess.Habitaciones;
using SistemaHotelColibri.Abstract.Modelos.Habitaciones;
using SistemaHotelColibri.DataAccess.Comun;
using SistemaHotelColibri.DataAccess.Contexto;

namespace SistemaHotelColibri.DataAccess.Habitaciones;

public class ObtenerHabitacionDA : IObtenerHabitacionDA
{
    private readonly HotelColibriContext _contexto;

    public ObtenerHabitacionDA(HotelColibriContext contexto)
    {
        _contexto = contexto;
    }

    public async Task<ResultadoPaginado<HabitacionDto>> ObtenerAsync(FiltroListado filtro)
    {
        var consulta = _contexto.Habitacion.AsNoTracking();

        if (filtro.Estado == Estados.Habitacion.Activas)
        {
            consulta = consulta.Where(h => h.EstadoHabitacion != Estados.Habitacion.Inactiva);
        }
        else if (!string.IsNullOrWhiteSpace(filtro.Estado))
        {
            consulta = consulta.Where(h => h.EstadoHabitacion == filtro.Estado);
        }

        if (!string.IsNullOrWhiteSpace(filtro.Busqueda))
        {
            var texto = filtro.Busqueda;
            consulta = consulta.Where(h => h.NumeroHabitacion.Contains(texto)
                                        || h.TipoHabitacion.Contains(texto)
                                        || h.TipoCama.Contains(texto)
                                        || (h.Vista != null && h.Vista.Contains(texto))
                                        || (h.Descripcion != null && h.Descripcion.Contains(texto)));
        }

        var pagina = await consulta
            .OrderBy(h => h.NumeroHabitacion)
            .PaginarAsync(filtro.Pagina, filtro.TamanoPagina);

        return pagina.Mapear(h => h.ADto());
    }
}
