using SistemaHotelColibri.Abstract.Interfaces.DataAccess.Menus;
using SistemaHotelColibri.Abstract.Modelos.Menus;
using SistemaHotelColibri.DataAccess.Contexto;

namespace SistemaHotelColibri.DataAccess.Menus;

public class EditarMenuDA : IEditarMenuDA
{
    private readonly HotelColibriContext _contexto;

    public EditarMenuDA(HotelColibriContext contexto)
    {
        _contexto = contexto;
    }

    public async Task<bool> EditarAsync(MenuDto producto)
    {
        var entidad = await _contexto.Menu.FindAsync(producto.IdProductoMenu);
        if (entidad == null)
        {
            return false;
        }

        _contexto.Entry(entidad).CurrentValues.SetValues(producto.AEntidad());
        await _contexto.SaveChangesAsync();
        return true;
    }
}
