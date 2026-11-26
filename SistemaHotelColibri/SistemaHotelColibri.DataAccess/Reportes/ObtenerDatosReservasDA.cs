using Microsoft.EntityFrameworkCore;
using SistemaHotelColibri.Abstract.Interfaces.DataAccess.Reportes;
using SistemaHotelColibri.Abstract.Modelos.Reportes;
using SistemaHotelColibri.Abstract.Modelos.Reservas;
using SistemaHotelColibri.DataAccess.Contexto;
using SistemaHotelColibri.DataAccess.Reservas;

namespace SistemaHotelColibri.DataAccess.Reportes;

// REP-005: reservas cuya fecha de entrada cae en el rango, con los filtros elegidos.
public class ObtenerDatosReservasDA : IObtenerDatosReservasDA
{
    private readonly HotelColibriContext _contexto;

    public ObtenerDatosReservasDA(HotelColibriContext contexto)
    {
        _contexto = contexto;
    }

    public async Task<List<ReservaDto>> ObtenerAsync(FiltroReporte filtro)
    {
        var consulta = _contexto.Reserva.AsNoTracking()
            .Include(r => r.IdClienteNavigation)
            .Include(r => r.IdHabitacionNavigation)
            .Where(r => r.FechaEntrada >= filtro.Desde && r.FechaEntrada <= filtro.Hasta);

        if (!string.IsNullOrWhiteSpace(filtro.Estado))
        {
            consulta = consulta.Where(r => r.EstadoReserva == filtro.Estado);
        }

        if (!string.IsNullOrWhiteSpace(filtro.TipoHabitacion))
        {
            consulta = consulta.Where(r => r.IdHabitacionNavigation.TipoHabitacion == filtro.TipoHabitacion);
        }

        if (!string.IsNullOrWhiteSpace(filtro.Busqueda))
        {
            var texto = filtro.Busqueda.Trim();
            consulta = consulta.Where(r => r.IdClienteNavigation.Nombre.Contains(texto)
                                        || r.IdClienteNavigation.PrimerApellido.Contains(texto)
                                        || r.IdClienteNavigation.Identificacion.Contains(texto)
                                        || r.IdHabitacionNavigation.NumeroHabitacion == texto);
        }

        var reservas = await consulta
            .OrderBy(r => r.FechaEntrada)
            .ThenBy(r => r.IdReserva)
            .ToListAsync();

        return reservas.Select(r => r.ADto()).ToList();
    }
}
