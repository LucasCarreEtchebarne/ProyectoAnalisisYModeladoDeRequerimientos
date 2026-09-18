using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using Prototipo.Common;
using Prototipo.Data;
using Prototipo.Models;

namespace Prototipo.Services
{
    public class PedidoService : ServicioBase
    {
        private readonly BitacoraService _bitacora;
        private readonly InventarioService _inventario;

        public PedidoService(HotelColibriContext db) : base(db)
        {
            _bitacora = new BitacoraService(db);
            _inventario = new InventarioService(db);
        }

        public List<Pedido> Listar(string estado, string tipo, DateTime? desde, DateTime? hasta)
        {
            var consulta = Db.Pedidos
                .Include(p => p.Cliente)
                .Include(p => p.Mesa)
                .Include(p => p.Habitacion)
                .Include(p => p.Usuario)
                .Include(p => p.Detalles)
                .AsQueryable();

            if (!string.IsNullOrEmpty(estado))
            {
                consulta = consulta.Where(p => p.EstadoPedido == estado);
            }

            if (!string.IsNullOrEmpty(tipo))
            {
                consulta = consulta.Where(p => p.TipoPedido == tipo);
            }

            if (desde.HasValue)
            {
                consulta = consulta.Where(p => p.FechaHoraPedido >= desde.Value);
            }

            if (hasta.HasValue)
            {
                var limite = hasta.Value.Date.AddDays(1);
                consulta = consulta.Where(p => p.FechaHoraPedido < limite);
            }

            return consulta.OrderByDescending(p => p.FechaHoraPedido).ToList();
        }

        public Pedido Obtener(int id)
        {
            return Db.Pedidos
                .Include(p => p.Cliente)
                .Include(p => p.Mesa)
                .Include(p => p.Habitacion)
                .Include(p => p.Usuario)
                .Include(p => p.Detalles.Select(d => d.ProductoMenu))
                .FirstOrDefault(p => p.IdPedido == id);
        }

        private ResultadoOperacion ValidarOrigen(Pedido pedido)
        {
            if (pedido.TipoPedido == Estados.TipoPedido.Mesa)
            {
                if (!pedido.IdMesa.HasValue)
                {
                    return ResultadoOperacion.Error(Mensajes.PedidoRequiereMesa);
                }

                pedido.IdHabitacion = null;
                return ResultadoOperacion.Ok(string.Empty);
            }

            if (pedido.TipoPedido == Estados.TipoPedido.Habitacion)
            {
                if (!pedido.IdHabitacion.HasValue)
                {
                    return ResultadoOperacion.Error(Mensajes.PedidoRequiereHabitacion);
                }

                pedido.IdMesa = null;

                var reserva = Db.Reservas.FirstOrDefault(r => r.IdHabitacion == pedido.IdHabitacion.Value
                                                          && r.EstadoReserva == Estados.Reserva.CheckIn);
                if (reserva == null)
                {
                    return ResultadoOperacion.Error(Mensajes.PedidoRequiereHabitacion);
                }

                pedido.IdReserva = reserva.IdReserva;
                if (!pedido.IdCliente.HasValue)
                {
                    pedido.IdCliente = reserva.IdCliente;
                }

                return ResultadoOperacion.Ok(string.Empty);
            }

            pedido.IdMesa = null;
            pedido.IdHabitacion = null;
            return ResultadoOperacion.Ok(string.Empty);
        }

        public ResultadoOperacion Crear(Pedido pedido, List<DetallePedido> detalles)
        {
            if (detalles == null || detalles.Count == 0)
            {
                return ResultadoOperacion.Error(Mensajes.PedidoSinDetalle);
            }

            var validacion = ValidarOrigen(pedido);
            if (!validacion.Exito)
            {
                return validacion;
            }

            pedido.FechaHoraPedido = DateTime.Now;
            pedido.EstadoPedido = Estados.Pedido.Pendiente;
            Db.Pedidos.Add(pedido);

            foreach (var detalle in detalles)
            {
                var producto = Db.Menu.FirstOrDefault(m => m.IdProductoMenu == detalle.IdProductoMenu);
                if (producto == null)
                {
                    return ResultadoOperacion.Error(Mensajes.NoEncontrado);
                }

                Db.DetallesPedido.Add(new DetallePedido
                {
                    Pedido = pedido,
                    IdProductoMenu = producto.IdProductoMenu,
                    Cantidad = detalle.Cantidad,
                    PrecioUnitario = producto.Precio
                });
            }

            Db.SaveChanges();

            if (pedido.IdMesa.HasValue)
            {
                var mesa = Db.Mesas.First(m => m.IdMesa == pedido.IdMesa.Value);
                mesa.EstadoMesa = Estados.Mesa.Ocupada;
            }

            _bitacora.Registrar(Estados.Modulos.Restaurante, Estados.Acciones.Crear, pedido.IdPedido,
                "Pedido creado (" + pedido.TipoPedido + ")");
            Db.SaveChanges();

            return ResultadoOperacion.Ok(Mensajes.CreadoOk, pedido.IdPedido);
        }

        public ResultadoOperacion Confirmar(int idPedido)
        {
            var pedido = Obtener(idPedido);
            if (pedido == null)
            {
                return ResultadoOperacion.Error(Mensajes.NoEncontrado);
            }

            if (pedido.EstadoPedido != Estados.Pedido.Pendiente)
            {
                return ResultadoOperacion.Error(Mensajes.PedidoYaConfirmado);
            }

            var requerido = new Dictionary<int, decimal>();
            foreach (var detalle in pedido.Detalles)
            {
                var receta = Db.Recetas.Where(r => r.IdProductoMenu == detalle.IdProductoMenu).ToList();
                foreach (var linea in receta)
                {
                    var cantidad = linea.CantidadUtilizada * detalle.Cantidad;
                    if (requerido.ContainsKey(linea.IdProducto))
                    {
                        requerido[linea.IdProducto] = requerido[linea.IdProducto] + cantidad;
                    }
                    else
                    {
                        requerido[linea.IdProducto] = cantidad;
                    }
                }
            }

            using (var transaccion = Db.Database.BeginTransaction())
            {
                try
                {
                    foreach (var item in requerido)
                    {
                        var insumo = Db.Inventario.First(p => p.IdProducto == item.Key);
                        if (insumo.Stock < item.Value)
                        {
                            transaccion.Rollback();
                            return ResultadoOperacion.Error(Mensajes.StockInsuficiente +
                                " Falta " + insumo.NombreProducto + " (disponible " + insumo.Stock +
                                ", requerido " + item.Value + ").");
                        }

                        insumo.Stock = insumo.Stock - item.Value;
                        _inventario.RegistrarMovimiento(insumo, Estados.Movimiento.Salida, item.Value,
                            "Consumo de receta del pedido " + pedido.IdPedido, pedido.IdPedido);
                    }

                    pedido.EstadoPedido = Estados.Pedido.Confirmado;

                    _bitacora.Registrar(Estados.Modulos.Restaurante, "CONFIRMAR", pedido.IdPedido,
                        "Pedido confirmado y stock descontado");

                    Db.SaveChanges();
                    transaccion.Commit();
                }
                catch (Exception)
                {
                    transaccion.Rollback();
                    return ResultadoOperacion.Error(Mensajes.ErrorGeneral);
                }
            }

            return ResultadoOperacion.Ok("Pedido confirmado. El stock fue descontado.", pedido.IdPedido);
        }

        public ResultadoOperacion Entregar(int idPedido)
        {
            var pedido = Obtener(idPedido);
            if (pedido == null)
            {
                return ResultadoOperacion.Error(Mensajes.NoEncontrado);
            }

            if (pedido.EstadoPedido != Estados.Pedido.Confirmado)
            {
                return ResultadoOperacion.Error("Solo se puede entregar un pedido confirmado.");
            }

            pedido.EstadoPedido = Estados.Pedido.Entregado;

            _bitacora.Registrar(Estados.Modulos.Restaurante, "ENTREGAR", pedido.IdPedido, "Pedido entregado");
            Db.SaveChanges();

            return ResultadoOperacion.Ok("Pedido marcado como entregado.", pedido.IdPedido);
        }

        public ResultadoOperacion Cancelar(int idPedido)
        {
            var pedido = Obtener(idPedido);
            if (pedido == null)
            {
                return ResultadoOperacion.Error(Mensajes.NoEncontrado);
            }

            if (pedido.EstadoPedido == Estados.Pedido.Cancelado)
            {
                return ResultadoOperacion.Error("El pedido ya estaba cancelado.");
            }

            if (Db.DetallesFactura.Any(d => d.IdPedido == idPedido))
            {
                return ResultadoOperacion.Error("El pedido ya fue facturado y no se puede cancelar.");
            }

            if (pedido.EstadoPedido != Estados.Pedido.Pendiente)
            {
                var salidas = Db.MovimientosInventario
                    .Where(m => m.IdPedido == idPedido && m.TipoMovimiento == Estados.Movimiento.Salida)
                    .ToList();

                foreach (var salida in salidas)
                {
                    var insumo = Db.Inventario.First(p => p.IdProducto == salida.IdProducto);
                    insumo.Stock = insumo.Stock + salida.Cantidad;
                    _inventario.RegistrarMovimiento(insumo, Estados.Movimiento.Entrada, salida.Cantidad,
                        "Reverso por cancelacion del pedido " + idPedido, idPedido);
                }
            }

            pedido.EstadoPedido = Estados.Pedido.Cancelado;

            if (pedido.IdMesa.HasValue && !Db.Pedidos.Any(p => p.IdMesa == pedido.IdMesa
                                                            && p.IdPedido != pedido.IdPedido
                                                            && p.EstadoPedido != Estados.Pedido.Cancelado
                                                            && p.EstadoPedido != Estados.Pedido.Entregado))
            {
                var mesa = Db.Mesas.First(m => m.IdMesa == pedido.IdMesa.Value);
                mesa.EstadoMesa = Estados.Mesa.Disponible;
            }

            _bitacora.Registrar(Estados.Modulos.Restaurante, Estados.Acciones.Anular, pedido.IdPedido,
                "Pedido cancelado");
            Db.SaveChanges();

            return ResultadoOperacion.Ok("El pedido fue cancelado.", pedido.IdPedido);
        }
    }
}
