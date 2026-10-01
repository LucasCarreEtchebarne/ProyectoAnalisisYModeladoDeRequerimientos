using Microsoft.EntityFrameworkCore;
using SistemaHotelColibri.Abstract.Comun;
using SistemaHotelColibri.Abstract.Interfaces.DataAccess.Habitaciones;
using SistemaHotelColibri.Abstract.Modelos.Habitaciones;
using SistemaHotelColibri.DataAccess.Contexto;

namespace SistemaHotelColibri.DataAccess.Habitaciones;

public class ObtenerHabitacionDA : IObtenerHabitacionDA
{
    private readonly HotelColibriContext _contexto;

    public ObtenerHabitacionDA(HotelColibriContext contexto)
    {
        _contexto = contexto;
    }

    public async Task<List<HabitacionDto>> ObtenerAsync(string? estado)
    {
        var consulta = _contexto.Habitacion.AsNoTracking();

        if (string.IsNullOrWhiteSpace(estado))
        {
            consulta = consulta.Where(h => h.EstadoHabitacion != Estados.Habitacion.Inactiva);
        }
        else
        {
            consulta = consulta.Where(h => h.EstadoHabitacion == estado);
        }

        var habitaciones = await consulta.OrderBy(h => h.NumeroHabitacion).ToListAsync();
        return habitaciones.Select(h => h.ADto()).ToList();
    }
}
