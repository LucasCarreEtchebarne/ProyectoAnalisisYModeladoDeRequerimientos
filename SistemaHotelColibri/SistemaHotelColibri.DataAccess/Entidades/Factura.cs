using System;
using System.Collections.Generic;

namespace SistemaHotelColibri.DataAccess.Entidades;

public partial class Factura
{
    public int IdFactura { get; set; }

    public string? NumeroFactura { get; set; }

    public int? IdCliente { get; set; }

    public int IdUsuario { get; set; }

    public DateTime FechaEmision { get; set; }

    public string TipoFactura { get; set; } = null!;

    public decimal MontoTotal { get; set; }

    public string EstadoFactura { get; set; } = null!;

    public string? MotivoAnulacion { get; set; }

    public DateTime? FechaAnulacion { get; set; }

    public virtual ICollection<DetalleFactura> DetalleFactura { get; set; } = new List<DetalleFactura>();

    public virtual Cliente? IdClienteNavigation { get; set; }

    public virtual ICollection<Pago> Pago { get; set; } = new List<Pago>();
}
