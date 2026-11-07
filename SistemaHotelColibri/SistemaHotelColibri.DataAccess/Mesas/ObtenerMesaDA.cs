using Microsoft.EntityFrameworkCore;
using SistemaHotelColibri.Abstract.Comun;
using SistemaHotelColibri.Abstract.Interfaces.DataAccess.Mesas;
using SistemaHotelColibri.Abstract.Modelos.Mesas;
using SistemaHotelColibri.DataAccess.Comun;
using SistemaHotelColibri.DataAccess.Contexto;

namespace SistemaHotelColibri.DataAccess.Mesas;

public class ObtenerMesaDA : IObtenerMesaDA
{
    private readonly HotelColibriContext _contexto;

    public ObtenerMesaDA(HotelColibriContext contexto)
    {
        _contexto = contexto;
    }

    public async Task<ResultadoPaginado<MesaDto>> ObtenerAsync(FiltroListado filtro)
    {
        var consulta = _contexto.Mesa.AsNoTracking();

        if (filtro.Estado == Estados.Mesa.Activas)
        {
            consulta = consulta.Where(m => m.EstadoMesa != Estados.Mesa.Inactiva);
        }
        else if (!string.IsNullOrWhiteSpace(filtro.Estado))
        {
            consulta = consulta.Where(m => m.EstadoMesa == filtro.Estado);
        }

        if (!string.IsNullOrWhiteSpace(filtro.Busqueda))
        {
            var texto = filtro.Busqueda;
            consulta = consulta.Where(m => m.NumeroMesa.ToString().Contains(texto)
                                        || m.EstadoMesa.Contains(texto)
                                        || (m.Descripcion != null && m.Descripcion.Contains(texto)));
        }

        var pagina = await consulta
            .OrderBy(m => m.NumeroMesa)
            .PaginarAsync(filtro.Pagina, filtro.TamanoPagina);

        return pagina.Mapear(m => m.ADto());
    }
}
