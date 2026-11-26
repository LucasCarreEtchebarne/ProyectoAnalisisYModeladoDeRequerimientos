using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SistemaHotelColibri.Abstract.Comun;
using SistemaHotelColibri.Abstract.Interfaces.BusinessLogic.Reportes;
using SistemaHotelColibri.Abstract.Modelos.Reportes;
using SistemaHotelColibri.Reportes;

namespace SistemaHotelColibri.Controllers;

// REP-001 a REP-006: reportes del hotel y del restaurante, con exportación a Excel y PDF.
// Cada reporte se ve en pantalla y, con ?formato=excel o ?formato=pdf, se descarga con los mismos filtros.
[Authorize(Roles = Roles.AccesoReportes)]
public class ReporteController : ControladorBase
{
    public const string FormatoExcel = "excel";
    public const string FormatoPdf = "pdf";

    private readonly IObtenerReporteOcupacionBL _ocupacionBL;
    private readonly IObtenerReporteVentasBL _ventasBL;
    private readonly IObtenerReporteIngresosBL _ingresosBL;
    private readonly IObtenerReporteInventarioBL _inventarioBL;
    private readonly IObtenerReporteReservasBL _reservasBL;
    private readonly ExportadorExcel _excel;
    private readonly ExportadorPdf _pdf;

    public ReporteController(
        IObtenerReporteOcupacionBL ocupacionBL,
        IObtenerReporteVentasBL ventasBL,
        IObtenerReporteIngresosBL ingresosBL,
        IObtenerReporteInventarioBL inventarioBL,
        IObtenerReporteReservasBL reservasBL,
        ExportadorExcel excel,
        ExportadorPdf pdf)
    {
        _ocupacionBL = ocupacionBL;
        _ventasBL = ventasBL;
        _ingresosBL = ingresosBL;
        _inventarioBL = inventarioBL;
        _reservasBL = reservasBL;
        _excel = excel;
        _pdf = pdf;
    }

    public IActionResult Index()
    {
        return View();
    }

    // REP-001
    public async Task<IActionResult> Ocupacion(DateOnly? desde, DateOnly? hasta, string? tipoHabitacion, string? formato)
    {
        var filtro = Filtro(desde, hasta);
        filtro.TipoHabitacion = tipoHabitacion;

        var reporte = await _ocupacionBL.ObtenerAsync(filtro);
        ViewBag.Filtro = filtro;
        return Responder(reporte, formato, r => ReportesExportables.Ocupacion(r, TextoFiltros(("Tipo", tipoHabitacion))));
    }

    // REP-002
    [Authorize(Roles = Roles.AccesoReportesFinancieros)]
    public async Task<IActionResult> Ventas(DateOnly? desde, DateOnly? hasta, string? formato)
    {
        var filtro = Filtro(desde, hasta);
        var reporte = await _ventasBL.ObtenerAsync(filtro);
        ViewBag.Filtro = filtro;
        return Responder(reporte, formato, ReportesExportables.Ventas);
    }

    // REP-003
    [Authorize(Roles = Roles.AccesoReportesFinancieros)]
    public async Task<IActionResult> Ingresos(DateOnly? desde, DateOnly? hasta, string? formato)
    {
        var filtro = Filtro(desde, hasta);
        var reporte = await _ingresosBL.ObtenerAsync(filtro);
        ViewBag.Filtro = filtro;
        return Responder(reporte, formato, ReportesExportables.Ingresos);
    }

    // REP-004
    [Authorize(Roles = Roles.AccesoReportesFinancieros)]
    public async Task<IActionResult> Inventario(DateOnly? desde, DateOnly? hasta, string? area, string? busqueda, bool soloStockBajo, string? formato)
    {
        var filtro = Filtro(desde, hasta);
        filtro.Area = area;
        filtro.Busqueda = busqueda;
        filtro.SoloStockBajo = soloStockBajo;

        var reporte = await _inventarioBL.ObtenerAsync(filtro);
        ViewBag.Filtro = filtro;
        return Responder(reporte, formato, r => ReportesExportables.Inventario(r, TextoFiltros(
            ("Área", area), ("Búsqueda", busqueda), ("Solo stock bajo", soloStockBajo ? "Sí" : null))));
    }

    // REP-005
    public async Task<IActionResult> Reservas(DateOnly? desde, DateOnly? hasta, string? estado, string? tipoHabitacion, string? busqueda, string? formato)
    {
        var filtro = Filtro(desde, hasta);
        filtro.Estado = estado;
        filtro.TipoHabitacion = tipoHabitacion;
        filtro.Busqueda = busqueda;

        var reporte = await _reservasBL.ObtenerAsync(filtro);
        ViewBag.Filtro = filtro;
        return Responder(reporte, formato, r => ReportesExportables.Reservas(r, TextoFiltros(
            ("Estado", estado), ("Tipo", tipoHabitacion), ("Cliente o habitación", busqueda))));
    }

    // REP-006: si se pidió un formato, se descarga el archivo; si no, se muestra la vista del reporte.
    private IActionResult Responder<T>(T reporte, string? formato, Func<T, ReporteExportable> aExportable)
    {
        if (formato != FormatoExcel && formato != FormatoPdf)
        {
            return View(reporte);
        }

        var exportable = aExportable(reporte);
        var usuario = User.FindFirst(Reclamos.NombreCompleto)?.Value ?? User.Identity?.Name ?? "Usuario";

        // REP-007 (bitácora de reportes): aquí es donde se debe registrar la exportación
        // (usuario, reporte = exportable.Codigo, formato y filtros = exportable.Periodo / exportable.Filtros).

        return formato == FormatoExcel
            ? File(_excel.Generar(exportable, usuario),
                "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                exportable.NombreArchivo + ".xlsx")
            : File(_pdf.Generar(exportable, usuario), "application/pdf", exportable.NombreArchivo + ".pdf");
    }

    // Por defecto se muestra el mes actual.
    private static FiltroReporte Filtro(DateOnly? desde, DateOnly? hasta)
    {
        var hoy = DateOnly.FromDateTime(DateTime.Today);
        var inicioMes = new DateOnly(hoy.Year, hoy.Month, 1);
        return new FiltroReporte
        {
            Desde = desde ?? inicioMes,
            Hasta = hasta ?? inicioMes.AddMonths(1).AddDays(-1)
        };
    }

    private static string? TextoFiltros(params (string Nombre, string? Valor)[] filtros)
    {
        var usados = filtros.Where(f => !string.IsNullOrWhiteSpace(f.Valor)).Select(f => $"{f.Nombre}: {f.Valor}").ToList();
        return usados.Count == 0 ? null : "Filtros: " + string.Join(" · ", usados);
    }
}
