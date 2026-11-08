using Microsoft.EntityFrameworkCore;
using SistemaHotelColibri.Abstract.Interfaces.DataAccess.Clientes;
using SistemaHotelColibri.Abstract.Modelos.Clientes;
using SistemaHotelColibri.DataAccess.Contexto;
using SistemaHotelColibri.DataAccess.Pedidos;
using SistemaHotelColibri.DataAccess.Reservas;

namespace SistemaHotelColibri.DataAccess.Clientes;

public class ObtenerHistorialClienteDA : IObtenerHistorialClienteDA
{
    private readonly HotelColibriContext _contexto;

    public ObtenerHistorialClienteDA(HotelColibriContext contexto)
    {
        _contexto = contexto;
    }

    public async Task<HistorialClienteDto> ObtenerAsync(int idCliente)
    {
        var reservas = await _contexto.Reserva
            .AsNoTracking()
            .Include(r => r.IdClienteNavigation)
            .Include(r => r.IdHabitacionNavigation)
            .Where(r => r.IdCliente == idCliente)
            .OrderByDescending(r => r.FechaEntrada)
            .ToListAsync();

        var pedidos = await _contexto.Pedido
            .AsNoTracking()
            .Include(p => p.IdClienteNavigation)
            .Include(p => p.DetallePedido)
                .ThenInclude(d => d.IdProductoMenuNavigation)
            .Where(p => p.IdCliente == idCliente)
            .OrderByDescending(p => p.FechaHoraPedido)
            .ToListAsync();

        var facturas = await _contexto.Factura
            .AsNoTracking()
            .Where(f => f.IdCliente == idCliente)
            .OrderByDescending(f => f.FechaEmision)
            .Select(f => new FacturaHistorialDto
            {
                IdFactura = f.IdFactura,
                NumeroFactura = f.NumeroFactura,
                FechaEmision = f.FechaEmision,
                TipoFactura = f.TipoFactura,
                MontoTotal = f.MontoTotal,
                EstadoFactura = f.EstadoFactura
            })
            .ToListAsync();

        return new HistorialClienteDto
        {
            Reservas = reservas.Select(r => r.ADto()).ToList(),
            Pedidos = pedidos.Select(p => p.ADto()).ToList(),
            Facturas = facturas
        };
    }
}