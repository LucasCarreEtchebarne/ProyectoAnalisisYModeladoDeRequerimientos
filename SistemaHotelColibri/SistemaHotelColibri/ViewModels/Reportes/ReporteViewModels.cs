namespace SistemaHotelColibri.ViewModels.Reportes;

// Un dato destacado arriba del reporte (ej.: "Ocupación 62 %").
public record KpiViewModel(string Titulo, string Valor, string? Detalle, string Icono, string Color);

// Una barra horizontal para comparar valores (ej.: ventas por categoría).
public record BarraViewModel(string Etiqueta, decimal Valor, string Texto);

// Datos para el bloque de barras con su título.
public record GraficoBarrasViewModel(string Titulo, string Icono, IReadOnlyList<BarraViewModel> Barras, string? Vacio = null);
