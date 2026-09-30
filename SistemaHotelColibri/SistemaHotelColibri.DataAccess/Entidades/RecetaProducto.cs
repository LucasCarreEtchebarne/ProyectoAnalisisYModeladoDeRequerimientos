using System;
using System.Collections.Generic;

namespace SistemaHotelColibri.DataAccess.Entidades;

public partial class RecetaProducto
{
    public int IdRecetaProducto { get; set; }

    public int IdProductoMenu { get; set; }

    public int IdProducto { get; set; }

    public decimal CantidadUtilizada { get; set; }

    public virtual Menu IdProductoMenuNavigation { get; set; } = null!;

    public virtual Inventario IdProductoNavigation { get; set; } = null!;
}
