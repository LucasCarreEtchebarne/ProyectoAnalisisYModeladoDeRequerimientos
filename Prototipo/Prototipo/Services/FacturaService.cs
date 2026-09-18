using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using Prototipo.Common;
using Prototipo.Data;
using Prototipo.Infrastructure;
using Prototipo.Models;

namespace Prototipo.Services
{
    public class FacturaService : ServicioBase
    {
        private readonly BitacoraService _bitacora;

        public FacturaService(HotelColibriContext db) : base(db)
        {
            _bitacora = new BitacoraService(db);
        }

        public List<Factura> Listar(string estado, string tipo, DateTime? desde, DateTime? hasta, string busqueda)
        {
            var consulta = Db.Facturas.Include(f => f.Cliente).AsQueryable();

            if (!string.IsNullOrEmpty(estado))
            {
                consulta = consulta.Where(f => f.EstadoFactura == estado);
            }

            if (!string.IsNullOrEmpty(tipo))
            {
                consulta = consulta.Where(f => f.TipoFactura == tipo);
            }

            if (desde.HasValue)
            {
                consulta = consulta.Where(f => f.FechaEmision >= desde.Value);
            }

            if (hasta.HasValue)
            {
                var limite = hasta.Value.Date.AddDays(1);
                consulta = consulta.Where(f => f.FechaEmision < limite);
            }

            if (!string.IsNullOrWhiteSpace(busqueda))
            {
                consulta = consulta.Where(f => f.NumeroFactura.Contains(busqueda)
                                            || f.Cliente.NombreCompleto.Contains(busqueda)
                                            || f.Cliente.PrimerApellido.Contains(busqueda)
                                            || f.Cliente.Identificacion.Contains(busqueda));
            }

            return consulta.OrderByDescending(f => f.FechaEmision).ToList();
        }

        public Factura Obtener(int id)
        {
            return Db.Facturas
                .Include(f => f.Cliente)
                .Include(f => f.Usuario)
                .Include(f => f.Detalles)
                .Include(f => f.Pagos)
                .FirstOrDefault(f => f.IdFactura == id);
        }

        public decimal TotalPagado(int idFactura)
        {
            return Db.Pagos
                .Where(p => p.IdFactura == idFactura && p.EstadoPago == Estados.Pago.Aprobado)
                .Select(p => (decimal?)p.MontoPagado)
                .Sum() ?? 0m;
        }

        public decimal SaldoPendiente(Factura factura)
        {
            return factura.MontoTotal - TotalPagado(factura.IdFactura);
        }

        public ResultadoOperacion Crear(int idCliente, string tipoFactura, List<DetalleFactura> detalles)
        {
            if (detalles == null || detalles.Count == 0)
            {
                return ResultadoOperacion.Error(Mensajes.FacturaSinDetalle);
            }

            var factura = new Factura
            {
                IdCliente = idCliente,
                IdUsuario = SesionActual.IdUsuario,
                FechaEmision = DateTime.Now,
                TipoFactura = tipoFactura,
                EstadoFactura = Estados.Factura.Pendiente,
                MontoTotal = 0m
            };

            Db.Facturas.Add(factura);

            decimal total = 0m;
            foreach (var detalle in detalles)
            {
                detalle.Factura = factura;
                Db.DetallesFactura.Add(detalle);
                total += detalle.Cantidad * detalle.PrecioUnitario;
            }

            factura.MontoTotal = total;
            Db.SaveChanges();

            _bitacora.Registrar(Estados.Modulos.Facturacion, Estados.Acciones.Crear, factura.IdFactura,
                "Factura " + factura.NumeroFactura + " emitida por " + Formato.Colones(total));
            Db.SaveChanges();

            return ResultadoOperacion.Ok("Factura emitida por " + Formato.Colones(total) + ".", factura.IdFactura);
        }

        public ResultadoOperacion CrearDesdePedido(int idPedido)
        {
            var pedido = Db.Pedidos
                .Include(p => p.Detalles.Select(d => d.ProductoMenu))
                .FirstOrDefault(p => p.IdPedido == idPedido);

            if (pedido == null)
            {
                return ResultadoOperacion.Error(Mensajes.NoEncontrado);
            }

            if (!pedido.IdCliente.HasValue)
            {
                return ResultadoOperacion.Error("El pedido no tiene cliente asociado; asigne uno antes de facturar.");
            }

            if (pedido.EstadoPedido == Estados.Pedido.Cancelado)
            {
                return ResultadoOperacion.Error("Un pedido cancelado no se puede facturar.");
            }

            if (Db.DetallesFactura.Any(d => d.IdPedido == idPedido))
            {
                return ResultadoOperacion.Error("El pedido ya fue facturado.");
            }

            var detalles = pedido.Detalles.Select(d => new DetalleFactura
            {
                TipoConcepto = Estados.TipoConcepto.Restaurante,
                IdPedido = pedido.IdPedido,
                Descripcion = d.ProductoMenu.NombreProducto,
                Cantidad = d.Cantidad,
                PrecioUnitario = d.PrecioUnitario
            }).ToList();

            return Crear(pedido.IdCliente.Value, Estados.TipoFactura.Restaurante, detalles);
        }

        public ResultadoOperacion CrearDesdeReserva(int idReserva)
        {
            var reserva = Db.Reservas
                .Include(r => r.Habitacion)
                .FirstOrDefault(r => r.IdReserva == idReserva);

            if (reserva == null)
            {
                return ResultadoOperacion.Error(Mensajes.NoEncontrado);
            }

            if (reserva.EstadoReserva == Estados.Reserva.Cancelada)
            {
                return ResultadoOperacion.Error("Una reserva cancelada no se puede facturar.");
            }

            if (Db.DetallesFactura.Any(d => d.IdReserva == idReserva))
            {
                return ResultadoOperacion.Error("La reserva ya fue facturada.");
            }

            var detalles = new List<DetalleFactura>
            {
                new DetalleFactura
                {
                    TipoConcepto = Estados.TipoConcepto.Hospedaje,
                    IdReserva = reserva.IdReserva,
                    Descripcion = "Habitacion " + reserva.Habitacion.NumeroHabitacion + " - " +
                                  reserva.CantidadNoches + " noche(s)",
                    Cantidad = reserva.CantidadNoches,
                    PrecioUnitario = reserva.PrecioNoche
                }
            };

            return Crear(reserva.IdCliente, Estados.TipoFactura.Hospedaje, detalles);
        }

        public ResultadoOperacion CrearDesdeEvento(int idEvento)
        {
            var evento = Db.Eventos.Include(e => e.Espacio).FirstOrDefault(e => e.IdEvento == idEvento);
            if (evento == null)
            {
                return ResultadoOperacion.Error(Mensajes.NoEncontrado);
            }

            if (evento.EstadoEvento == Estados.Evento.Cancelado)
            {
                return ResultadoOperacion.Error("Un evento cancelado no se puede facturar.");
            }

            if (Db.DetallesFactura.Any(d => d.IdEvento == idEvento))
            {
                return ResultadoOperacion.Error("El evento ya fue facturado.");
            }

            var detalles = new List<DetalleFactura>
            {
                new DetalleFactura
                {
                    TipoConcepto = Estados.TipoConcepto.Evento,
                    IdEvento = evento.IdEvento,
                    Descripcion = evento.NombreEvento + " - " + evento.Espacio.NombreEspacio,
                    Cantidad = 1,
                    PrecioUnitario = evento.MontoAcordado
                }
            };

            return Crear(evento.IdCliente, Estados.TipoFactura.Evento, detalles);
        }

        public ResultadoOperacion CrearMixtaDeReserva(int idReserva)
        {
            var reserva = Db.Reservas.Include(r => r.Habitacion).FirstOrDefault(r => r.IdReserva == idReserva);
            if (reserva == null)
            {
                return ResultadoOperacion.Error(Mensajes.NoEncontrado);
            }

            var detalles = new List<DetalleFactura>();

            if (!Db.DetallesFactura.Any(d => d.IdReserva == idReserva))
            {
                detalles.Add(new DetalleFactura
                {
                    TipoConcepto = Estados.TipoConcepto.Hospedaje,
                    IdReserva = reserva.IdReserva,
                    Descripcion = "Habitacion " + reserva.Habitacion.NumeroHabitacion + " - " +
                                  reserva.CantidadNoches + " noche(s)",
                    Cantidad = reserva.CantidadNoches,
                    PrecioUnitario = reserva.PrecioNoche
                });
            }

            var pedidos = Db.Pedidos
                .Include(p => p.Detalles.Select(d => d.ProductoMenu))
                .Where(p => p.IdReserva == idReserva && p.EstadoPedido != Estados.Pedido.Cancelado)
                .ToList();

            foreach (var pedido in pedidos)
            {
                if (Db.DetallesFactura.Any(d => d.IdPedido == pedido.IdPedido))
                {
                    continue;
                }

                foreach (var linea in pedido.Detalles)
                {
                    detalles.Add(new DetalleFactura
                    {
                        TipoConcepto = Estados.TipoConcepto.Restaurante,
                        IdPedido = pedido.IdPedido,
                        Descripcion = linea.ProductoMenu.NombreProducto,
                        Cantidad = linea.Cantidad,
                        PrecioUnitario = linea.PrecioUnitario
                    });
                }
            }

            if (detalles.Count == 0)
            {
                return ResultadoOperacion.Error("No hay conceptos pendientes de facturar para esa reserva.");
            }

            var tipo = detalles.Select(d => d.TipoConcepto).Distinct().Count() > 1
                ? Estados.TipoFactura.Mixta
                : Estados.TipoFactura.Hospedaje;

            return Crear(reserva.IdCliente, tipo, detalles);
        }

        public ResultadoOperacion Anular(int idFactura, string motivo)
        {
            if (string.IsNullOrWhiteSpace(motivo))
            {
                return ResultadoOperacion.Error(Mensajes.MotivoAnulacionRequerido);
            }

            var factura = Obtener(idFactura);
            if (factura == null)
            {
                return ResultadoOperacion.Error(Mensajes.NoEncontrado);
            }

            if (factura.EstadoFactura == Estados.Factura.Anulada)
            {
                return ResultadoOperacion.Error(Mensajes.FacturaAnulada);
            }

            factura.EstadoFactura = Estados.Factura.Anulada;
            factura.MotivoAnulacion = motivo;
            factura.FechaAnulacion = DateTime.Now;

            foreach (var pago in factura.Pagos.Where(p => p.EstadoPago == Estados.Pago.Aprobado))
            {
                pago.EstadoPago = Estados.Pago.Reversado;
            }

            _bitacora.Registrar(Estados.Modulos.Facturacion, Estados.Acciones.Anular, factura.IdFactura,
                "Factura " + factura.NumeroFactura + " anulada: " + motivo);
            Db.SaveChanges();

            return ResultadoOperacion.Ok("La factura fue anulada.", factura.IdFactura);
        }

        internal void ActualizarEstadoPorPagos(Factura factura)
        {
            if (factura.EstadoFactura == Estados.Factura.Anulada)
            {
                return;
            }

            var pagado = TotalPagado(factura.IdFactura);
            factura.EstadoFactura = pagado >= factura.MontoTotal
                ? Estados.Factura.Pagada
                : Estados.Factura.Pendiente;
        }
    }
}
