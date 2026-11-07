using SistemaHotelColibri.Abstract.Comun;
using SistemaHotelColibri.Abstract.Interfaces.DataAccess.Mesas;
using SistemaHotelColibri.DataAccess.Contexto;

namespace SistemaHotelColibri.DataAccess.Mesas;

public class EliminarMesaDA : IEliminarMesaDA
{
    private readonly HotelColibriContext _contexto;

    public EliminarMesaDA(HotelColibriContext contexto)
    {
        _contexto = contexto;
    }

    public async Task<bool> EliminarAsync(int idMesa)
    {
        var entidad = await _contexto.Mesa.FindAsync(idMesa);
        if (entidad == null)
        {
            return false;
        }

        entidad.EstadoMesa = Estados.Mesa.Inactiva;
        await _contexto.SaveChangesAsync();
        return true;
    }
}
