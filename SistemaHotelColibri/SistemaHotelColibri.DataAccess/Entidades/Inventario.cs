using System;
using System.Collections.Generic;

namespace SistemaHotelColibri.DataAccess.Entidades;

public partial class Inventario
{
    public int IdProducto { get; set; }

    public string NombreProducto { get; set; } = null!;

    public string CategoriaProducto { get; set; } = null!;

    public string UnidadMedida { get; set; } = null!;

    public decimal Stock { get; set; }

    public decimal StockMinimo { get; set; }

    public DateOnly FechaIngreso { get; set; }

    public string EstadoProducto { get; set; } = null!;

    public string? Descripcion { get; set; }

    public virtual ICollection<MovimientoInventario> MovimientoInventario { get; set; } = new List<MovimientoInventario>();

    public virtual ICollection<RecetaProducto> RecetaProducto { get; set; } = new List<RecetaProducto>();
}
