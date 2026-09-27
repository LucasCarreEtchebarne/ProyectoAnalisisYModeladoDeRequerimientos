using System;
using System.Collections.Generic;

namespace SistemaHotelColibri.DataAccess.Entidades;

public partial class Cliente
{
    public int IdCliente { get; set; }

    public string Identificacion { get; set; } = null!;

    public string NombreCompleto { get; set; } = null!;

    public string PrimerApellido { get; set; } = null!;

    public string? SegundoApellido { get; set; }

    public string? Telefono { get; set; }

    public string? CorreoElectronico { get; set; }

    public string? Direccion { get; set; }

    public DateTime FechaRegistro { get; set; }

    public string EstadoCliente { get; set; } = null!;

    public virtual ICollection<Evento> Evento { get; set; } = new List<Evento>();

    public virtual ICollection<Factura> Factura { get; set; } = new List<Factura>();

    public virtual ICollection<Pedido> Pedido { get; set; } = new List<Pedido>();

    public virtual ICollection<Reserva> Reserva { get; set; } = new List<Reserva>();

    public virtual ICollection<ReservaMesa> ReservaMesa { get; set; } = new List<ReservaMesa>();
}
