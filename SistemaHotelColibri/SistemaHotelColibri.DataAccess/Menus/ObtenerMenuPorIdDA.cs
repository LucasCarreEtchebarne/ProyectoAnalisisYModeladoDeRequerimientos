using Microsoft.EntityFrameworkCore;
using SistemaHotelColibri.Abstract.Interfaces.DataAccess.Menus;
using SistemaHotelColibri.Abstract.Modelos.Menus;
using SistemaHotelColibri.DataAccess.Contexto;

namespace SistemaHotelColibri.DataAccess.Menus;

public class ObtenerMenuPorIdDA : IObtenerMenuPorIdDA
{
    private readonly HotelColibriContext _contexto;

    public ObtenerMenuPorIdDA(HotelColibriContext contexto)
    {
        _contexto = contexto;
    }

    public async Task<MenuDto?> ObtenerPorIdAsync(int idProductoMenu)
    {
        var producto = await _contexto.Menu
            .AsNoTracking()
            .FirstOrDefaultAsync(p => p.IdProductoMenu == idProductoMenu);

        return producto?.ADto();
    }
}
