using SistemaHotelColibri.Abstract.Comun;
using SistemaHotelColibri.Abstract.Interfaces.DataAccess.Inventarios;
using SistemaHotelColibri.DataAccess.Contexto;

namespace SistemaHotelColibri.DataAccess.Inventarios;

public class EliminarInventarioDA : IEliminarInventarioDA
{
    private readonly HotelColibriContext _contexto;

    public EliminarInventarioDA(HotelColibriContext contexto)
    {
        _contexto = contexto;
    }

    public async Task<bool> EliminarAsync(int idProducto)
    {
        var entidad = await _contexto.Inventario.FindAsync(idProducto);
        if (entidad == null)
        {
            return false;
        }

        entidad.EstadoProducto = Estados.ProductoInventario.Inactivo;
        await _contexto.SaveChangesAsync();
        return true;
    }
}
