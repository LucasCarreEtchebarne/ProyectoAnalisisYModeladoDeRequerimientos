using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Prototipo.Common;
using Prototipo.Data;
using Prototipo.Infrastructure;
using Prototipo.Models;

namespace Prototipo.Services
{
    public class ConsultaIAService : ServicioBase
    {
        private readonly BitacoraService _bitacora;

        public ConsultaIAService(HotelColibriContext db) : base(db)
        {
            _bitacora = new BitacoraService(db);
        }

        public List<ConsultaIA> Historial(int cantidad = 30)
        {
            return Db.ConsultasIA
                .Where(c => c.IdUsuario == SesionActual.IdUsuario)
                .OrderByDescending(c => c.FechaRealizada)
                .Take(cantidad)
                .ToList();
        }

        public ConsultaIA Responder(string pregunta)
        {
            var respuesta = Analizar(pregunta ?? string.Empty);

            var consulta = new ConsultaIA
            {
                IdUsuario = SesionActual.IdUsuario,
                ConsultaIngresada = pregunta,
                RespuestaGenerada = respuesta,
                FechaRealizada = DateTime.Now
            };

            Db.ConsultasIA.Add(consulta);
            Db.SaveChanges();

            _bitacora.Registrar(Estados.Modulos.IA, Estados.Acciones.Consultar, consulta.IdConsultaIA,
                "Consulta al asistente");
            Db.SaveChanges();

            return consulta;
        }

        private string Analizar(string pregunta)
        {
            var texto = pregunta.ToLowerInvariant();
            var respuesta = new StringBuilder();

            if (Contiene(texto, "disponible", "habitacion", "habitaciones", "ocupacion"))
            {
                var total = Db.Habitaciones.Count();
                var disponibles = Db.Habitaciones.Count(h => h.EstadoHabitacion == Estados.Habitacion.Disponible);
                var ocupadas = Db.Habitaciones.Count(h => h.EstadoHabitacion == Estados.Habitacion.Ocupada);
                var limpieza = Db.Habitaciones.Count(h => h.EstadoHabitacion == Estados.Habitacion.Limpieza);

                respuesta.AppendLine("Habitaciones: " + total + " en total. " + disponibles + " disponibles, " +
                    ocupadas + " ocupadas y " + limpieza + " en limpieza.");
            }

            if (Contiene(texto, "reserva", "reservas", "llegada", "llegadas", "check"))
            {
                var hoy = DateTime.Today;
                var llegadas = Db.Reservas.Count(r => r.FechaEntrada == hoy && r.EstadoReserva != Estados.Reserva.Cancelada);
                var salidas = Db.Reservas.Count(r => r.FechaSalida == hoy && r.EstadoReserva != Estados.Reserva.Cancelada);
                var pendientes = Db.Reservas.Count(r => r.EstadoReserva == Estados.Reserva.Pendiente);

                respuesta.AppendLine("Reservas: " + llegadas + " llegada(s) y " + salidas + " salida(s) para hoy; " +
                    pendientes + " reserva(s) pendiente(s) de confirmar.");
            }

            if (Contiene(texto, "stock", "inventario", "insumo", "alerta", "agotado"))
            {
                var bajos = Db.Inventario
                    .Where(p => p.EstadoProducto == Estados.Producto.Activo && p.Stock <= p.StockMinimo)
                    .OrderBy(p => p.NombreProducto)
                    .Take(10)
                    .ToList();

                if (bajos.Count == 0)
                {
                    respuesta.AppendLine("Inventario: ningun producto esta bajo el stock minimo.");
                }
                else
                {
                    respuesta.AppendLine("Inventario bajo minimo (" + bajos.Count + "):");
                    foreach (var producto in bajos)
                    {
                        respuesta.AppendLine("  - " + producto.NombreProducto + ": " + producto.Stock + " " +
                            producto.UnidadMedida + " (minimo " + producto.StockMinimo + ")");
                    }
                }
            }

            if (Contiene(texto, "ingreso", "ingresos", "venta", "ventas", "factura", "facturacion", "cobrar", "pago"))
            {
                var hoy = DateTime.Today;
                var manana = hoy.AddDays(1);
                var inicioMes = new DateTime(hoy.Year, hoy.Month, 1);

                var ingresosHoy = Db.Pagos
                    .Where(p => p.EstadoPago == Estados.Pago.Aprobado && p.FechaHoraPago >= hoy && p.FechaHoraPago < manana)
                    .Select(p => (decimal?)p.MontoPagado).Sum() ?? 0m;

                var ingresosMes = Db.Pagos
                    .Where(p => p.EstadoPago == Estados.Pago.Aprobado && p.FechaHoraPago >= inicioMes)
                    .Select(p => (decimal?)p.MontoPagado).Sum() ?? 0m;

                var porCobrar = Db.Facturas
                    .Where(f => f.EstadoFactura == Estados.Factura.Pendiente)
                    .Select(f => (decimal?)f.MontoTotal).Sum() ?? 0m;

                respuesta.AppendLine("Ingresos: " + Formato.Colones(ingresosHoy) + " hoy y " +
                    Formato.Colones(ingresosMes) + " en el mes. Por cobrar: " + Formato.Colones(porCobrar) + ".");
            }

            if (Contiene(texto, "producto", "vendido", "vendidos", "menu", "popular"))
            {
                var top = Db.DetallesPedido
                    .Where(d => d.Pedido.EstadoPedido != Estados.Pedido.Cancelado)
                    .GroupBy(d => d.ProductoMenu.NombreProducto)
                    .Select(g => new { Nombre = g.Key, Unidades = g.Sum(d => d.Cantidad) })
                    .OrderByDescending(g => g.Unidades)
                    .Take(5)
                    .ToList();

                if (top.Count > 0)
                {
                    respuesta.AppendLine("Productos mas vendidos:");
                    foreach (var item in top)
                    {
                        respuesta.AppendLine("  - " + item.Nombre + ": " + item.Unidades + " unidad(es)");
                    }
                }
            }

            if (Contiene(texto, "tarea", "housekeeping", "limpieza"))
            {
                var pendientes = Db.Housekeeping.Count(t => t.EstadoTarea != Estados.Tarea.Completada);
                respuesta.AppendLine("Housekeeping: " + pendientes + " tarea(s) sin completar.");
            }

            if (Contiene(texto, "evento", "eventos", "salon"))
            {
                var proximos = Db.Eventos.Count(e => e.FechaEvento >= DateTime.Today
                                                  && e.EstadoEvento == Estados.Evento.Programado);
                respuesta.AppendLine("Eventos: " + proximos + " evento(s) programado(s) a futuro.");
            }

            if (respuesta.Length == 0)
            {
                return "No encontre datos para esa consulta. Puede preguntar por habitaciones, " +
                       "reservas, inventario, ingresos, productos mas vendidos, housekeeping o eventos.";
            }

            return respuesta.ToString().TrimEnd();
        }

        private static bool Contiene(string texto, params string[] palabras)
        {
            return palabras.Any(texto.Contains);
        }
    }
}
