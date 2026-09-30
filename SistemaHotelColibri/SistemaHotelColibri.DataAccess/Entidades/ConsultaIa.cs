using System;
using System.Collections.Generic;

namespace SistemaHotelColibri.DataAccess.Entidades;

public partial class ConsultaIa
{
    public int IdConsultaIa { get; set; }

    public int IdUsuario { get; set; }

    public string ConsultaIngresada { get; set; } = null!;

    public string? RespuestaGenerada { get; set; }

    public DateTime FechaRealizada { get; set; }
}
