using SistemaHotelColibri.Abstract.Interfaces.DataAccess.Inventarios;
using SistemaHotelColibri.Abstract.Modelos.Inventarios;
using SistemaHotelColibri.DataAccess.Contexto;

namespace SistemaHotelColibri.DataAccess.Inventarios;

public class EditarInventarioDA : IEditarInventarioDA
{
    private readonly HotelColibriContext _contexto;

    public EditarInventarioDA(HotelColibriContext contexto)
    {
        _contexto = contexto;
    }

    public async Task<bool> EditarAsync(InventarioDto producto)
    {
        var entidad = await _contexto.Inventario.FindAsync(producto.IdProducto);
        if (entidad == null)
        {
            return false;
        }

        _contexto.Entry(entidad).CurrentValues.SetValues(producto.AEntidad());
        await _contexto.SaveChangesAsync();
        return true;
    }
}
