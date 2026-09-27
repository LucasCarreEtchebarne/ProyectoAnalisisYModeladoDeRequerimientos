using System;
using System.Collections.Generic;

namespace SistemaHotelColibri.DataAccess.Entidades;

public partial class MovimientoInventario
{
    public long IdMovimiento { get; set; }

    public int IdProducto { get; set; }

    public int IdUsuario { get; set; }

    public int? IdPedido { get; set; }

    public string TipoMovimiento { get; set; } = null!;

    public decimal Cantidad { get; set; }

    public decimal StockResultante { get; set; }

    public DateTime FechaHora { get; set; }

    public string? Motivo { get; set; }

    public virtual Pedido? IdPedidoNavigation { get; set; }

    public virtual Inventario IdProductoNavigation { get; set; } = null!;
}
