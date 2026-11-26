using SistemaHotelColibri.Abstract.Interfaces.BusinessLogic.Reportes;
using SistemaHotelColibri.Abstract.Interfaces.DataAccess.Reportes;
using SistemaHotelColibri.Abstract.Modelos.Reportes;

namespace SistemaHotelColibri.BusinessLogic.Reportes;

// REP-004: existencias de cada producto frente a su mínimo y lo que entró o salió en el rango.
public class ObtenerReporteInventarioBL : IObtenerReporteInventarioBL
{
    private const string Entrada = "Entrada";
    private const string Salida = "Salida";
    private const string Ajuste = "Ajuste";

    private readonly IObtenerDatosInventarioDA _datosDA;

    public ObtenerReporteInventarioBL(IObtenerDatosInventarioDA datosDA)
    {
        _datosDA = datosDA;
    }

    public async Task<ReporteInventarioDto> ObtenerAsync(FiltroReporte filtro)
    {
        ReglasReporte.Normalizar(filtro);
        var datos = await _datosDA.ObtenerAsync(filtro);

        decimal Movido(int idProducto, string tipo) => datos.Movimientos
            .Where(m => m.IdProducto == idProducto && m.TipoMovimiento == tipo)
            .Sum(m => m.Cantidad);

        static int Gravedad(string estado) => estado switch
        {
            ReporteInventarioDto.EstadoAgotado => 0,
            ReporteInventarioDto.EstadoBajo => 1,
            _ => 2
        };

        var filas = datos.Productos
            .Select(p => new InventarioReporteFilaDto
            {
                Producto = p.NombreProducto,
                Categoria = p.NombreCategoria,
                Area = p.AreaCategoria,
                Proveedor = p.NombreProveedor,
                UnidadMedida = p.UnidadMedida,
                Stock = p.Stock,
                StockMinimo = p.StockMinimo,
                Faltante = Math.Max(0, p.StockMinimo - p.Stock),
                Entradas = Movido(p.IdProducto, Entrada),
                Salidas = Movido(p.IdProducto, Salida),
                Ajustes = Movido(p.IdProducto, Ajuste),
                Estado = p.Stock <= 0 ? ReporteInventarioDto.EstadoAgotado
                    : p.Stock <= p.StockMinimo ? ReporteInventarioDto.EstadoBajo
                    : ReporteInventarioDto.EstadoNormal
            })
            // Primero lo que urge reponer.
            .OrderBy(f => Gravedad(f.Estado))
            .ThenBy(f => f.Categoria)
            .ThenBy(f => f.Producto)
            .ToList();

        return new ReporteInventarioDto
        {
            Desde = filtro.Desde,
            Hasta = filtro.Hasta,
            TotalProductos = filas.Count,
            ConStockBajo = filas.Count(f => f.Estado == ReporteInventarioDto.EstadoBajo),
            SinExistencias = filas.Count(f => f.Estado == ReporteInventarioDto.EstadoAgotado),
            TotalEntradas = filas.Sum(f => f.Entradas),
            TotalSalidas = filas.Sum(f => f.Salidas),
            Productos = filas,
            PorCategoria = filas
                .GroupBy(f => new { f.Categoria, f.Area })
                .Select(g => new InventarioCategoriaReporteDto
                {
                    Categoria = g.Key.Categoria,
                    Area = g.Key.Area,
                    Productos = g.Count(),
                    ConStockBajo = g.Count(f => f.Estado != ReporteInventarioDto.EstadoNormal)
                })
                .OrderBy(c => c.Area)
                .ThenBy(c => c.Categoria)
                .ToList()
        };
    }
}
