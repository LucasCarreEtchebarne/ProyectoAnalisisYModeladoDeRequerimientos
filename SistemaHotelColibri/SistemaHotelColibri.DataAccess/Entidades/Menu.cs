using System;
using System.Collections.Generic;

namespace SistemaHotelColibri.DataAccess.Entidades;

public partial class Menu
{
    public int IdProductoMenu { get; set; }

    public string NombreProducto { get; set; } = null!;

    public string CategoriaMenu { get; set; } = null!;

    public decimal Precio { get; set; }

    public string EstadoProductoMenu { get; set; } = null!;

    public string? Descripcion { get; set; }

    public virtual ICollection<DetallePedido> DetallePedido { get; set; } = new List<DetallePedido>();

    public virtual ICollection<RecetaProducto> RecetaProducto { get; set; } = new List<RecetaProducto>();
}
