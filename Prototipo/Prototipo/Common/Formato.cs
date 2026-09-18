using System;
using System.Globalization;

namespace Prototipo.Common
{
    public static class Formato
    {
        private static readonly CultureInfo CostaRica = CrearCulturaCostaRica();

        private static CultureInfo CrearCulturaCostaRica()
        {
            var cultura = (CultureInfo)CultureInfo.GetCultureInfo("es-CR").Clone();
            cultura.NumberFormat.NumberGroupSeparator = ".";
            cultura.NumberFormat.NumberDecimalSeparator = ",";
            return cultura;
        }

        public static string Colones(decimal monto)
        {
            return "₡ " + monto.ToString("#,##0.00", CostaRica);
        }

        public static string Colones(decimal? monto)
        {
            return monto.HasValue ? Colones(monto.Value) : "-";
        }

        public static string Fecha(DateTime fecha)
        {
            return fecha.ToString("dd/MM/yyyy", CostaRica);
        }

        public static string Fecha(DateTime? fecha)
        {
            return fecha.HasValue ? Fecha(fecha.Value) : "-";
        }

        public static string FechaHora(DateTime fecha)
        {
            return fecha.ToString("dd/MM/yyyy hh:mm tt", CostaRica);
        }

        public static string FechaHora(DateTime? fecha)
        {
            return fecha.HasValue ? FechaHora(fecha.Value) : "-";
        }

        public static string Hora(TimeSpan hora)
        {
            return DateTime.Today.Add(hora).ToString("hh:mm tt", CostaRica);
        }

        public static string BadgeEstado(string estado)
        {
            if (string.IsNullOrEmpty(estado))
            {
                return "badge badge-pendiente";
            }

            switch (estado)
            {
                case "CheckIn":
                    return "badge badge-encurso";
                case "CheckOut":
                case "Realizado":
                    return "badge badge-completada";
                case "Confirmado":
                    return "badge badge-confirmada";
                case "Entregado":
                    return "badge badge-servido";
                case "EnProceso":
                    return "badge badge-enprogreso";
                case "Reversado":
                    return "badge badge-reembolsado";
                case "Atendida":
                    return "badge badge-completada";
                default:
                    return "badge badge-" + estado.ToLowerInvariant();
            }
        }
    }
}
