using Microsoft.EntityFrameworkCore;
using SistemaHotelColibri.Abstract.Comun;
using SistemaHotelColibri.Abstract.Interfaces.DataAccess.Facturas;
using SistemaHotelColibri.Abstract.Modelos.Facturas;
using SistemaHotelColibri.DataAccess.Contexto;
using SistemaHotelColibri.DataAccess.Entidades;

namespace SistemaHotelColibri.DataAccess.Facturas;

public class RegistrarFacturaDA : IRegistrarFacturaDA
{
    private readonly HotelColibriContext _contexto;

    public RegistrarFacturaDA(HotelColibriContext contexto)
    {
        _contexto = contexto;
    }

    public async Task<int> RegistrarAsync(FacturaDto dto)
    {
        await using var transaccion = await _contexto.Database.BeginTransactionAsync();

        var factura = new Factura
        {
            IdCliente = dto.IdCliente,
            IdUsuario = dto.IdUsuario,
            FechaEmision = dto.FechaEmision,
            TipoFactura = dto.TipoFactura,
            Subtotal = dto.Subtotal,
            MontoServicio = dto.MontoServicio,
            MontoIva = dto.MontoIva,
            MontoTotal = dto.MontoTotal,
            EstadoFactura = Estados.Factura.Pendiente,
            DetalleFactura = dto.Detalles.Select(d => new DetalleFactura
            {
                TipoConcepto = d.TipoConcepto,
                IdReserva = d.IdReserva,
                IdPedido = d.IdPedido,
                IdEvento = d.IdEvento,
                Descripcion = d.Descripcion,
                Cantidad = d.Cantidad,
                PrecioUnitario = d.PrecioUnitario
            }).ToList()
        };

        _contexto.Factura.Add(factura);
        await _contexto.SaveChangesAsync();

        var idPedidos = dto.Detalles.Where(d => d.IdPedido.HasValue).Select(d => d.IdPedido!.Value).Distinct().ToList();
        if (idPedidos.Count > 0)
        {
            await _contexto.Pedido
                .Where(p => idPedidos.Contains(p.IdPedido))
                .ExecuteUpdateAsync(s => s.SetProperty(p => p.EstadoPedido, Estados.Pedido.Entregado));
        }

        await transaccion.CommitAsync();
        return factura.IdFactura;
    }
}
