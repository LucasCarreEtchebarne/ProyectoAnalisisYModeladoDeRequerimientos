using System;
using System.Collections.Generic;
using System.Linq;
using Prototipo.Common;
using Prototipo.Data;
using Prototipo.Infrastructure;
using Prototipo.Models;

namespace Prototipo.Services
{
    public class InventarioService : ServicioBase
    {
        private readonly BitacoraService _bitacora;

        public InventarioService(HotelColibriContext db) : base(db)
        {
            _bitacora = new BitacoraService(db);
        }

        public List<Inventario> Listar(string estado, string categoria, string busqueda)
        {
            var consulta = Db.Inventario.AsQueryable();

            if (!string.IsNullOrEmpty(estado))
            {
                consulta = consulta.Where(p => p.EstadoProducto == estado);
            }

            if (!string.IsNullOrEmpty(categoria))
            {
                consulta = consulta.Where(p => p.CategoriaProducto == categoria);
            }

            if (!string.IsNullOrWhiteSpace(busqueda))
            {
                consulta = consulta.Where(p => p.NombreProducto.Contains(busqueda));
            }

            return consulta.OrderBy(p => p.NombreProducto).ToList();
        }

        public List<Inventario> ListarAlertas()
        {
            return Db.Inventario
                .Where(p => p.EstadoProducto == Estados.Producto.Activo && p.Stock <= p.StockMinimo)
                .OrderBy(p => p.NombreProducto)
                .ToList();
        }

        public List<string> ListarCategorias()
        {
            return Db.Inventario.Select(p => p.CategoriaProducto).Distinct().OrderBy(c => c).ToList();
        }

        public Inventario Obtener(int id)
        {
            return Db.Inventario.FirstOrDefault(p => p.IdProducto == id);
        }

        public List<MovimientoInventario> ListarMovimientos(int idProducto)
        {
            return Db.MovimientosInventario
                .Where(m => m.IdProducto == idProducto)
                .OrderByDescending(m => m.FechaHora)
                .Take(100)
                .ToList();
        }

        public ResultadoOperacion Crear(Inventario producto)
        {
            if (Db.Inventario.Any(p => p.NombreProducto == producto.NombreProducto))
            {
                return ResultadoOperacion.Error(Mensajes.ProductoDuplicado);
            }

            producto.FechaIngreso = DateTime.Today;
            if (string.IsNullOrEmpty(producto.EstadoProducto))
            {
                producto.EstadoProducto = Estados.Producto.Activo;
            }

            var stockInicial = producto.Stock;
            Db.Inventario.Add(producto);
            Db.SaveChanges();

            if (stockInicial > 0)
            {
                RegistrarMovimiento(producto, Estados.Movimiento.Entrada, stockInicial, "Stock inicial", null);
            }

            _bitacora.Registrar(Estados.Modulos.Inventario, Estados.Acciones.Crear, producto.IdProducto,
                "Producto creado: " + producto.NombreProducto);
            Db.SaveChanges();

            return ResultadoOperacion.Ok(Mensajes.CreadoOk, producto.IdProducto);
        }

        public ResultadoOperacion Actualizar(Inventario datos)
        {
            var producto = Obtener(datos.IdProducto);
            if (producto == null)
            {
                return ResultadoOperacion.Error(Mensajes.NoEncontrado);
            }

            if (Db.Inventario.Any(p => p.IdProducto != datos.IdProducto && p.NombreProducto == datos.NombreProducto))
            {
                return ResultadoOperacion.Error(Mensajes.ProductoDuplicado);
            }

            if (datos.Stock != producto.Stock)
            {
                var diferencia = datos.Stock - producto.Stock;
                var tipo = diferencia > 0 ? Estados.Movimiento.Entrada : Estados.Movimiento.Ajuste;
                producto.Stock = datos.Stock;
                RegistrarMovimiento(producto, tipo, Math.Abs(diferencia), "Ajuste manual desde inventario", null);
            }

            producto.NombreProducto = datos.NombreProducto;
            producto.CategoriaProducto = datos.CategoriaProducto;
            producto.UnidadMedida = datos.UnidadMedida;
            producto.StockMinimo = datos.StockMinimo;
            producto.EstadoProducto = datos.EstadoProducto;
            producto.Descripcion = datos.Descripcion;

            _bitacora.Registrar(Estados.Modulos.Inventario, Estados.Acciones.Editar, producto.IdProducto,
                "Producto actualizado: " + producto.NombreProducto);
            Db.SaveChanges();

            return ResultadoOperacion.Ok(Mensajes.ActualizadoOk, producto.IdProducto);
        }

        public ResultadoOperacion RegistrarEntrada(int idProducto, decimal cantidad, string motivo)
        {
            if (cantidad <= 0)
            {
                return ResultadoOperacion.Error("La cantidad debe ser mayor que cero.");
            }

            var producto = Obtener(idProducto);
            if (producto == null)
            {
                return ResultadoOperacion.Error(Mensajes.NoEncontrado);
            }

            producto.Stock = producto.Stock + cantidad;
            RegistrarMovimiento(producto, Estados.Movimiento.Entrada, cantidad,
                string.IsNullOrWhiteSpace(motivo) ? "Entrada de mercaderia" : motivo, null);

            _bitacora.Registrar(Estados.Modulos.Inventario, Estados.Acciones.Editar, producto.IdProducto,
                "Entrada de " + cantidad + " a " + producto.NombreProducto);
            Db.SaveChanges();

            return ResultadoOperacion.Ok("Entrada registrada. Stock actual: " + producto.Stock, producto.IdProducto);
        }

        public ResultadoOperacion Inactivar(int id)
        {
            var producto = Obtener(id);
            if (producto == null)
            {
                return ResultadoOperacion.Error(Mensajes.NoEncontrado);
            }

            producto.EstadoProducto = Estados.Producto.Inactivo;

            _bitacora.Registrar(Estados.Modulos.Inventario, Estados.Acciones.Inactivar, producto.IdProducto,
                "Producto inactivado: " + producto.NombreProducto);
            Db.SaveChanges();

            return ResultadoOperacion.Ok(Mensajes.InactivadoOk, producto.IdProducto);
        }

        internal void RegistrarMovimiento(Inventario producto, string tipo, decimal cantidad, string motivo, int? idPedido)
        {
            Db.MovimientosInventario.Add(new MovimientoInventario
            {
                IdProducto = producto.IdProducto,
                IdUsuario = SesionActual.IdUsuario,
                IdPedido = idPedido,
                TipoMovimiento = tipo,
                Cantidad = cantidad,
                StockResultante = producto.Stock,
                FechaHora = DateTime.Now,
                Motivo = motivo
            });
        }
    }
}
