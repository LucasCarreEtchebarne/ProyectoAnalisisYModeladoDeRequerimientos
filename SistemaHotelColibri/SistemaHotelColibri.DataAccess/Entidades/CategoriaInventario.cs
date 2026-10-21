using System;
using System.Collections.Generic;

namespace SistemaHotelColibri.DataAccess.Entidades;

public partial class CategoriaInventario
{
    public int IdCategoria { get; set; }

    public string NombreCategoria { get; set; } = null!;

    public string Area { get; set; } = null!;

    public string EstadoCategoria { get; set; } = null!;

    public string? Descripcion { get; set; }

    public virtual ICollection<Inventario> Inventario { get; set; } = new List<Inventario>();
}
