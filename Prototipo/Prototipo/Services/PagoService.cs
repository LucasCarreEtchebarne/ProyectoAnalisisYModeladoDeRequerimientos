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
    public class PagoService : ServicioBase
    {
        private readonly BitacoraService _bitacora;
        private readonly FacturaService _facturas;

        public PagoService(HotelColibriContext db) : base(db)
        {
            _bitacora = new BitacoraService(db);
            _facturas = new FacturaService(db);
        }

        public List<Pago> Listar(string metodo, string estado, DateTime? desde, DateTime? hasta)
        {
            var consulta = Db.Pagos
                .Include(p => p.Factura)
                .Include(p => p.Factura.Cliente)
                .Include(p => p.Usuario)
                .AsQueryable();

            if (!string.IsNullOrEmpty(metodo))
            {
                consulta = consulta.Where(p => p.MetodoPago == metodo);
            }

            if (!string.IsNullOrEmpty(estado))
            {
                consulta = consulta.Where(p => p.EstadoPago == estado);
            }

            if (desde.HasValue)
            {
                consulta = consulta.Where(p => p.FechaHoraPago >= desde.Value);
            }

            if (hasta.HasValue)
            {
                var limite = hasta.Value.Date.AddDays(1);
                consulta = consulta.Where(p => p.FechaHoraPago < limite);
            }

            return consulta.OrderByDescending(p => p.FechaHoraPago).ToList();
        }

        public Pago Obtener(int id)
        {
            return Db.Pagos
                .Include(p => p.Factura)
                .Include(p => p.Factura.Cliente)
                .Include(p => p.Usuario)
                .FirstOrDefault(p => p.IdPago == id);
        }

        public ResultadoOperacion Registrar(Pago pago)
        {
            if (pago.MontoPagado <= 0)
            {
                return ResultadoOperacion.Error("El monto pagado debe ser mayor que cero.");
            }

            var factura = _facturas.Obtener(pago.IdFactura);
            if (factura == null)
            {
                return ResultadoOperacion.Error(Mensajes.NoEncontrado);
            }

            if (factura.EstadoFactura == Estados.Factura.Anulada)
            {
                return ResultadoOperacion.Error(Mensajes.FacturaAnulada);
            }

            if (factura.EstadoFactura == Estados.Factura.Pagada)
            {
                return ResultadoOperacion.Error(Mensajes.FacturaPagada);
            }

            var saldo = _facturas.SaldoPendiente(factura);
            if (pago.MontoPagado > saldo)
            {
                return ResultadoOperacion.Error(Mensajes.PagoExcedeSaldo +
                    " Saldo pendiente: " + Formato.Colones(saldo) + ".");
            }

            if (pago.MetodoPago == Estados.MetodoPago.Efectivo)
            {
                if (!pago.MontoRecibido.HasValue || pago.MontoRecibido.Value < pago.MontoPagado)
                {
                    return ResultadoOperacion.Error(Mensajes.EfectivoInsuficiente);
                }

                pago.CambioDevuelto = pago.MontoRecibido.Value - pago.MontoPagado;
                pago.TipoTarjeta = null;
                pago.UltimosCuatroDigitos = null;
            }
            else if (pago.MetodoPago == Estados.MetodoPago.Tarjeta)
            {
                if (string.IsNullOrWhiteSpace(pago.TipoTarjeta)
                    || string.IsNullOrWhiteSpace(pago.UltimosCuatroDigitos)
                    || pago.UltimosCuatroDigitos.Length != 4
                    || !pago.UltimosCuatroDigitos.All(char.IsDigit))
                {
                    return ResultadoOperacion.Error(Mensajes.TarjetaDatosRequeridos);
                }

                pago.MontoRecibido = null;
                pago.CambioDevuelto = null;
            }
            else
            {
                if (string.IsNullOrWhiteSpace(pago.NumeroReferencia))
                {
                    return ResultadoOperacion.Error(Mensajes.ReferenciaRequerida);
                }

                pago.MontoRecibido = null;
                pago.CambioDevuelto = null;
                pago.TipoTarjeta = null;
                pago.UltimosCuatroDigitos = null;
            }

            pago.IdUsuario = SesionActual.IdUsuario;
            pago.FechaHoraPago = DateTime.Now;
            if (string.IsNullOrEmpty(pago.EstadoPago))
            {
                pago.EstadoPago = Estados.Pago.Aprobado;
            }

            Db.Pagos.Add(pago);
            Db.SaveChanges();

            _facturas.ActualizarEstadoPorPagos(factura);

            _bitacora.Registrar(Estados.Modulos.Facturacion, Estados.Acciones.Crear, pago.IdPago,
                "Pago de " + Formato.Colones(pago.MontoPagado) + " (" + pago.MetodoPago +
                ") sobre la factura " + factura.NumeroFactura);
            Db.SaveChanges();

            var mensaje = "Pago registrado por " + Formato.Colones(pago.MontoPagado) + ".";
            if (pago.CambioDevuelto.HasValue && pago.CambioDevuelto.Value > 0)
            {
                mensaje += " Cambio a devolver: " + Formato.Colones(pago.CambioDevuelto.Value) + ".";
            }

            if (factura.EstadoFactura == Estados.Factura.Pagada)
            {
                mensaje += " La factura quedo pagada.";
            }

            return ResultadoOperacion.Ok(mensaje, pago.IdPago);
        }

        public ResultadoOperacion Reversar(int idPago, string motivo)
        {
            var pago = Obtener(idPago);
            if (pago == null)
            {
                return ResultadoOperacion.Error(Mensajes.NoEncontrado);
            }

            if (pago.EstadoPago != Estados.Pago.Aprobado)
            {
                return ResultadoOperacion.Error("Solo se puede reversar un pago aprobado.");
            }

            pago.EstadoPago = Estados.Pago.Reversado;

            var factura = _facturas.Obtener(pago.IdFactura);
            _facturas.ActualizarEstadoPorPagos(factura);

            _bitacora.Registrar(Estados.Modulos.Facturacion, Estados.Acciones.Anular, pago.IdPago,
                "Pago reversado: " + (motivo ?? string.Empty));
            Db.SaveChanges();

            return ResultadoOperacion.Ok("El pago fue reversado.", pago.IdPago);
        }
    }
}
