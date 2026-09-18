using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using Prototipo.Common;
using Prototipo.Data;
using Prototipo.Models;

namespace Prototipo.Services
{
    public class MenuService : ServicioBase
    {
        public MenuService(HotelColibriContext db) : base(db)
        {
        }

        public List<ProductoMenu> Listar(string estado, string categoria)
        {
            var consulta = Db.Menu.AsQueryable();

            if (!string.IsNullOrEmpty(estado))
            {
                consulta = consulta.Where(p => p.EstadoProductoMenu == estado);
            }

            if (!string.IsNullOrEmpty(categoria))
            {
                consulta = consulta.Where(p => p.CategoriaMenu == categoria);
            }

            return consulta.OrderBy(p => p.CategoriaMenu).ThenBy(p => p.NombreProducto).ToList();
        }

        public List<ProductoMenu> ListarDisponibles()
        {
            return Db.Menu
                .Where(p => p.EstadoProductoMenu == Estados.ProductoMenu.Disponible)
                .OrderBy(p => p.CategoriaMenu)
                .ThenBy(p => p.NombreProducto)
                .ToList();
        }

        public ProductoMenu Obtener(int id)
        {
            return Db.Menu.FirstOrDefault(p => p.IdProductoMenu == id);
        }

        public List<RecetaProducto> ObtenerReceta(int idProductoMenu)
        {
            return Db.Recetas
                .Include(r => r.Insumo)
                .Where(r => r.IdProductoMenu == idProductoMenu)
                .ToList();
        }
    }
}
