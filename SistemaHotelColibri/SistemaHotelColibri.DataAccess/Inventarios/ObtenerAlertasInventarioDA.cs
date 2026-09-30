using Microsoft.EntityFrameworkCore;
using SistemaHotelColibri.Abstract.Comun;
using SistemaHotelColibri.Abstract.Interfaces.DataAccess.Inventarios;
using SistemaHotelColibri.Abstract.Modelos.Inventarios;
using SistemaHotelColibri.DataAccess.Contexto;

namespace SistemaHotelColibri.DataAccess.Inventarios;

public class ObtenerAlertasInventarioDA : IObtenerAlertasInventarioDA
{
    private readonly HotelColibriContext _contexto;

    public ObtenerAlertasInventarioDA(HotelColibriContext contexto)
    {
        _contexto = contexto;
    }

    public async Task<List<InventarioDto>> ObtenerAlertasAsync()
    {
        var productos = await _contexto.Inventario
            .AsNoTracking()
            .Where(p => p.EstadoProducto == Estados.ProductoInventario.Activo && p.Stock <= p.StockMinimo)
            .OrderBy(p => p.Stock)
            .ThenBy(p => p.NombreProducto)
            .ToListAsync();

        return productos.Select(p => p.ADto()).ToList();
    }
}
