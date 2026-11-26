using SistemaHotelColibri.Abstract.Interfaces.BusinessLogic.Reportes;
using SistemaHotelColibri.Abstract.Interfaces.DataAccess.Reportes;
using SistemaHotelColibri.Abstract.Modelos.Reportes;
using SistemaHotelColibri.Abstract.Modelos.Reservas;

namespace SistemaHotelColibri.BusinessLogic.Reportes;

// REP-001: cuántas noches de habitación se ocuparon en el rango, por habitación, por tipo y por día.
public class ObtenerReporteOcupacionBL : IObtenerReporteOcupacionBL
{
    private readonly IObtenerDatosOcupacionDA _datosDA;

    public ObtenerReporteOcupacionBL(IObtenerDatosOcupacionDA datosDA)
    {
        _datosDA = datosDA;
    }

    public async Task<ReporteOcupacionDto> ObtenerAsync(FiltroReporte filtro)
    {
        ReglasReporte.Normalizar(filtro);
        var datos = await _datosDA.ObtenerAsync(filtro);
        var dias = filtro.Dias;

        // Noches de cada reserva que caen dentro del rango (la noche del día de salida no cuenta).
        int NochesEnRango(ReservaDto r)
        {
            var inicio = r.FechaEntrada > filtro.Desde ? r.FechaEntrada : filtro.Desde;
            var fin = r.FechaSalida < filtro.Hasta.AddDays(1) ? r.FechaSalida : filtro.Hasta.AddDays(1);
            return Math.Max(0, fin.DayNumber - inicio.DayNumber);
        }

        var porHabitacion = datos.Habitaciones.Select(h =>
        {
            var reservas = datos.Reservas.Where(r => r.IdHabitacion == h.IdHabitacion).ToList();
            var noches = Math.Min(dias, reservas.Sum(NochesEnRango));
            return new OcupacionHabitacionReporteDto
            {
                NumeroHabitacion = h.NumeroHabitacion,
                TipoHabitacion = h.TipoHabitacion,
                EstadoActual = h.EstadoHabitacion,
                Reservas = reservas.Count,
                NochesOcupadas = noches,
                PorcentajeOcupacion = ReglasReporte.Porcentaje(noches, dias),
                Ingreso = reservas.Sum(r => NochesEnRango(r) * r.PrecioNoche)
            };
        }).ToList();

        var totalHabitaciones = datos.Habitaciones.Count;
        var nochesDisponibles = totalHabitaciones * dias;
        var nochesOcupadas = porHabitacion.Sum(h => h.NochesOcupadas);

        var porTipo = porHabitacion
            .GroupBy(h => h.TipoHabitacion)
            .Select(g => new OcupacionTipoReporteDto
            {
                TipoHabitacion = g.Key,
                Habitaciones = g.Count(),
                NochesOcupadas = g.Sum(h => h.NochesOcupadas),
                NochesDisponibles = g.Count() * dias,
                PorcentajeOcupacion = ReglasReporte.Porcentaje(g.Sum(h => h.NochesOcupadas), g.Count() * dias),
                Ingreso = g.Sum(h => h.Ingreso)
            })
            .OrderByDescending(t => t.PorcentajeOcupacion)
            .ToList();

        var porDia = Enumerable.Range(0, dias)
            .Select(i => filtro.Desde.AddDays(i))
            .Select(fecha =>
            {
                var ocupadas = datos.Reservas
                    .Where(r => r.FechaEntrada <= fecha && r.FechaSalida > fecha)
                    .Select(r => r.IdHabitacion)
                    .Distinct()
                    .Count();
                return new OcupacionDiaReporteDto
                {
                    Fecha = fecha,
                    Ocupadas = ocupadas,
                    Total = totalHabitaciones,
                    PorcentajeOcupacion = ReglasReporte.Porcentaje(ocupadas, totalHabitaciones)
                };
            })
            .ToList();

        return new ReporteOcupacionDto
        {
            Desde = filtro.Desde,
            Hasta = filtro.Hasta,
            Dias = dias,
            TotalHabitaciones = totalHabitaciones,
            NochesDisponibles = nochesDisponibles,
            NochesOcupadas = nochesOcupadas,
            PorcentajeOcupacion = ReglasReporte.Porcentaje(nochesOcupadas, nochesDisponibles),
            CantidadReservas = datos.Reservas.Count,
            IngresoHospedaje = porHabitacion.Sum(h => h.Ingreso),
            Habitaciones = porHabitacion,
            PorTipo = porTipo,
            PorDia = porDia
        };
    }
}
