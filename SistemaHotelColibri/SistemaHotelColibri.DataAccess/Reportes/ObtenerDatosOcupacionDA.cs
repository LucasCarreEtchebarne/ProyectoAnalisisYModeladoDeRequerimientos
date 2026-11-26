using Microsoft.EntityFrameworkCore;
using SistemaHotelColibri.Abstract.Comun;
using SistemaHotelColibri.Abstract.Interfaces.DataAccess.Reportes;
using SistemaHotelColibri.Abstract.Modelos.Reportes;
using SistemaHotelColibri.DataAccess.Contexto;
using SistemaHotelColibri.DataAccess.Habitaciones;
using SistemaHotelColibri.DataAccess.Reservas;

namespace SistemaHotelColibri.DataAccess.Reportes;

// REP-001: habitaciones activas y las reservas (no canceladas) que tocan alguna noche del rango.
public class ObtenerDatosOcupacionDA : IObtenerDatosOcupacionDA
{
    private readonly HotelColibriContext _contexto;

    public ObtenerDatosOcupacionDA(HotelColibriContext contexto)
    {
        _contexto = contexto;
    }

    public async Task<DatosOcupacionDto> ObtenerAsync(FiltroReporte filtro)
    {
        var habitaciones = _contexto.Habitacion.AsNoTracking()
            .Where(h => h.EstadoHabitacion != Estados.Habitacion.Inactiva);

        if (!string.IsNullOrWhiteSpace(filtro.TipoHabitacion))
        {
            habitaciones = habitaciones.Where(h => h.TipoHabitacion == filtro.TipoHabitacion);
        }

        var listaHabitaciones = await habitaciones
            .OrderBy(h => h.NumeroHabitacion.Length)
            .ThenBy(h => h.NumeroHabitacion)
            .ToListAsync();

        var ids = listaHabitaciones.Select(h => h.IdHabitacion).ToList();
        var hastaExclusivo = filtro.Hasta.AddDays(1);

        var reservas = await _contexto.Reserva.AsNoTracking()
            .Include(r => r.IdClienteNavigation)
            .Include(r => r.IdHabitacionNavigation)
            .Where(r => ids.Contains(r.IdHabitacion)
                     && r.EstadoReserva != Estados.Reserva.Cancelada
                     && r.FechaEntrada < hastaExclusivo
                     && r.FechaSalida > filtro.Desde)
            .ToListAsync();

        return new DatosOcupacionDto
        {
            Habitaciones = listaHabitaciones.Select(h => h.ADto()).ToList(),
            Reservas = reservas.Select(r => r.ADto()).ToList()
        };
    }
}
