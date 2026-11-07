using Microsoft.EntityFrameworkCore;
using SistemaHotelColibri.Abstract.Interfaces.DataAccess.Mesas;
using SistemaHotelColibri.DataAccess.Contexto;

namespace SistemaHotelColibri.DataAccess.Mesas;

public class ExisteNumeroMesaDA : IExisteNumeroMesaDA
{
    private readonly HotelColibriContext _contexto;

    public ExisteNumeroMesaDA(HotelColibriContext contexto)
    {
        _contexto = contexto;
    }

    public async Task<bool> ExisteNumeroAsync(int numeroMesa, int idExcluir)
    {
        return await _contexto.Mesa
            .AnyAsync(m => m.NumeroMesa == numeroMesa && m.IdMesa != idExcluir);
    }
}
