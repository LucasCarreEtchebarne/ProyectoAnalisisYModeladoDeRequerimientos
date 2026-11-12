using SistemaHotelColibri.Abstract.Comun;
using SistemaHotelColibri.Abstract.Interfaces.BusinessLogic.Facturas;
using SistemaHotelColibri.Abstract.Interfaces.DataAccess.Facturas;

namespace SistemaHotelColibri.BusinessLogic.Facturas;

public class AnularFacturaBL : IAnularFacturaBL
{
    private const int LongitudMaximaMotivo = 250;

    private readonly IObtenerFacturaPorIdDA _obtenerPorIdDA;
    private readonly IAnularFacturaDA _anularDA;

    public AnularFacturaBL(IObtenerFacturaPorIdDA obtenerPorIdDA, IAnularFacturaDA anularDA)
    {
        _obtenerPorIdDA = obtenerPorIdDA;
        _anularDA = anularDA;
    }

    public async Task<ResultadoOperacion> AnularAsync(int idFactura, string? motivo)
    {
        var factura = await _obtenerPorIdDA.ObtenerPorIdAsync(idFactura);
        if (factura == null)
        {
            return ResultadoOperacion.Error(Mensajes.NoEncontrado);
        }

        if (factura.EstadoFactura == Estados.Factura.Anulada)
        {
            return ResultadoOperacion.Error(Mensajes.FacturaYaAnulada);
        }

        var motivoLimpio = motivo?.Trim();
        if (string.IsNullOrEmpty(motivoLimpio))
        {
            return ResultadoOperacion.Error(Mensajes.MotivoAnulacionObligatorio);
        }

        if (motivoLimpio.Length > LongitudMaximaMotivo)
        {
            return ResultadoOperacion.Error(string.Format(Mensajes.LongitudMaxima, "motivo", LongitudMaximaMotivo));
        }

        return await _anularDA.AnularAsync(idFactura, motivoLimpio, DateTime.Now)
            ? ResultadoOperacion.Ok(string.Format(Mensajes.FacturaAnulada, factura.NumeroFactura), idFactura)
            : ResultadoOperacion.Error(Mensajes.FacturaYaAnulada);
    }
}
