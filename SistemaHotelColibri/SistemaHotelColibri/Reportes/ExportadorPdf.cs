using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;

namespace SistemaHotelColibri.Reportes;

// REP-006: genera el PDF del reporte con el logo, el resumen y las tablas.
public class ExportadorPdf
{
    private const string ColorMarca = "#0F3D2E";
    private const string ColorEncabezado = "#0F766E";
    private const string ColorSuave = "#E3F3F0";
    private const string ColorTextoSuave = "#64748B";
    private const string ColorBorde = "#E2E8E5";

    private readonly string _rutaLogo;

    public ExportadorPdf(IWebHostEnvironment entorno)
    {
        _rutaLogo = Path.Combine(entorno.WebRootPath, "img", "logo-colibri.png");
    }

    public byte[] Generar(ReporteExportable reporte, string generadoPor)
    {
        // Las tablas con muchas columnas se ven mejor con la hoja acostada.
        var horizontal = reporte.Tablas.Any(t => t.Columnas.Count > 6);

        return Document.Create(documento =>
        {
            documento.Page(pagina =>
            {
                pagina.Size(horizontal ? PageSizes.A4.Landscape() : PageSizes.A4);
                pagina.Margin(28);
                pagina.DefaultTextStyle(estilo => estilo.FontSize(9).FontColor("#1F2937"));

                pagina.Header().Element(c => Encabezado(c, reporte, generadoPor));
                pagina.Content().PaddingVertical(10).Column(columna =>
                {
                    columna.Spacing(14);
                    if (reporte.Resumen.Count > 0)
                    {
                        columna.Item().Element(c => Resumen(c, reporte));
                    }

                    foreach (var tabla in reporte.Tablas)
                    {
                        columna.Item().Element(c => Tabla(c, tabla));
                    }
                });
                pagina.Footer().AlignCenter().Text(texto =>
                {
                    texto.DefaultTextStyle(e => e.FontSize(8).FontColor(ColorTextoSuave));
                    texto.Span($"{reporte.Codigo} · {reporte.Titulo} · Página ");
                    texto.CurrentPageNumber();
                    texto.Span(" de ");
                    texto.TotalPages();
                });
            });
        }).GeneratePdf();
    }

    private void Encabezado(IContainer contenedor, ReporteExportable reporte, string generadoPor)
    {
        contenedor.BorderBottom(1.5f).BorderColor(ColorEncabezado).PaddingBottom(8).Row(fila =>
        {
            if (File.Exists(_rutaLogo))
            {
                fila.ConstantItem(90).Height(44).Image(_rutaLogo).FitArea();
                fila.ConstantItem(12);
            }

            fila.RelativeItem().Column(columna =>
            {
                columna.Item().Text("Hotel El Colibrí").FontSize(9).FontColor(ColorTextoSuave);
                columna.Item().Text(reporte.Titulo).FontSize(16).Bold().FontColor(ColorMarca);
                columna.Item().Text(reporte.Periodo).FontSize(9).FontColor(ColorTextoSuave);
                if (!string.IsNullOrWhiteSpace(reporte.Filtros))
                {
                    columna.Item().Text(reporte.Filtros).FontSize(8).FontColor(ColorTextoSuave);
                }
            });

            fila.ConstantItem(150).AlignRight().Column(columna =>
            {
                columna.Item().AlignRight().Text(reporte.Codigo).Bold().FontColor(ColorEncabezado);
                columna.Item().AlignRight().Text($"Generado: {DateTime.Now:dd/MM/yyyy HH:mm}").FontSize(8).FontColor(ColorTextoSuave);
                columna.Item().AlignRight().Text($"Por: {generadoPor}").FontSize(8).FontColor(ColorTextoSuave);
            });
        });
    }

    private static void Resumen(IContainer contenedor, ReporteExportable reporte)
    {
        const int porFila = 4;
        contenedor.Column(columna =>
        {
            columna.Spacing(6);
            foreach (var grupo in reporte.Resumen.Chunk(porFila))
            {
                columna.Item().Row(fila =>
                {
                    fila.Spacing(6);
                    foreach (var (etiqueta, valor) in grupo)
                    {
                        fila.RelativeItem().Background(ColorSuave).Padding(7).Column(dato =>
                        {
                            dato.Item().Text(etiqueta).FontSize(8).FontColor(ColorTextoSuave);
                            dato.Item().Text(valor).FontSize(12).Bold().FontColor(ColorMarca);
                        });
                    }

                    for (var i = grupo.Length; i < porFila; i++)
                    {
                        fila.RelativeItem();
                    }
                });
            }
        });
    }

    private static void Tabla(IContainer contenedor, TablaExportable tabla)
    {
        // Si no cabe el título con unas cuantas filas, la tabla empieza en la página siguiente.
        contenedor.EnsureSpace(90).Column(columna =>
        {
            columna.Item().PaddingBottom(4).Text(tabla.Titulo).FontSize(11).Bold().FontColor(ColorMarca);

            if (tabla.Filas.Count == 0)
            {
                columna.Item().Text("Sin datos para el período elegido.").FontColor(ColorTextoSuave);
                return;
            }

            columna.Item().Table(t =>
            {
                t.ColumnsDefinition(definicion =>
                {
                    foreach (var c in tabla.Columnas)
                    {
                        if (c.Tipo == TipoColumna.Texto)
                        {
                            definicion.RelativeColumn(2);
                        }
                        else
                        {
                            definicion.RelativeColumn(1);
                        }
                    }
                });

                t.Header(encabezado =>
                {
                    foreach (var c in tabla.Columnas)
                    {
                        var celda = encabezado.Cell().Background(ColorEncabezado).PaddingVertical(4).PaddingHorizontal(5);
                        var texto = FormatoExportable.AlineadoDerecha(c.Tipo) ? celda.AlignRight() : celda;
                        texto.Text(c.Titulo).FontColor(Colors.White).Bold().FontSize(8);
                    }
                });

                var numero = 0;
                foreach (var valores in tabla.Filas)
                {
                    var fondo = numero++ % 2 == 0 ? "#FFFFFF" : "#F6F9F8";
                    Fila(t, tabla.Columnas, valores, fondo, negrita: false);
                }

                if (tabla.Total != null)
                {
                    Fila(t, tabla.Columnas, tabla.Total, ColorSuave, negrita: true);
                }
            });
        });
    }

    private static void Fila(TableDescriptor tabla, List<ColumnaExportable> columnas, object?[] valores, string fondo, bool negrita)
    {
        for (var c = 0; c < columnas.Count; c++)
        {
            var valor = c < valores.Length ? valores[c] : null;
            var celda = tabla.Cell().Background(fondo).BorderBottom(0.5f).BorderColor(ColorBorde).PaddingVertical(3).PaddingHorizontal(5);
            var alineada = FormatoExportable.AlineadoDerecha(columnas[c].Tipo) ? celda.AlignRight() : celda;
            var texto = alineada.Text(FormatoExportable.Texto(valor, columnas[c].Tipo));
            if (negrita)
            {
                texto.Bold();
            }
        }
    }
}
