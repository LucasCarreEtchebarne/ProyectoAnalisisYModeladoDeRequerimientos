using Microsoft.EntityFrameworkCore;
using SistemaHotelColibri.Abstract.Interfaces.DataAccess.Inventarios;
using SistemaHotelColibri.Abstract.Modelos.Inventarios;
using SistemaHotelColibri.DataAccess.Contexto;

namespace SistemaHotelColibri.DataAccess.Inventarios;

public class ObtenerInventarioPorIdDA : IObtenerInventarioPorIdDA
{
    private readonly HotelColibriContext _contexto;

    public ObtenerInventarioPorIdDA(HotelColibriContext contexto)
    {
        _contexto = contexto;
    }

    public async Task<InventarioDto?> ObtenerPorIdAsync(int idProducto)
    {
        var producto = await _contexto.Inventario
            .AsNoTracking()
            .FirstOrDefaultAsync(p => p.IdProducto == idProducto);

        return producto?.ADto();
    }
}
