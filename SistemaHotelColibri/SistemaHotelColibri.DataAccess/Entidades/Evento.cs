using System;
using System.Collections.Generic;

namespace SistemaHotelColibri.DataAccess.Entidades;

public partial class Evento
{
    public int IdEvento { get; set; }

    public int IdCliente { get; set; }

    public int IdEspacio { get; set; }

    public string NombreEvento { get; set; } = null!;

    public DateOnly FechaEvento { get; set; }

    public TimeOnly HoraInicio { get; set; }

    public TimeOnly HoraFin { get; set; }

    public int Participantes { get; set; }

    public decimal MontoAcordado { get; set; }

    public string EstadoEvento { get; set; } = null!;

    public string? Descripcion { get; set; }

    public virtual ICollection<DetalleFactura> DetalleFactura { get; set; } = new List<DetalleFactura>();

    public virtual Cliente IdClienteNavigation { get; set; } = null!;

    public virtual EspacioEvento IdEspacioNavigation { get; set; } = null!;
}
