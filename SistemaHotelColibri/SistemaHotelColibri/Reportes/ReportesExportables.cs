using SistemaHotelColibri.Abstract.Comun;
using SistemaHotelColibri.Abstract.Modelos.Reportes;

namespace SistemaHotelColibri.Reportes;

// Arma la versión exportable (Excel / PDF) de cada reporte.
public static class ReportesExportables
{
    public static string Periodo(DateOnly desde, DateOnly hasta) =>
        $"Del {Formato.Fecha(desde)} al {Formato.Fecha(hasta)}";

    private static string Archivo(string nombre, DateOnly desde, DateOnly hasta) =>
        $"{nombre}_{desde:yyyyMMdd}_{hasta:yyyyMMdd}";

    public static ReporteExportable Ocupacion(ReporteOcupacionDto r, string? filtros)
    {
        var porHabitacion = new TablaExportable { Titulo = "Ocupación por habitación" }
            .Columna("Habitación").Columna("Tipo").Columna("Estado actual")
            .Columna("Reservas", TipoColumna.Entero).Columna("Noches ocupadas", TipoColumna.Entero)
            .Columna("Ocupación", TipoColumna.Porcentaje).Columna("Ingreso estimado", TipoColumna.Dinero);
        foreach (var h in r.Habitaciones)
        {
            porHabitacion.Fila(h.NumeroHabitacion, h.TipoHabitacion, h.EstadoActual, h.Reservas, h.NochesOcupadas, h.PorcentajeOcupacion, h.Ingreso);
        }

        porHabitacion.ConTotal("Total", null, null, r.CantidadReservas, r.NochesOcupadas, r.PorcentajeOcupacion, r.IngresoHospedaje);

        var porTipo = new TablaExportable { Titulo = "Ocupación por tipo" }
            .Columna("Tipo").Columna("Habitaciones", TipoColumna.Entero).Columna("Noches ocupadas", TipoColumna.Entero)
            .Columna("Noches disponibles", TipoColumna.Entero).Columna("Ocupación", TipoColumna.Porcentaje).Columna("Ingreso estimado", TipoColumna.Dinero);
        foreach (var t in r.PorTipo)
        {
            porTipo.Fila(t.TipoHabitacion, t.Habitaciones, t.NochesOcupadas, t.NochesDisponibles, t.PorcentajeOcupacion, t.Ingreso);
        }

        var porDia = new TablaExportable { Titulo = "Ocupación por día" }
            .Columna("Fecha", TipoColumna.Fecha).Columna("Ocupadas", TipoColumna.Entero)
            .Columna("Total", TipoColumna.Entero).Columna("Ocupación", TipoColumna.Porcentaje);
        foreach (var d in r.PorDia)
        {
            porDia.Fila(d.Fecha, d.Ocupadas, d.Total, d.PorcentajeOcupacion);
        }

        return new ReporteExportable
        {
            Codigo = "REP-001",
            Titulo = "Reporte de ocupación de habitaciones",
            Periodo = Periodo(r.Desde, r.Hasta),
            Filtros = filtros,
            NombreArchivo = Archivo("ocupacion", r.Desde, r.Hasta),
            Resumen =
            [
                ("Ocupación del período", FormatoExportable.Texto(r.PorcentajeOcupacion, TipoColumna.Porcentaje)),
                ("Noches ocupadas", $"{r.NochesOcupadas} de {r.NochesDisponibles}"),
                ("Habitaciones", r.TotalHabitaciones.ToString()),
                ("Reservas", r.CantidadReservas.ToString()),
                ("Ingreso estimado de hospedaje", Formato.Colones(r.IngresoHospedaje)),
                ("Tarifa promedio por noche", Formato.Colones(r.TarifaPromedio))
            ],
            Tablas = [porHabitacion, porTipo, porDia]
        };
    }

    public static ReporteExportable Ventas(ReporteVentasDto r)
    {
        var productos = new TablaExportable { Titulo = "Productos vendidos" }
            .Columna("Producto").Columna("Categoría").Columna("Cantidad", TipoColumna.Entero)
            .Columna("Monto", TipoColumna.Dinero).Columna("% de las ventas", TipoColumna.Porcentaje);
        foreach (var p in r.Productos)
        {
            productos.Fila(p.Producto, p.Categoria, p.Cantidad, p.Monto, p.Porcentaje);
        }

        productos.ConTotal("Total", null, r.ProductosVendidos, r.Ventas, r.Ventas == 0 ? 0m : 100m);

        TablaExportable Grupo(string titulo, string nombre, List<VentaGrupoReporteDto> grupos)
        {
            var tabla = new TablaExportable { Titulo = titulo }
                .Columna(nombre).Columna("Pedidos", TipoColumna.Entero).Columna("Productos", TipoColumna.Entero)
                .Columna("Monto", TipoColumna.Dinero).Columna("% de las ventas", TipoColumna.Porcentaje);
            foreach (var g in grupos)
            {
                tabla.Fila(g.Nombre, g.Pedidos, g.Cantidad, g.Monto, g.Porcentaje);
            }

            return tabla;
        }

        var porDia = new TablaExportable { Titulo = "Ventas por día" }
            .Columna("Fecha", TipoColumna.Fecha).Columna("Pedidos", TipoColumna.Entero).Columna("Monto", TipoColumna.Dinero);
        foreach (var d in r.PorDia)
        {
            porDia.Fila(d.Fecha, d.Pedidos, d.Monto);
        }

        return new ReporteExportable
        {
            Codigo = "REP-002",
            Titulo = "Reporte de ventas del restaurante",
            Periodo = Periodo(r.Desde, r.Hasta),
            Filtros = "Pedidos facturados en el período (sin facturas anuladas). Montos sin servicio ni IVA.",
            NombreArchivo = Archivo("ventas_restaurante", r.Desde, r.Hasta),
            Resumen =
            [
                ("Ventas", Formato.Colones(r.Ventas)),
                ("Servicio (10 %)", Formato.Colones(r.Servicio)),
                ("Pedidos", r.CantidadPedidos.ToString()),
                ("Ticket promedio", Formato.Colones(r.TicketPromedio)),
                ("Productos vendidos", r.ProductosVendidos.ToString())
            ],
            Tablas =
            [
                productos,
                Grupo("Ventas por categoría", "Categoría", r.PorCategoria),
                Grupo("Ventas por origen", "Origen", r.PorOrigen),
                porDia
            ]
        };
    }

    public static ReporteExportable Ingresos(ReporteIngresosDto r)
    {
        var porConcepto = new TablaExportable { Titulo = "Ingresos por concepto" }
            .Columna("Concepto").Columna("Líneas", TipoColumna.Entero)
            .Columna("Subtotal", TipoColumna.Dinero).Columna("% del subtotal", TipoColumna.Porcentaje);
        foreach (var c in r.PorConcepto)
        {
            porConcepto.Fila(c.Concepto, c.Lineas, c.Monto, c.Porcentaje);
        }

        porConcepto.ConTotal("Total", r.PorConcepto.Sum(c => c.Lineas), r.Subtotal, r.Subtotal == 0 ? 0m : 100m);

        var porMetodo = new TablaExportable { Titulo = "Cobros por método de pago" }
            .Columna("Método").Columna("Pagos", TipoColumna.Entero)
            .Columna("Monto", TipoColumna.Dinero).Columna("% de lo cobrado", TipoColumna.Porcentaje);
        foreach (var m in r.PorMetodoPago)
        {
            porMetodo.Fila(m.MetodoPago, m.Pagos, m.Monto, m.Porcentaje);
        }

        var porDia = new TablaExportable { Titulo = "Ingresos por día" }
            .Columna("Fecha", TipoColumna.Fecha).Columna("Facturado", TipoColumna.Dinero).Columna("Cobrado", TipoColumna.Dinero);
        foreach (var d in r.PorDia)
        {
            porDia.Fila(d.Fecha, d.Facturado, d.Cobrado);
        }

        var facturas = new TablaExportable { Titulo = "Facturas del período" }
            .Columna("Factura").Columna("Fecha", TipoColumna.Fecha).Columna("Tipo").Columna("Cliente").Columna("Estado")
            .Columna("Total", TipoColumna.Dinero).Columna("Saldo", TipoColumna.Dinero);
        foreach (var f in r.Facturas)
        {
            facturas.Fila(f.NumeroFactura, DateOnly.FromDateTime(f.FechaEmision), f.TipoFactura, f.ClienteMostrado, f.EstadoFactura, f.MontoTotal, f.Saldo);
        }

        return new ReporteExportable
        {
            Codigo = "REP-003",
            Titulo = "Reporte de ingresos totales",
            Periodo = Periodo(r.Desde, r.Hasta),
            Filtros = "Facturado: facturas emitidas en el período (sin anuladas). Cobrado: pagos aprobados en el período.",
            NombreArchivo = Archivo("ingresos_totales", r.Desde, r.Hasta),
            Resumen =
            [
                ("Total facturado", Formato.Colones(r.TotalFacturado)),
                ("Total cobrado", Formato.Colones(r.TotalCobrado)),
                ("Pendiente por cobrar", Formato.Colones(r.PendientePorCobrar)),
                ("Facturas emitidas", r.FacturasEmitidas.ToString()),
                ("Subtotal", Formato.Colones(r.Subtotal)),
                ("Servicio", Formato.Colones(r.Servicio)),
                ("IVA", Formato.Colones(r.Iva)),
                ("Facturas anuladas", r.FacturasAnuladas.ToString())
            ],
            Tablas = [porConcepto, porMetodo, porDia, facturas]
        };
    }

    public static ReporteExportable Inventario(ReporteInventarioDto r, string? filtros)
    {
        var productos = new TablaExportable { Titulo = "Existencias" }
            .Columna("Producto").Columna("Categoría").Columna("Área").Columna("Unidad")
            .Columna("Stock", TipoColumna.Decimal).Columna("Mínimo", TipoColumna.Decimal).Columna("Faltante", TipoColumna.Decimal)
            .Columna("Entradas", TipoColumna.Decimal).Columna("Salidas", TipoColumna.Decimal).Columna("Estado");
        foreach (var p in r.Productos)
        {
            productos.Fila(p.Producto, p.Categoria, p.Area, p.UnidadMedida, p.Stock, p.StockMinimo, p.Faltante, p.Entradas, p.Salidas, p.Estado);
        }

        var porCategoria = new TablaExportable { Titulo = "Resumen por categoría" }
            .Columna("Categoría").Columna("Área").Columna("Productos", TipoColumna.Entero).Columna("Por reponer", TipoColumna.Entero);
        foreach (var c in r.PorCategoria)
        {
            porCategoria.Fila(c.Categoria, c.Area, c.Productos, c.ConStockBajo);
        }

        return new ReporteExportable
        {
            Codigo = "REP-004",
            Titulo = "Reporte de inventario",
            Periodo = $"Existencias al {Formato.Fecha(DateOnly.FromDateTime(DateTime.Today))} · Movimientos {Periodo(r.Desde, r.Hasta).ToLowerInvariant()}",
            Filtros = filtros,
            NombreArchivo = $"inventario_{DateTime.Today:yyyyMMdd}",
            Resumen =
            [
                ("Productos activos", r.TotalProductos.ToString()),
                ("Con stock bajo", r.ConStockBajo.ToString()),
                ("Sin existencias", r.SinExistencias.ToString()),
                ("Entradas del período", Formato.Cantidad(r.TotalEntradas)),
                ("Salidas del período", Formato.Cantidad(r.TotalSalidas))
            ],
            Tablas = [productos, porCategoria]
        };
    }

    public static ReporteExportable Reservas(ReporteReservasDto r, string? filtros)
    {
        var reservas = new TablaExportable { Titulo = "Reservas" }
            .Columna("Reserva", TipoColumna.Entero).Columna("Cliente").Columna("Identificación").Columna("Habitación")
            .Columna("Entrada", TipoColumna.Fecha).Columna("Salida", TipoColumna.Fecha).Columna("Noches", TipoColumna.Entero)
            .Columna("Huéspedes", TipoColumna.Entero).Columna("Monto", TipoColumna.Dinero).Columna("Estado");
        foreach (var x in r.Reservas)
        {
            reservas.Fila(x.IdReserva, x.NombreCliente, x.IdentificacionCliente, $"{x.NumeroHabitacion} ({x.TipoHabitacion})",
                x.FechaEntrada, x.FechaSalida, x.CantidadNoches, x.CantidadHuespedes, x.MontoHospedaje, x.EstadoReserva);
        }

        var porEstado = new TablaExportable { Titulo = "Reservas por estado" }
            .Columna("Estado").Columna("Reservas", TipoColumna.Entero).Columna("Noches", TipoColumna.Entero).Columna("Monto", TipoColumna.Dinero);
        foreach (var e in r.PorEstado)
        {
            porEstado.Fila(e.Estado, e.Reservas, e.Noches, e.Monto);
        }

        return new ReporteExportable
        {
            Codigo = "REP-005",
            Titulo = "Reporte de reservas",
            Periodo = $"Entradas {Periodo(r.Desde, r.Hasta).ToLowerInvariant()}",
            Filtros = filtros,
            NombreArchivo = Archivo("reservas", r.Desde, r.Hasta),
            Resumen =
            [
                ("Reservas", r.TotalReservas.ToString()),
                ("Noches", r.TotalNoches.ToString()),
                ("Huéspedes", r.TotalHuespedes.ToString()),
                ("Monto de hospedaje", Formato.Colones(r.MontoTotal)),
                ("Estancia promedio", $"{r.EstanciaPromedio} noches")
            ],
            Tablas = [reservas, porEstado]
        };
    }
}
