using Microsoft.EntityFrameworkCore;
using SistemaHotelColibri.Abstract.Comun;
using SistemaHotelColibri.Abstract.Interfaces.DataAccess.Facturas;
using SistemaHotelColibri.Abstract.Modelos.Facturas;
using SistemaHotelColibri.DataAccess.Contexto;
using SistemaHotelColibri.DataAccess.Entidades;

namespace SistemaHotelColibri.DataAccess.Facturas;

public class ObtenerPedidoFacturableDA : IObtenerPedidoFacturableDA
{
    private const int MaximoResultados = 50;

    private readonly HotelColibriContext _contexto;

    public ObtenerPedidoFacturableDA(HotelColibriContext contexto)
    {
        _contexto = contexto;
    }

    public async Task<List<PedidoFacturableDto>> ListarAsync(string? busqueda)
    {
        var consulta = Pendientes();

        if (!string.IsNullOrWhiteSpace(busqueda))
        {
            consulta = consulta.Where(p =>
                (p.IdClienteNavigation != null &&
                 (p.IdClienteNavigation.Identificacion.Contains(busqueda) ||
                  (p.IdClienteNavigation.Nombre + " " + p.IdClienteNavigation.PrimerApellido).Contains(busqueda))) ||
                (p.IdMesaNavigation != null && p.IdMesaNavigation.NumeroMesa.ToString() == busqueda) ||
                (p.IdHabitacionNavigation != null && p.IdHabitacionNavigation.NumeroHabitacion.Contains(busqueda)));
        }

        var pedidos = await ConDatos(consulta)
            .OrderByDescending(p => p.FechaHoraPedido)
            .Take(MaximoResultados)
            .ToListAsync();

        return pedidos.Select(p => p.ADtoFacturable(yaFacturado: false)).ToList();
    }

    public async Task<List<PedidoFacturableDto>> ListarParaReservaAsync(int idReserva, int idCliente)
    {
        var pedidos = await ConDatos(Pendientes().Where(p => p.IdReserva == idReserva || p.IdCliente == idCliente))
            .OrderBy(p => p.FechaHoraPedido)
            .ToListAsync();

        return pedidos.Select(p => p.ADtoFacturable(yaFacturado: false)).ToList();
    }

    public async Task<List<PedidoFacturableDto>> ObtenerAsync(IReadOnlyCollection<int> idPedidos)
    {
        var pedidos = await ConDatos(_contexto.Pedido.AsNoTracking().Where(p => idPedidos.Contains(p.IdPedido)))
            .Select(p => new
            {
                Pedido = p,
                YaFacturado = p.DetalleFactura.Any(d => d.IdFacturaNavigation.EstadoFactura != Estados.Factura.Anulada)
            })
            .ToListAsync();

        return pedidos.Select(x => x.Pedido.ADtoFacturable(x.YaFacturado)).ToList();
    }

    private IQueryable<Pedido> Pendientes() =>
        _contexto.Pedido
            .AsNoTracking()
            .Where(p => p.EstadoPedido == Estados.Pedido.Pendiente
                     && !p.DetalleFactura.Any(d => d.IdFacturaNavigation.EstadoFactura != Estados.Factura.Anulada));

    private static IQueryable<Pedido> ConDatos(IQueryable<Pedido> consulta) =>
        consulta
            .Include(p => p.IdClienteNavigation)
            .Include(p => p.IdMesaNavigation)
            .Include(p => p.IdHabitacionNavigation)
            .Include(p => p.DetallePedido)
                .ThenInclude(d => d.IdProductoMenuNavigation);
}

internal static class PedidoFacturableMapeo
{
    public static PedidoFacturableDto ADtoFacturable(this Pedido pedido, bool yaFacturado) => new()
    {
        IdPedido = pedido.IdPedido,
        IdCliente = pedido.IdCliente,
        NombreCliente = pedido.IdClienteNavigation?.NombreCompleto(),
        IdReserva = pedido.IdReserva,
        Origen = pedido.TipoPedido switch
        {
            "Mesa" => $"Mesa {pedido.IdMesaNavigation?.NumeroMesa}",
            "Habitacion" => $"Habitación {pedido.IdHabitacionNavigation?.NumeroHabitacion}",
            _ => "Para llevar"
        },
        FechaHoraPedido = pedido.FechaHoraPedido,
        EstadoPedido = pedido.EstadoPedido,
        YaFacturado = yaFacturado,
        Lineas = pedido.DetallePedido
            .OrderBy(d => d.IdDetallePedido)
            .Select(d => new LineaPedidoFacturableDto
            {
                NombreProducto = d.IdProductoMenuNavigation.NombreProducto,
                Cantidad = d.Cantidad,
                PrecioUnitario = d.PrecioUnitario
            })
            .ToList()
    };
}
