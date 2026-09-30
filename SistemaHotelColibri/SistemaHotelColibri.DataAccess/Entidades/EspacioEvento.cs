using System;
using System.Collections.Generic;

namespace SistemaHotelColibri.DataAccess.Entidades;

public partial class EspacioEvento
{
    public int IdEspacio { get; set; }

    public string NombreEspacio { get; set; } = null!;

    public int CapacidadMaxima { get; set; }

    public string EstadoEspacio { get; set; } = null!;

    public string? Descripcion { get; set; }

    public virtual ICollection<Evento> Evento { get; set; } = new List<Evento>();
}
