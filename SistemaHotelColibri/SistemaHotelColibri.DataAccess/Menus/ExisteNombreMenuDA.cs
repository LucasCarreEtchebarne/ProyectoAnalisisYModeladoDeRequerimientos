using Microsoft.EntityFrameworkCore;
using SistemaHotelColibri.Abstract.Interfaces.DataAccess.Menus;
using SistemaHotelColibri.DataAccess.Contexto;

namespace SistemaHotelColibri.DataAccess.Menus;

public class ExisteNombreMenuDA : IExisteNombreMenuDA
{
    private readonly HotelColibriContext _contexto;

    public ExisteNombreMenuDA(HotelColibriContext contexto)
    {
        _contexto = contexto;
    }

    public async Task<bool> ExisteNombreAsync(string nombreProducto, int idExcluir)
    {
        return await _contexto.Menu
            .AnyAsync(p => p.NombreProducto == nombreProducto && p.IdProductoMenu != idExcluir);
    }
}
