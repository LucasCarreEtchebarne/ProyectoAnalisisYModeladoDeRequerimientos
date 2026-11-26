using SistemaHotelColibri.Abstract.Modelos.Reservas;

namespace SistemaHotelColibri.Abstract.Modelos.Reportes;

// REP-005: Reporte de reservas con filtros (fechas de entrada, estado, tipo de habitación y cliente).
public class ReporteReservasDto
{
    public DateOnly Desde { get; set; }
    public DateOnly Hasta { get; set; }
    public int TotalReservas { get; set; }
    public int TotalNoches { get; set; }
    public int TotalHuespedes { get; set; }
    public decimal MontoTotal { get; set; }

    public decimal EstanciaPromedio => TotalReservas == 0 ? 0 : Math.Round((decimal)TotalNoches / TotalReservas, 1);

    public List<ReservaDto> Reservas { get; set; } = [];
    public List<ReservaEstadoReporteDto> PorEstado { get; set; } = [];
}

public class ReservaEstadoReporteDto
{
    public string Estado { get; set; } = string.Empty;
    public int Reservas { get; set; }
    public int Noches { get; set; }
    public decimal Monto { get; set; }
}
