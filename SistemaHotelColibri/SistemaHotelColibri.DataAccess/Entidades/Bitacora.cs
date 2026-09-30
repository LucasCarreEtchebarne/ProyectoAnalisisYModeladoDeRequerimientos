using System;
using System.Collections.Generic;

namespace SistemaHotelColibri.DataAccess.Entidades;

public partial class Bitacora
{
    public long IdBitacora { get; set; }

    public int IdUsuario { get; set; }

    public string Modulo { get; set; } = null!;

    public int? IdRegistro { get; set; }

    public string AccionRealizada { get; set; } = null!;

    public DateTime FechaHora { get; set; }

    public string? Descripcion { get; set; }
}
