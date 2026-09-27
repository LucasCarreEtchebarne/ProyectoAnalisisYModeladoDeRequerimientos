using System;
using System.Collections.Generic;

namespace SistemaHotelColibri.DataAccess.Entidades;

public partial class DetalleFactura
{
    public int IdDetalleFactura { get; set; }

    public int IdFactura { get; set; }

    public string TipoConcepto { get; set; } = null!;

    public int? IdReserva { get; set; }

    public int? IdPedido { get; set; }

    public int? IdEvento { get; set; }

    public string Descripcion { get; set; } = null!;

    public decimal Cantidad { get; set; }

    public decimal PrecioUnitario { get; set; }

    public decimal? Subtotal { get; set; }

    public virtual Evento? IdEventoNavigation { get; set; }

    public virtual Factura IdFacturaNavigation { get; set; } = null!;

    public virtual Pedido? IdPedidoNavigation { get; set; }

    public virtual Reserva? IdReservaNavigation { get; set; }
}
