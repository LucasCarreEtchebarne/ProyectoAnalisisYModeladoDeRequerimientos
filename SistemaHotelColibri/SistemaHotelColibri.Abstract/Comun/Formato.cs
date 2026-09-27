using System.Globalization;

namespace SistemaHotelColibri.Abstract.Comun;

public static class Formato
{
    private static readonly CultureInfo Cultura = CulturaSistema.Crear();

    public static string Colones(decimal monto) => "₡ " + monto.ToString("#,##0.00", Cultura);

    public static string Cantidad(decimal cantidad) => cantidad.ToString("#,##0.###", Cultura);

    public static string Fecha(DateOnly fecha) => fecha.ToString("dd/MM/yyyy", Cultura);

    public static string Fecha(DateTime fecha) => fecha.ToString("dd/MM/yyyy", Cultura);
}

public static class CulturaSistema
{
    public static CultureInfo Crear()
    {
        var cultura = (CultureInfo)CultureInfo.GetCultureInfo("es-CR").Clone();
        cultura.NumberFormat.NumberDecimalSeparator = ".";
        cultura.NumberFormat.NumberGroupSeparator = ",";
        cultura.NumberFormat.CurrencyDecimalSeparator = ".";
        cultura.NumberFormat.CurrencyGroupSeparator = ",";
        cultura.NumberFormat.CurrencySymbol = "₡";
        cultura.DateTimeFormat.ShortDatePattern = "dd/MM/yyyy";
        return cultura;
    }
}
