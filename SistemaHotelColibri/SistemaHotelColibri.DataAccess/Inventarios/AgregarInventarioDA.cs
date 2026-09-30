using SistemaHotelColibri.Abstract.Interfaces.DataAccess.Inventarios;
using SistemaHotelColibri.Abstract.Modelos.Inventarios;
using SistemaHotelColibri.DataAccess.Contexto;

namespace SistemaHotelColibri.DataAccess.Inventarios;

public class AgregarInventarioDA : IAgregarInventarioDA
{
    private readonly HotelColibriContext _contexto;

    public AgregarInventarioDA(HotelColibriContext contexto)
    {
        _contexto = contexto;
    }

    public async Task<int> AgregarAsync(InventarioDto producto)
    {
        var entidad = producto.AEntidad();
        _contexto.Inventario.Add(entidad);
        await _contexto.SaveChangesAsync();
        return entidad.IdProducto;
    }
}
