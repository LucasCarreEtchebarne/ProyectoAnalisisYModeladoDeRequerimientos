using System;
using System.Collections.Generic;

namespace Prototipo.ViewModels
{
    public class ReporteViewModel
    {
        public string Tipo { get; set; }
        public string Titulo { get; set; }
        public DateTime Desde { get; set; }
        public DateTime Hasta { get; set; }
        public List<string> Encabezados { get; set; }
        public List<List<string>> Filas { get; set; }
        public string Resumen { get; set; }

        public ReporteViewModel()
        {
            Encabezados = new List<string>();
            Filas = new List<List<string>>();
        }

        public static readonly Dictionary<string, string> Tipos = new Dictionary<string, string>
        {
            { "ingresos", "Ingresos por periodo" },
            { "ocupacion", "Ocupacion de habitaciones" },
            { "consumo", "Productos mas vendidos" },
            { "inventario", "Estado del inventario" },
            { "clientes", "Clientes con mas facturacion" }
        };
    }
}
