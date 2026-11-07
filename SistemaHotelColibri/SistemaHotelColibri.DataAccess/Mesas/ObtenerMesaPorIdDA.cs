using Microsoft.EntityFrameworkCore;
using SistemaHotelColibri.Abstract.Interfaces.DataAccess.Mesas;
using SistemaHotelColibri.Abstract.Modelos.Mesas;
using SistemaHotelColibri.DataAccess.Contexto;

namespace SistemaHotelColibri.DataAccess.Mesas;

public class ObtenerMesaPorIdDA : IObtenerMesaPorIdDA
{
    private readonly HotelColibriContext _contexto;

    public ObtenerMesaPorIdDA(HotelColibriContext contexto)
    {
        _contexto = contexto;
    }

    public async Task<MesaDto?> ObtenerPorIdAsync(int idMesa)
    {
        var mesa = await _contexto.Mesa
            .AsNoTracking()
            .FirstOrDefaultAsync(m => m.IdMesa == idMesa);

        return mesa?.ADto();
    }
}
