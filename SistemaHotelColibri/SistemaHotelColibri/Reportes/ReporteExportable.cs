using SistemaHotelColibri.Abstract.Comun;

namespace SistemaHotelColibri.Reportes;

// REP-006: forma común de cualquier reporte para poder exportarlo a Excel o PDF con el mismo código.
public class ReporteExportable
{
    public string Codigo { get; set; } = string.Empty;
    public string Titulo { get; set; } = string.Empty;
    public string Periodo { get; set; } = string.Empty;
    public string? Filtros { get; set; }
    public string NombreArchivo { get; set; } = "reporte";
    public List<(string Etiqueta, string Valor)> Resumen { get; set; } = [];
    public List<TablaExportable> Tablas { get; set; } = [];
}

public class TablaExportable
{
    public string Titulo { get; set; } = string.Empty;
    public List<ColumnaExportable> Columnas { get; set; } = [];
    public List<object?[]> Filas { get; set; } = [];
    public object?[]? Total { get; set; }

    public TablaExportable Columna(string titulo, TipoColumna tipo = TipoColumna.Texto)
    {
        Columnas.Add(new ColumnaExportable(titulo, tipo));
        return this;
    }

    public TablaExportable Fila(params object?[] valores)
    {
        Filas.Add(valores);
        return this;
    }

    public TablaExportable ConTotal(params object?[] valores)
    {
        Total = valores;
        return this;
    }
}

public record ColumnaExportable(string Titulo, TipoColumna Tipo);

public enum TipoColumna
{
    Texto,
    Entero,
    Decimal,
    Dinero,
    Porcentaje,
    Fecha
}

// Formato de cada valor cuando se muestra como texto (PDF y pantalla).
public static class FormatoExportable
{
    public static string Texto(object? valor, TipoColumna tipo) => valor switch
    {
        null => string.Empty,
        string texto => texto,
        DateOnly fecha => Formato.Fecha(fecha),
        DateTime fecha => Formato.FechaHora(fecha),
        _ when tipo == TipoColumna.Dinero => Formato.Colones(Convert.ToDecimal(valor)),
        _ when tipo == TipoColumna.Porcentaje => Convert.ToDecimal(valor).ToString("0.0", CulturaSistema.Crear()) + " %",
        _ when tipo == TipoColumna.Decimal => Formato.Cantidad(Convert.ToDecimal(valor)),
        _ when tipo == TipoColumna.Entero => Convert.ToDecimal(valor).ToString("#,##0", CulturaSistema.Crear()),
        _ => valor.ToString() ?? string.Empty
    };

    public static bool AlineadoDerecha(TipoColumna tipo) => tipo is not (TipoColumna.Texto or TipoColumna.Fecha);
}
