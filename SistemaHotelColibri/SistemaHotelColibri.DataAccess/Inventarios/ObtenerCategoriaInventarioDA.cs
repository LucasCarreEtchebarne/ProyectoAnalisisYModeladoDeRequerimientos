using Microsoft.EntityFrameworkCore;
using SistemaHotelColibri.Abstract.Interfaces.DataAccess.Inventarios;
using SistemaHotelColibri.Abstract.Modelos.Inventarios;
using SistemaHotelColibri.DataAccess.Contexto;

namespace SistemaHotelColibri.DataAccess.Inventarios;

public class ObtenerCategoriaInventarioDA : IObtenerCategoriaInventarioDA
{
    private readonly HotelColibriContext _contexto;

    public ObtenerCategoriaInventarioDA(HotelColibriContext contexto)
    {
        _contexto = contexto;
    }

    public async Task<List<CategoriaInventarioDto>> ObtenerAsync(string? estado)
    {
        var consulta = _contexto.CategoriaInventario.AsNoTracking();

        if (!string.IsNullOrWhiteSpace(estado))
        {
            consulta = consulta.Where(c => c.EstadoCategoria == estado);
        }

        var categorias = await consulta.OrderBy(c => c.Area).ThenBy(c => c.NombreCategoria).ToListAsync();
        return categorias.Select(c => c.ADto()).ToList();
    }
}
