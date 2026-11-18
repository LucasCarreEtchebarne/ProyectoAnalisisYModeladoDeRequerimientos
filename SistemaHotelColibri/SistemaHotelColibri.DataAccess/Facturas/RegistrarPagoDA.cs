using Microsoft.EntityFrameworkCore;
using SistemaHotelColibri.Abstract.Comun;
using SistemaHotelColibri.Abstract.Interfaces.DataAccess.Facturas;
using SistemaHotelColibri.Abstract.Modelos.Facturas;
using SistemaHotelColibri.DataAccess.Contexto;
using SistemaHotelColibri.DataAccess.Entidades;

namespace SistemaHotelColibri.DataAccess.Facturas;

public class RegistrarPagoDA : IRegistrarPagoDA
{
    private readonly HotelColibriContext _contexto;

    public RegistrarPagoDA(HotelColibriContext contexto)
    {
        _contexto = contexto;
    }

    public async Task<int> RegistrarAsync(PagoDto dto, bool facturaQuedaPagada)
    {
        await using var transaccion = await _contexto.Database.BeginTransactionAsync();

        var pago = new Pago
        {
            IdFactura = dto.IdFactura,
            IdUsuario = dto.IdUsuario,
            MetodoPago = dto.MetodoPago,
            MontoPagado = dto.MontoPagado,
            MontoRecibido = dto.MontoRecibido,
            CambioDevuelto = dto.CambioDevuelto,
            TipoTarjeta = dto.TipoTarjeta,
            UltimosCuatroDigitos = dto.UltimosCuatroDigitos,
            NumeroReferencia = dto.NumeroReferencia,
            FechaHoraPago = dto.FechaHoraPago,
            EstadoPago = Estados.Pago.Aprobado
        };

        _contexto.Pago.Add(pago);
        await _contexto.SaveChangesAsync();

        if (facturaQuedaPagada)
        {
            await _contexto.Factura
                .Where(f => f.IdFactura == dto.IdFactura)
                .ExecuteUpdateAsync(s => s.SetProperty(f => f.EstadoFactura, Estados.Factura.Pagada));
        }

        await transaccion.CommitAsync();
        return pago.IdPago;
    }
}
