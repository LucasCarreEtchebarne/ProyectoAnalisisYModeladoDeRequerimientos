using System;
using System.Collections.Generic;

namespace SistemaHotelColibri.DataAccess.Entidades;

public partial class Proveedor
{
    public int IdProveedor { get; set; }

    public string NombreProveedor { get; set; } = null!;

    public string? NombreContacto { get; set; }

    public string Telefono { get; set; } = null!;

    public string? CorreoElectronico { get; set; }

    public string EstadoProveedor { get; set; } = null!;

    public string? Descripcion { get; set; }

    public virtual ICollection<Inventario> Inventario { get; set; } = new List<Inventario>();
}
