using Microsoft.EntityFrameworkCore;
using SistemaHotelColibri.Abstract.Comun;
using SistemaHotelColibri.Abstract.Interfaces.DataAccess.Facturas;
using SistemaHotelColibri.DataAccess.Contexto;

namespace SistemaHotelColibri.DataAccess.Facturas;

public class AnularFacturaDA : IAnularFacturaDA
{
    private readonly HotelColibriContext _contexto;

    public AnularFacturaDA(HotelColibriContext contexto)
    {
        _contexto = contexto;
    }

    public async Task<bool> AnularAsync(int idFactura, string motivo, DateTime fechaAnulacion)
    {
        await using var transaccion = await _contexto.Database.BeginTransactionAsync();

        var actualizadas = await _contexto.Factura
            .Where(f => f.IdFactura == idFactura && f.EstadoFactura != Estados.Factura.Anulada)
            .ExecuteUpdateAsync(s => s
                .SetProperty(f => f.EstadoFactura, Estados.Factura.Anulada)
                .SetProperty(f => f.MotivoAnulacion, motivo)
                .SetProperty(f => f.FechaAnulacion, fechaAnulacion));

        if (actualizadas == 0)
        {
            return false;
        }

        await _contexto.Pago
            .Where(p => p.IdFactura == idFactura && p.EstadoPago == Estados.Pago.Aprobado)
            .ExecuteUpdateAsync(s => s.SetProperty(p => p.EstadoPago, Estados.Pago.Reversado));

        await _contexto.Pedido
            .Where(p => p.EstadoPedido == Estados.Pedido.Entregado
                     && p.DetalleFactura.Any(d => d.IdFactura == idFactura))
            .ExecuteUpdateAsync(s => s.SetProperty(p => p.EstadoPedido, Estados.Pedido.Pendiente));

        await transaccion.CommitAsync();
        return true;
    }
}
