using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using Prototipo.Common;
using Prototipo.Data;
using Prototipo.ViewModels;

namespace Prototipo.Services
{
    public class ReporteService : ServicioBase
    {
        public ReporteService(HotelColibriContext db) : base(db)
        {
        }

        public DashboardViewModel ObtenerDashboard()
        {
            var hoy = DateTime.Today;
            var manana = hoy.AddDays(1);
            var inicioMes = new DateTime(hoy.Year, hoy.Month, 1);

            var modelo = new DashboardViewModel
            {
                HabitacionesTotales = Db.Habitaciones.Count(),
                HabitacionesOcupadas = Db.Habitaciones.Count(h => h.EstadoHabitacion == Estados.Habitacion.Ocupada),
                HabitacionesDisponibles = Db.Habitaciones.Count(h => h.EstadoHabitacion == Estados.Habitacion.Disponible),
                HabitacionesLimpieza = Db.Habitaciones.Count(h => h.EstadoHabitacion == Estados.Habitacion.Limpieza),

                LlegadasHoy = Db.Reservas.Count(r => r.FechaEntrada == hoy && r.EstadoReserva != Estados.Reserva.Cancelada),
                SalidasHoy = Db.Reservas.Count(r => r.FechaSalida == hoy && r.EstadoReserva != Estados.Reserva.Cancelada),
                ReservasPendientes = Db.Reservas.Count(r => r.EstadoReserva == Estados.Reserva.Pendiente),

                PedidosHoy = Db.Pedidos.Count(p => p.FechaHoraPedido >= hoy && p.FechaHoraPedido < manana),
                PedidosPendientes = Db.Pedidos.Count(p => p.EstadoPedido == Estados.Pedido.Pendiente),

                FacturasPendientes = Db.Facturas.Count(f => f.EstadoFactura == Estados.Factura.Pendiente),
                TareasPendientes = Db.Housekeeping.Count(t => t.EstadoTarea != Estados.Tarea.Completada),
                EventosProximos = Db.Eventos.Count(e => e.FechaEvento >= hoy && e.EstadoEvento == Estados.Evento.Programado)
            };

            modelo.IngresosHoy = Db.Pagos
                .Where(p => p.EstadoPago == Estados.Pago.Aprobado
                         && p.FechaHoraPago >= hoy && p.FechaHoraPago < manana)
                .Select(p => (decimal?)p.MontoPagado).Sum() ?? 0m;

            modelo.IngresosMes = Db.Pagos
                .Where(p => p.EstadoPago == Estados.Pago.Aprobado && p.FechaHoraPago >= inicioMes)
                .Select(p => (decimal?)p.MontoPagado).Sum() ?? 0m;

            modelo.MontoPorCobrar = Db.Facturas
                .Where(f => f.EstadoFactura == Estados.Factura.Pendiente)
                .Select(f => (decimal?)f.MontoTotal).Sum() ?? 0m;

            modelo.AlertasStock = Db.Inventario
                .Where(p => p.EstadoProducto == Estados.Producto.Activo && p.Stock <= p.StockMinimo)
                .OrderBy(p => p.NombreProducto)
                .Take(10)
                .ToList();

            modelo.ProximasLlegadas = Db.Reservas
                .Include(r => r.Cliente)
                .Include(r => r.Habitacion)
                .Where(r => r.FechaEntrada >= hoy
                         && (r.EstadoReserva == Estados.Reserva.Confirmada
                          || r.EstadoReserva == Estados.Reserva.Pendiente))
                .OrderBy(r => r.FechaEntrada)
                .Take(8)
                .ToList();

            return modelo;
        }

        public ReporteViewModel Generar(string tipo, DateTime desde, DateTime hasta)
        {
            var limite = hasta.Date.AddDays(1);
            var reporte = new ReporteViewModel
            {
                Tipo = tipo,
                Desde = desde,
                Hasta = hasta,
                Titulo = ReporteViewModel.Tipos.ContainsKey(tipo ?? string.Empty)
                    ? ReporteViewModel.Tipos[tipo]
                    : "Reporte"
            };

            switch (tipo)
            {
                case "ingresos":
                    GenerarIngresos(reporte, desde, limite);
                    break;
                case "ocupacion":
                    GenerarOcupacion(reporte, desde, hasta);
                    break;
                case "consumo":
                    GenerarConsumo(reporte, desde, limite);
                    break;
                case "inventario":
                    GenerarInventario(reporte);
                    break;
                case "clientes":
                    GenerarClientes(reporte, desde, limite);
                    break;
                default:
                    reporte.Resumen = "Seleccione un tipo de reporte.";
                    break;
            }

            return reporte;
        }

        private void GenerarIngresos(ReporteViewModel reporte, DateTime desde, DateTime limite)
        {
            reporte.Encabezados = new List<string> { "Fecha", "Metodo de pago", "Pagos", "Monto" };

            var datos = Db.Pagos
                .Where(p => p.EstadoPago == Estados.Pago.Aprobado
                         && p.FechaHoraPago >= desde && p.FechaHoraPago < limite)
                .GroupBy(p => new { Anio = p.FechaHoraPago.Year, Mes = p.FechaHoraPago.Month, Dia = p.FechaHoraPago.Day, p.MetodoPago })
                .Select(g => new
                {
                    g.Key.Anio,
                    g.Key.Mes,
                    g.Key.Dia,
                    g.Key.MetodoPago,
                    Cantidad = g.Count(),
                    Monto = g.Sum(p => p.MontoPagado)
                })
                .OrderBy(g => g.Anio).ThenBy(g => g.Mes).ThenBy(g => g.Dia)
                .ToList();

            foreach (var fila in datos)
            {
                reporte.Filas.Add(new List<string>
                {
                    Formato.Fecha(new DateTime(fila.Anio, fila.Mes, fila.Dia)),
                    fila.MetodoPago,
                    fila.Cantidad.ToString(),
                    Formato.Colones(fila.Monto)
                });
            }

            var total = datos.Sum(d => d.Monto);
            reporte.Resumen = "Total recaudado en el periodo: " + Formato.Colones(total);
        }

        private void GenerarOcupacion(ReporteViewModel reporte, DateTime desde, DateTime hasta)
        {
            reporte.Encabezados = new List<string> { "Habitacion", "Tipo", "Reservas", "Noches vendidas", "Ingreso de hospedaje" };

            var datos = Db.Reservas
                .Include(r => r.Habitacion)
                .Where(r => r.EstadoReserva != Estados.Reserva.Cancelada
                         && r.FechaEntrada <= hasta && r.FechaSalida >= desde)
                .GroupBy(r => new { r.Habitacion.NumeroHabitacion, r.Habitacion.TipoHabitacion })
                .Select(g => new
                {
                    g.Key.NumeroHabitacion,
                    g.Key.TipoHabitacion,
                    Reservas = g.Count(),
                    Noches = g.Sum(r => r.CantidadNoches),
                    Monto = g.Sum(r => r.MontoHospedaje)
                })
                .OrderBy(g => g.NumeroHabitacion)
                .ToList();

            foreach (var fila in datos)
            {
                reporte.Filas.Add(new List<string>
                {
                    fila.NumeroHabitacion,
                    fila.TipoHabitacion,
                    fila.Reservas.ToString(),
                    fila.Noches.ToString(),
                    Formato.Colones(fila.Monto)
                });
            }

            reporte.Resumen = "Noches vendidas: " + datos.Sum(d => d.Noches) +
                " | Hospedaje facturable: " + Formato.Colones(datos.Sum(d => d.Monto));
        }

        private void GenerarConsumo(ReporteViewModel reporte, DateTime desde, DateTime limite)
        {
            reporte.Encabezados = new List<string> { "Producto", "Categoria", "Unidades", "Venta" };

            var datos = Db.DetallesPedido
                .Where(d => d.Pedido.EstadoPedido != Estados.Pedido.Cancelado
                         && d.Pedido.FechaHoraPedido >= desde && d.Pedido.FechaHoraPedido < limite)
                .GroupBy(d => new { d.ProductoMenu.NombreProducto, d.ProductoMenu.CategoriaMenu })
                .Select(g => new
                {
                    g.Key.NombreProducto,
                    g.Key.CategoriaMenu,
                    Unidades = g.Sum(d => d.Cantidad),
                    Venta = g.Sum(d => d.Subtotal)
                })
                .OrderByDescending(g => g.Unidades)
                .ToList();

            foreach (var fila in datos)
            {
                reporte.Filas.Add(new List<string>
                {
                    fila.NombreProducto,
                    fila.CategoriaMenu,
                    fila.Unidades.ToString(),
                    Formato.Colones(fila.Venta)
                });
            }

            reporte.Resumen = "Venta de restaurante en el periodo: " + Formato.Colones(datos.Sum(d => d.Venta));
        }

        private void GenerarInventario(ReporteViewModel reporte)
        {
            reporte.Encabezados = new List<string> { "Producto", "Categoria", "Stock", "Minimo", "Estado" };

            var datos = Db.Inventario.OrderBy(p => p.NombreProducto).ToList();

            foreach (var producto in datos)
            {
                reporte.Filas.Add(new List<string>
                {
                    producto.NombreProducto,
                    producto.CategoriaProducto,
                    producto.Stock + " " + producto.UnidadMedida,
                    producto.StockMinimo.ToString(),
                    producto.StockBajo ? "Bajo minimo" : "Normal"
                });
            }

            reporte.Resumen = datos.Count(p => p.StockBajo) + " producto(s) bajo el minimo.";
        }

        private void GenerarClientes(ReporteViewModel reporte, DateTime desde, DateTime limite)
        {
            reporte.Encabezados = new List<string> { "Cliente", "Identificacion", "Facturas", "Total facturado" };

            var datos = Db.Facturas
                .Include(f => f.Cliente)
                .Where(f => f.EstadoFactura != Estados.Factura.Anulada
                         && f.FechaEmision >= desde && f.FechaEmision < limite)
                .GroupBy(f => new { f.Cliente.NombreCompleto, f.Cliente.PrimerApellido, f.Cliente.Identificacion })
                .Select(g => new
                {
                    g.Key.NombreCompleto,
                    g.Key.PrimerApellido,
                    g.Key.Identificacion,
                    Facturas = g.Count(),
                    Total = g.Sum(f => f.MontoTotal)
                })
                .OrderByDescending(g => g.Total)
                .ToList();

            foreach (var fila in datos)
            {
                reporte.Filas.Add(new List<string>
                {
                    fila.NombreCompleto + " " + fila.PrimerApellido,
                    fila.Identificacion,
                    fila.Facturas.ToString(),
                    Formato.Colones(fila.Total)
                });
            }

            reporte.Resumen = "Facturacion del periodo: " + Formato.Colones(datos.Sum(d => d.Total));
        }
    }
}
