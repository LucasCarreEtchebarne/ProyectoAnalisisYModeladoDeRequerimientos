using System.Collections.Generic;
using System.Linq;
using Prototipo.Common;
using Prototipo.Data;
using Prototipo.Models;

namespace Prototipo.Services
{
    public class MesaService : ServicioBase
    {
        private readonly BitacoraService _bitacora;

        public MesaService(HotelColibriContext db) : base(db)
        {
            _bitacora = new BitacoraService(db);
        }

        public List<Mesa> Listar(string estado)
        {
            var consulta = Db.Mesas.AsQueryable();

            if (!string.IsNullOrEmpty(estado))
            {
                consulta = consulta.Where(m => m.EstadoMesa == estado);
            }

            return consulta.OrderBy(m => m.NumeroMesa).ToList();
        }

        public Mesa Obtener(int id)
        {
            return Db.Mesas.FirstOrDefault(m => m.IdMesa == id);
        }

        public List<Pedido> PedidosActivos(int idMesa)
        {
            return Db.Pedidos
                .Where(p => p.IdMesa == idMesa && p.EstadoPedido != Estados.Pedido.Cancelado)
                .OrderByDescending(p => p.FechaHoraPedido)
                .Take(20)
                .ToList();
        }

        public ResultadoOperacion CambiarEstado(int idMesa, string estado)
        {
            var mesa = Obtener(idMesa);
            if (mesa == null)
            {
                return ResultadoOperacion.Error(Mensajes.NoEncontrado);
            }

            if (!Estados.Mesa.Todos.Contains(estado))
            {
                return ResultadoOperacion.Error("El estado indicado no es valido.");
            }

            mesa.EstadoMesa = estado;

            _bitacora.Registrar(Estados.Modulos.Restaurante, Estados.Acciones.Editar, mesa.IdMesa,
                "Mesa " + mesa.NumeroMesa + " paso a " + estado);
            Db.SaveChanges();

            return ResultadoOperacion.Ok("La mesa quedo en estado " + estado + ".", mesa.IdMesa);
        }
    }
}
