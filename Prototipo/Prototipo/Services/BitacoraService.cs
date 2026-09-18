using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using Prototipo.Data;
using Prototipo.Infrastructure;
using Prototipo.Models;

namespace Prototipo.Services
{
    public class BitacoraService : ServicioBase
    {
        public BitacoraService(HotelColibriContext db) : base(db)
        {
        }

        public void Registrar(string modulo, string accion, int? idRegistro, string descripcion, int? idUsuario = null)
        {
            var usuario = idUsuario ?? SesionActual.IdUsuario;
            if (usuario <= 0)
            {
                return;
            }

            Db.Bitacoras.Add(new Bitacora
            {
                IdUsuario = usuario,
                Modulo = modulo,
                AccionRealizada = accion,
                IdRegistro = idRegistro,
                FechaHora = DateTime.Now,
                Descripcion = descripcion
            });
        }

        public List<Bitacora> Listar(string modulo, DateTime? desde, DateTime? hasta, int? idUsuario)
        {
            var consulta = Db.Bitacoras.Include(b => b.Usuario).AsQueryable();

            if (!string.IsNullOrEmpty(modulo))
            {
                consulta = consulta.Where(b => b.Modulo == modulo);
            }

            if (desde.HasValue)
            {
                consulta = consulta.Where(b => b.FechaHora >= desde.Value);
            }

            if (hasta.HasValue)
            {
                var limite = hasta.Value.Date.AddDays(1);
                consulta = consulta.Where(b => b.FechaHora < limite);
            }

            if (idUsuario.HasValue && idUsuario.Value > 0)
            {
                consulta = consulta.Where(b => b.IdUsuario == idUsuario.Value);
            }

            return consulta.OrderByDescending(b => b.FechaHora).Take(500).ToList();
        }

        public Bitacora Obtener(long id)
        {
            return Db.Bitacoras.Include(b => b.Usuario).FirstOrDefault(b => b.IdBitacora == id);
        }
    }
}
