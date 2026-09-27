using Microsoft.EntityFrameworkCore;
using SistemaHotelColibri.Abstract.Interfaces.DataAccess.Inventarios;
using SistemaHotelColibri.DataAccess.Contexto;

namespace SistemaHotelColibri.DataAccess.Inventarios;

public class ExisteNombreInventarioDA : IExisteNombreInventarioDA
{
    private readonly HotelColibriContext _contexto;

    public ExisteNombreInventarioDA(HotelColibriContext contexto)
    {
        _contexto = contexto;
    }

    public async Task<bool> ExisteNombreAsync(string nombreProducto, int idExcluir)
    {
        return await _contexto.Inventario
            .AnyAsync(p => p.NombreProducto == nombreProducto && p.IdProducto != idExcluir);
    }
}
