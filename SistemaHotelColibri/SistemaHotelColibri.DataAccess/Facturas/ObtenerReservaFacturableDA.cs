using Microsoft.EntityFrameworkCore;
using SistemaHotelColibri.Abstract.Comun;
using SistemaHotelColibri.Abstract.Interfaces.DataAccess.Facturas;
using SistemaHotelColibri.Abstract.Modelos.Facturas;
using SistemaHotelColibri.DataAccess.Contexto;

namespace SistemaHotelColibri.DataAccess.Facturas;

public class ObtenerReservaFacturableDA : IObtenerReservaFacturableDA
{
    private const int MaximoResultados = 50;

    private readonly HotelColibriContext _contexto;

    public ObtenerReservaFacturableDA(HotelColibriContext contexto)
    {
        _contexto = contexto;
    }

    public async Task<List<ReservaFacturableDto>> ListarAsync(string? busqueda)
    {
        var consulta = Proyectar()
            .Where(r => Facturacion.EstadosReservaFacturables.Contains(r.EstadoReserva) && !r.YaFacturada);

        if (!string.IsNullOrWhiteSpace(busqueda))
        {
            consulta = consulta.Where(r => r.NombreCliente.Contains(busqueda)
                                        || r.IdentificacionCliente.Contains(busqueda)
                                        || r.NumeroHabitacion.Contains(busqueda));
        }

        return await consulta
            .OrderBy(r => r.FechaSalida)
            .ThenBy(r => r.NumeroHabitacion)
            .Take(MaximoResultados)
            .ToListAsync();
    }

    public Task<ReservaFacturableDto?> ObtenerAsync(int idReserva) =>
        Proyectar().FirstOrDefaultAsync(r => r.IdReserva == idReserva);

    private IQueryable<ReservaFacturableDto> Proyectar() =>
        _contexto.Reserva
            .AsNoTracking()
            .Select(r => new ReservaFacturableDto
            {
                IdReserva = r.IdReserva,
                IdCliente = r.IdCliente,
                NombreCliente = (r.IdClienteNavigation.Nombre + " " + r.IdClienteNavigation.PrimerApellido + " " +
                                 (r.IdClienteNavigation.SegundoApellido ?? "")).Trim(),
                IdentificacionCliente = r.IdClienteNavigation.Identificacion,
                NumeroHabitacion = r.IdHabitacionNavigation.NumeroHabitacion,
                FechaEntrada = r.FechaEntrada,
                FechaSalida = r.FechaSalida,
                CantidadNoches = r.CantidadNoches ?? 0,
                PrecioNoche = r.PrecioNoche,
                EstadoReserva = r.EstadoReserva,
                YaFacturada = r.DetalleFactura.Any(d => d.IdFacturaNavigation.EstadoFactura != Estados.Factura.Anulada)
            });
}
