using Microsoft.EntityFrameworkCore;
using SistemaHotelColibri.Abstract.Interfaces.DataAccess.Inventarios;
using SistemaHotelColibri.Abstract.Modelos.Inventarios;
using SistemaHotelColibri.DataAccess.Contexto;

namespace SistemaHotelColibri.DataAccess.Inventarios;

public class ObtenerInventarioDA : IObtenerInventarioDA
{
    private readonly HotelColibriContext _contexto;

    public ObtenerInventarioDA(HotelColibriContext contexto)
    {
        _contexto = contexto;
    }

    public async Task<List<InventarioDto>> ObtenerAsync(string? estado)
    {
        var consulta = _contexto.Inventario.AsNoTracking();

        if (!string.IsNullOrWhiteSpace(estado))
        {
            consulta = consulta.Where(p => p.EstadoProducto == estado);
        }

        var productos = await consulta.OrderBy(p => p.NombreProducto).ToListAsync();
        return productos.Select(p => p.ADto()).ToList();
    }
}
