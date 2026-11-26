using SistemaHotelColibri.Abstract.Modelos.Facturas;

namespace SistemaHotelColibri.Abstract.Modelos.Reportes;

// REP-003: Reporte de ingresos totales (hospedaje, restaurante y eventos).
public class ReporteIngresosDto
{
    public DateOnly Desde { get; set; }
    public DateOnly Hasta { get; set; }
    public int FacturasEmitidas { get; set; }
    public int FacturasAnuladas { get; set; }
    public decimal Subtotal { get; set; }
    public decimal Servicio { get; set; }
    public decimal Iva { get; set; }
    public decimal TotalFacturado { get; set; }
    public decimal TotalCobrado { get; set; }
    public decimal PendientePorCobrar { get; set; }

    public List<IngresoConceptoReporteDto> PorConcepto { get; set; } = [];
    public List<IngresoMetodoReporteDto> PorMetodoPago { get; set; } = [];
    public List<IngresoDiaReporteDto> PorDia { get; set; } = [];
    public List<FacturaDto> Facturas { get; set; } = [];
}

// Datos crudos que entrega la capa de datos.
public class DatosIngresosDto
{
    public List<FacturaDto> Facturas { get; set; } = [];
    public List<PagoDto> Pagos { get; set; } = [];
}

public class IngresoConceptoReporteDto
{
    public string Concepto { get; set; } = string.Empty;
    public int Lineas { get; set; }
    public decimal Monto { get; set; }
    public decimal Porcentaje { get; set; }
}

public class IngresoMetodoReporteDto
{
    public string MetodoPago { get; set; } = string.Empty;
    public int Pagos { get; set; }
    public decimal Monto { get; set; }
    public decimal Porcentaje { get; set; }
}

public class IngresoDiaReporteDto
{
    public DateOnly Fecha { get; set; }
    public decimal Facturado { get; set; }
    public decimal Cobrado { get; set; }
}
