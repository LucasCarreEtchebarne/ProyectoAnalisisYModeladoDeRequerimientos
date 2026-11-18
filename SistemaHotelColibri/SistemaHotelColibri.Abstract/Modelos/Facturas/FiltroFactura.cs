using SistemaHotelColibri.Abstract.Comun;

namespace SistemaHotelColibri.Abstract.Modelos.Facturas;

public class FiltroFactura : FiltroListado
{
    public string? Tipo { get; set; }
    public DateOnly? Desde { get; set; }
    public DateOnly? Hasta { get; set; }
}
