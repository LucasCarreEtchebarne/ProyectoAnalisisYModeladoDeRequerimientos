using System.ComponentModel.DataAnnotations;
using SistemaHotelColibri.Abstract.Comun;
using SistemaHotelColibri.Abstract.Modelos.Facturas;

namespace SistemaHotelColibri.ViewModels.Facturas;

public class PagoFormViewModel
{
    [Display(Name = "método de pago")]
    [Required(ErrorMessage = Mensajes.CampoObligatorio)]
    public string? MetodoPago { get; set; } = Facturacion.MetodoPago.Efectivo;

    [Display(Name = "monto a pagar")]
    [Required(ErrorMessage = Mensajes.CampoObligatorio)]
    [Range(0.01, 9999999999.99, ErrorMessage = Mensajes.MontoPagoInvalido)]
    public decimal? MontoPagado { get; set; }

    [Display(Name = "monto recibido")]
    [Range(0, 9999999999.99, ErrorMessage = Mensajes.ValorNoNegativo)]
    public decimal? MontoRecibido { get; set; }

    [Display(Name = "tipo de tarjeta")]
    public string? TipoTarjeta { get; set; }

    [Display(Name = "últimos 4 dígitos")]
    [RegularExpression(@"^\d{4}$", ErrorMessage = Mensajes.UltimosDigitosInvalidos)]
    public string? UltimosCuatroDigitos { get; set; }

    [Display(Name = "número de referencia")]
    [StringLength(50, ErrorMessage = Mensajes.LongitudMaxima)]
    public string? NumeroReferencia { get; set; }

    public PagoDto ADto(int idFactura) => new()
    {
        IdFactura = idFactura,
        MetodoPago = MetodoPago ?? string.Empty,
        MontoPagado = MontoPagado ?? 0,
        MontoRecibido = MontoRecibido,
        TipoTarjeta = TipoTarjeta,
        UltimosCuatroDigitos = UltimosCuatroDigitos,
        NumeroReferencia = NumeroReferencia
    };
}

public class AnularFacturaViewModel
{
    [Display(Name = "motivo")]
    [Required(ErrorMessage = Mensajes.MotivoAnulacionObligatorio)]
    [StringLength(250, ErrorMessage = Mensajes.LongitudMaxima)]
    public string? Motivo { get; set; }
}

public class ConsolidadaViewModel
{
    public ReservaFacturableDto Reserva { get; set; } = new();
    public List<PedidoFacturableDto> Pedidos { get; set; } = [];
}
