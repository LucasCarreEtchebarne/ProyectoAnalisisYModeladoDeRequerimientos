using SistemaHotelColibri.Abstract.Comun;
using SistemaHotelColibri.Abstract.Interfaces.BusinessLogic.Reportes;
using SistemaHotelColibri.Abstract.Interfaces.DataAccess.Reportes;
using SistemaHotelColibri.Abstract.Modelos.Reportes;

namespace SistemaHotelColibri.BusinessLogic.Reportes;

// REP-005: listado de reservas con los filtros elegidos y sus totales por estado.
public class ObtenerReporteReservasBL : IObtenerReporteReservasBL
{
    private readonly IObtenerDatosReservasDA _datosDA;

    public ObtenerReporteReservasBL(IObtenerDatosReservasDA datosDA)
    {
        _datosDA = datosDA;
    }

    public async Task<ReporteReservasDto> ObtenerAsync(FiltroReporte filtro)
    {
        ReglasReporte.Normalizar(filtro);
        var reservas = await _datosDA.ObtenerAsync(filtro);

        // Las canceladas se listan, pero no suman noches, huéspedes ni dinero.
        var validas = reservas.Where(r => r.EstadoReserva != Estados.Reserva.Cancelada).ToList();

        return new ReporteReservasDto
        {
            Desde = filtro.Desde,
            Hasta = filtro.Hasta,
            TotalReservas = reservas.Count,
            TotalNoches = validas.Sum(r => r.CantidadNoches),
            TotalHuespedes = validas.Sum(r => r.CantidadHuespedes),
            MontoTotal = validas.Sum(r => r.MontoHospedaje),
            Reservas = reservas,
            PorEstado = Estados.Reserva.Todos
                .Select(estado =>
                {
                    var grupo = reservas.Where(r => r.EstadoReserva == estado).ToList();
                    return new ReservaEstadoReporteDto
                    {
                        Estado = estado,
                        Reservas = grupo.Count,
                        Noches = grupo.Sum(r => r.CantidadNoches),
                        Monto = grupo.Sum(r => r.MontoHospedaje)
                    };
                })
                .Where(e => e.Reservas > 0)
                .ToList()
        };
    }
}
