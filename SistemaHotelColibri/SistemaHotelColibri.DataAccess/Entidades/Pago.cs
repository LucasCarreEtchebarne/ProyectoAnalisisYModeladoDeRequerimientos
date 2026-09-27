using System;
using System.Collections.Generic;

namespace SistemaHotelColibri.DataAccess.Entidades;

public partial class Pago
{
    public int IdPago { get; set; }

    public int IdFactura { get; set; }

    public int IdUsuario { get; set; }

    public string MetodoPago { get; set; } = null!;

    public decimal MontoPagado { get; set; }

    public decimal? MontoRecibido { get; set; }

    public decimal? CambioDevuelto { get; set; }

    public string? TipoTarjeta { get; set; }

    public string? UltimosCuatroDigitos { get; set; }

    public string? NumeroReferencia { get; set; }

    public DateTime FechaHoraPago { get; set; }

    public string EstadoPago { get; set; } = null!;

    public virtual Factura IdFacturaNavigation { get; set; } = null!;
}
