using SistemaHotelColibri.Abstract.Modelos.Habitaciones;
using SistemaHotelColibri.Abstract.Modelos.Reservas;

namespace SistemaHotelColibri.Abstract.Modelos.Reportes;

// Datos crudos que entrega la capa de datos.
public class DatosOcupacionDto
{
    public List<HabitacionDto> Habitaciones { get; set; } = [];
    public List<ReservaDto> Reservas { get; set; } = [];
}

// REP-001: Reporte de ocupación de habitaciones.
public class ReporteOcupacionDto
{
    public DateOnly Desde { get; set; }
    public DateOnly Hasta { get; set; }
    public int Dias { get; set; }
    public int TotalHabitaciones { get; set; }
    public int NochesDisponibles { get; set; }
    public int NochesOcupadas { get; set; }
    public decimal PorcentajeOcupacion { get; set; }
    public int CantidadReservas { get; set; }
    public decimal IngresoHospedaje { get; set; }

    // Tarifa promedio cobrada por noche ocupada.
    public decimal TarifaPromedio => NochesOcupadas == 0 ? 0 : Math.Round(IngresoHospedaje / NochesOcupadas, 2);

    public List<OcupacionHabitacionReporteDto> Habitaciones { get; set; } = [];
    public List<OcupacionTipoReporteDto> PorTipo { get; set; } = [];
    public List<OcupacionDiaReporteDto> PorDia { get; set; } = [];
}

public class OcupacionHabitacionReporteDto
{
    public string NumeroHabitacion { get; set; } = string.Empty;
    public string TipoHabitacion { get; set; } = string.Empty;
    public string EstadoActual { get; set; } = string.Empty;
    public int Reservas { get; set; }
    public int NochesOcupadas { get; set; }
    public decimal PorcentajeOcupacion { get; set; }
    public decimal Ingreso { get; set; }
}

public class OcupacionTipoReporteDto
{
    public string TipoHabitacion { get; set; } = string.Empty;
    public int Habitaciones { get; set; }
    public int NochesOcupadas { get; set; }
    public int NochesDisponibles { get; set; }
    public decimal PorcentajeOcupacion { get; set; }
    public decimal Ingreso { get; set; }
}

public class OcupacionDiaReporteDto
{
    public DateOnly Fecha { get; set; }
    public int Ocupadas { get; set; }
    public int Total { get; set; }
    public decimal PorcentajeOcupacion { get; set; }
}
