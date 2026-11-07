using SistemaHotelColibri.Abstract.Interfaces.DataAccess.Menus;
using SistemaHotelColibri.Abstract.Modelos.Menus;
using SistemaHotelColibri.DataAccess.Contexto;

namespace SistemaHotelColibri.DataAccess.Menus;

public class AgregarMenuDA : IAgregarMenuDA
{
    private readonly HotelColibriContext _contexto;

    public AgregarMenuDA(HotelColibriContext contexto)
    {
        _contexto = contexto;
    }

    public async Task<int> AgregarAsync(MenuDto producto)
    {
        var entidad = producto.AEntidad();
        _contexto.Menu.Add(entidad);
        await _contexto.SaveChangesAsync();
        return entidad.IdProductoMenu;
    }
}
