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
            .OrderBy(h => h.NumeroHabitacion.Length)
            .ThenBy(h => h.NumeroHabitacion)
            .Select(h => new HabitacionOpcionDto
            {
                IdHabitacion = h.IdHabitacion,
                Texto = h.NumeroHabitacion + " - " + h.TipoHabitacion + " (cap. " + h.Capacidad + ")",
                Precio = h.Precio,
                Capacidad = h.Capacidad
            })
            .ToListAsync();

        // Fechas ya tomadas de cada habitación, para avisar en el formulario antes de guardar.
        var hoy = DateOnly.FromDateTime(DateTime.Today);
        var tomadas = await _contexto.Reserva.AsNoTracking()
            .Where(r => r.EstadoReserva != Estados.Reserva.Cancelada
                     && r.EstadoReserva != Estados.Reserva.CheckOut
                     && r.FechaSalida > hoy)
            .OrderBy(r => r.FechaEntrada)
            .Select(r => new
            {
                r.IdHabitacion,
                Rango = new RangoOcupadoDto
                {
                    IdReserva = r.IdReserva,
                    FechaEntrada = r.FechaEntrada,
                    FechaSalida = r.FechaSalida,
                    Cliente = r.IdClienteNavigation.Nombre + " " + r.IdClienteNavigation.PrimerApellido
                }
            })
            .ToListAsync();

        foreach (var habitacion in habitaciones)
        {
            habitacion.Ocupado = tomadas.Where(t => t.IdHabitacion == habitacion.IdHabitacion).Select(t => t.Rango).ToList();
        }

        return new OpcionesReservaDto { Clientes = clientes, Habitaciones = habitaciones };
    }
}
