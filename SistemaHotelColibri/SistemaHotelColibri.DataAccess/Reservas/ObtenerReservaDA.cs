using Microsoft.EntityFrameworkCore;
using SistemaHotelColibri.Abstract.Comun;
using SistemaHotelColibri.Abstract.Interfaces.DataAccess.Reservas;
using SistemaHotelColibri.Abstract.Modelos.Reservas;
using SistemaHotelColibri.DataAccess.Comun;
using SistemaHotelColibri.DataAccess.Contexto;

namespace SistemaHotelColibri.DataAccess.Reservas;

public class ObtenerReservaDA : IObtenerReservaDA
{
    private readonly HotelColibriContext _contexto;

    public ObtenerReservaDA(HotelColibriContext contexto)
    {
        _contexto = contexto;
    }

    public async Task<ResultadoPaginado<ReservaDto>> ObtenerAsync(FiltroListado filtro)
    {
        var consulta = _contexto.Reserva
            .AsNoTracking()
            .Include(r => r.IdClienteNavigation)
            .Include(r => r.IdHabitacionNavigation)
            .AsQueryable();

        if (!string.IsNullOrWhiteSpace(filtro.Estado))
        {
            consulta = consulta.Where(r => r.EstadoReserva == filtro.Estado);
        }

        if (!string.IsNullOrWhiteSpace(filtro.Busqueda))
        {
            var texto = filtro.Busqueda;
            consulta = consulta.Where(r => r.IdClienteNavigation.Nombre.Contains(texto)
                                        || r.IdClienteNavigation.PrimerApellido.Contains(texto)
                                        || r.IdClienteNavigation.Identificacion.Contains(texto)
                                        || r.IdHabitacionNavigation.NumeroHabitacion.Contains(texto)
                                        || r.IdReserva.ToString() == texto);
        }

        var pagina = await consulta
            .OrderByDescending(r => r.FechaEntrada)
            .ThenByDescending(r => r.IdReserva)
            .PaginarAsync(filtro.Pagina, filtro.TamanoPagina);

        return pagina.Mapear(r => r.ADto());
    }
}
