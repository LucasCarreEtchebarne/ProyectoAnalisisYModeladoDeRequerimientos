using Microsoft.EntityFrameworkCore;
using SistemaHotelColibri.Abstract.Comun;
using SistemaHotelColibri.Abstract.Interfaces.DataAccess.Reservas;
using SistemaHotelColibri.Abstract.Modelos.Reservas;
using SistemaHotelColibri.DataAccess.Contexto;

namespace SistemaHotelColibri.DataAccess.Reservas;

public class ObtenerOpcionesReservaDA : IObtenerOpcionesReservaDA
{
    private readonly HotelColibriContext _contexto;

    public ObtenerOpcionesReservaDA(HotelColibriContext contexto)
    {
        _contexto = contexto;
    }

    public async Task<OpcionesReservaDto> ObtenerAsync(int idClienteActual, int idHabitacionActual)
    {
        var clientes = await _contexto.Cliente.AsNoTracking()
            .Where(c => c.EstadoCliente == Estados.Cliente.Activo || c.IdCliente == idClienteActual)
            .OrderBy(c => c.Nombre).ThenBy(c => c.PrimerApellido)
            .Select(c => new ClienteOpcionDto
            {
                IdCliente = c.IdCliente,
                Texto = c.Nombre + " " + c.PrimerApellido + " (" + c.Identificacion + ")"
            })
            .ToListAsync();

        var habitaciones = await _contexto.Habitacion.AsNoTracking()
            .Where(h => h.EstadoHabitacion != Estados.Habitacion.Inactiva || h.IdHabitacion == idHabitacionActual)
            .OrderBy(h => h.NumeroHabitacion)
            .Select(h => new HabitacionOpcionDto
            {
                IdHabitacion = h.IdHabitacion,
                Texto = h.NumeroHabitacion + " - " + h.TipoHabitacion + " (cap. " + h.Capacidad + ")",
                Precio = h.Precio,
                Capacidad = h.Capacidad
            })
            .ToListAsync();

        return new OpcionesReservaDto { Clientes = clientes, Habitaciones = habitaciones };
    }
}
