using ClosedXML.Excel;

namespace SistemaHotelColibri.Reportes;

// REP-006: genera el archivo .xlsx. Hoja "Resumen" con los datos clave y una hoja por cada tabla.
public class ExportadorExcel
{
    private static readonly XLColor ColorMarca = XLColor.FromHtml("#0F3D2E");
    private static readonly XLColor ColorEncabezado = XLColor.FromHtml("#0F766E");
    private static readonly XLColor ColorSuave = XLColor.FromHtml("#E3F3F0");

    public byte[] Generar(ReporteExportable reporte, string generadoPor)
    {
        using var libro = new XLWorkbook();
        var nombresUsados = new HashSet<string>(StringComparer.OrdinalIgnoreCase);

        var resumen = libro.Worksheets.Add("Resumen");
        nombresUsados.Add("Resumen");

        resumen.Cell(1, 1).Value = "Hotel El Colibrí";
        resumen.Cell(1, 1).Style.Font.SetBold().Font.SetFontSize(16).Font.SetFontColor(ColorMarca);
        resumen.Cell(2, 1).Value = $"{reporte.Codigo} · {reporte.Titulo}";
        resumen.Cell(2, 1).Style.Font.SetBold().Font.SetFontSize(13);
        resumen.Cell(3, 1).Value = reporte.Periodo;
        var fila = 4;
        if (!string.IsNullOrWhiteSpace(reporte.Filtros))
        {
            resumen.Cell(fila++, 1).Value = reporte.Filtros;
        }

        resumen.Cell(fila++, 1).Value = $"Generado el {DateTime.Now:dd/MM/yyyy HH:mm} por {generadoPor}";
        resumen.Range(3, 1, fila - 1, 1).Style.Font.SetFontColor(XLColor.FromHtml("#64748B"));

        fila++;
        foreach (var (etiqueta, valor) in reporte.Resumen)
        {
            resumen.Cell(fila, 1).Value = etiqueta;
            resumen.Cell(fila, 1).Style.Font.SetBold().Fill.SetBackgroundColor(ColorSuave);
            resumen.Cell(fila, 2).Value = valor;
            resumen.Cell(fila, 2).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right);
            fila++;
        }

        resumen.Columns(1, 2).AdjustToContents();
        resumen.Column(1).Width = Math.Max(resumen.Column(1).Width, 32);

        foreach (var tabla in reporte.Tablas)
        {
            var hoja = libro.Worksheets.Add(NombreHoja(tabla.Titulo, nombresUsados));
            EscribirTabla(hoja, tabla);
        }

        using var memoria = new MemoryStream();
        libro.SaveAs(memoria);
        return memoria.ToArray();
    }

    private static void EscribirTabla(IXLWorksheet hoja, TablaExportable tabla)
    {
        hoja.Cell(1, 1).Value = tabla.Titulo;
        hoja.Cell(1, 1).Style.Font.SetBold().Font.SetFontSize(13).Font.SetFontColor(ColorMarca);

        const int filaEncabezado = 3;
        for (var c = 0; c < tabla.Columnas.Count; c++)
        {
            var celda = hoja.Cell(filaEncabezado, c + 1);
            celda.Value = tabla.Columnas[c].Titulo;
            celda.Style.Font.SetBold().Font.SetFontColor(XLColor.White).Fill.SetBackgroundColor(ColorEncabezado);
        }

        var filaActual = filaEncabezado + 1;
        foreach (var valores in tabla.Filas)
        {
            EscribirFila(hoja, filaActual++, valores, tabla.Columnas);
        }

        if (tabla.Filas.Count == 0)
        {
            hoja.Cell(filaActual++, 1).Value = "Sin datos para el período elegido.";
        }
        else
        {
            hoja.Range(filaEncabezado, 1, filaActual - 1, tabla.Columnas.Count).SetAutoFilter();
        }

        if (tabla.Total != null)
        {
            EscribirFila(hoja, filaActual, tabla.Total, tabla.Columnas);
            hoja.Range(filaActual, 1, filaActual, tabla.Columnas.Count).Style
                .Font.SetBold()
                .Fill.SetBackgroundColor(ColorSuave)
                .Border.SetTopBorder(XLBorderStyleValues.Thin);
        }

        hoja.SheetView.FreezeRows(filaEncabezado);
        hoja.Columns(1, tabla.Columnas.Count).AdjustToContents();
        foreach (var columna in hoja.Columns(1, tabla.Columnas.Count))
        {
            columna.Width = Math.Min(Math.Max(columna.Width, 12), 60);
        }
    }

    private static void EscribirFila(IXLWorksheet hoja, int fila, object?[] valores, List<ColumnaExportable> columnas)
    {
        for (var c = 0; c < columnas.Count && c < valores.Length; c++)
        {
            var celda = hoja.Cell(fila, c + 1);
            var valor = valores[c];
            var tipo = columnas[c].Tipo;

            switch (valor)
            {
                case null:
                    break;
                case string texto:
                    celda.Value = texto;
                    break;
                case DateOnly fecha:
                    celda.Value = fecha.ToDateTime(TimeOnly.MinValue);
                    celda.Style.DateFormat.Format = "dd/mm/yyyy";
                    break;
                case DateTime fechaHora:
                    celda.Value = fechaHora;
                    celda.Style.DateFormat.Format = "dd/mm/yyyy hh:mm";
                    break;
                default:
                    celda.Value = Convert.ToDecimal(valor);
                    celda.Style.NumberFormat.Format = tipo switch
                    {
                        TipoColumna.Dinero => "\"₡\" #,##0.00",
                        TipoColumna.Porcentaje => "0.0\" %\"",
                        TipoColumna.Entero => "#,##0",
                        _ => "#,##0.###"
                    };
                    break;
            }
        }
    }

    // Excel permite nombres de hoja de hasta 31 caracteres y sin : \ / ? * [ ].
    private static string NombreHoja(string titulo, HashSet<string> usados)
    {
        var limpio = new string(titulo.Where(ch => !":\\/?*[]".Contains(ch)).ToArray()).Trim();
        if (limpio.Length == 0)
        {
            limpio = "Datos";
        }

        limpio = limpio.Length > 28 ? limpio[..28] : limpio;
        var nombre = limpio;
        for (var i = 2; usados.Contains(nombre); i++)
        {
            nombre = $"{limpio} {i}";
        }

        usados.Add(nombre);
        return nombre;
    }
}
