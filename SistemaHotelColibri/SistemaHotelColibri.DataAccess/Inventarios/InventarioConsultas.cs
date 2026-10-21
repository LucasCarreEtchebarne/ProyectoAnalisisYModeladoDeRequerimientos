using Microsoft.EntityFrameworkCore;
using SistemaHotelColibri.DataAccess.Entidades;

namespace SistemaHotelColibri.DataAccess.Inventarios;

internal static class InventarioConsultas
{
    public static IQueryable<Inventario> ConCategoriaYProveedor(this IQueryable<Inventario> consulta) =>
        consulta
            .Include(p => p.IdCategoriaNavigation)
            .Include(p => p.IdProveedorNavigation);
}
