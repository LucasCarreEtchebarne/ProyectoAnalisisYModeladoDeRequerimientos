using SistemaHotelColibri.Abstract.Comun;
using SistemaHotelColibri.Abstract.Interfaces.DataAccess.Menus;
using SistemaHotelColibri.DataAccess.Contexto;

namespace SistemaHotelColibri.DataAccess.Menus;

public class EliminarMenuDA : IEliminarMenuDA
{
    private readonly HotelColibriContext _contexto;

    public EliminarMenuDA(HotelColibriContext contexto)
    {
        _contexto = contexto;
    }

    public async Task<bool> EliminarAsync(int idProductoMenu)
    {
        var entidad = await _contexto.Menu.FindAsync(idProductoMenu);
        if (entidad == null)
        {
            return false;
        }

        entidad.EstadoProductoMenu = Estados.ProductoMenu.Inactivo;
        await _contexto.SaveChangesAsync();
        return true;
    }
}
