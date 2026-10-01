using Microsoft.EntityFrameworkCore;
using SistemaHotelColibri.Abstract.Interfaces.DataAccess.Clientes;
using SistemaHotelColibri.Abstract.Modelos.Clientes;
using SistemaHotelColibri.DataAccess.Contexto;

namespace SistemaHotelColibri.DataAccess.Clientes;

public class ObtenerClientePorIdDA : IObtenerClientePorIdDA
{
    private readonly HotelColibriContext _contexto;

    public ObtenerClientePorIdDA(HotelColibriContext contexto)
    {
        _contexto = contexto;
    }

    public async Task<ClienteDto?> ObtenerPorIdAsync(int idCliente)
    {
        var cliente = await _contexto.Cliente
            .AsNoTracking()
            .FirstOrDefaultAsync(c => c.IdCliente == idCliente);

        return cliente?.ADto();
    }
}