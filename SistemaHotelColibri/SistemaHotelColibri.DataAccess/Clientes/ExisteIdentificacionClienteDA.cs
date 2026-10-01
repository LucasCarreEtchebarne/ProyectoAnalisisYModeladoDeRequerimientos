using Microsoft.EntityFrameworkCore;
using SistemaHotelColibri.Abstract.Interfaces.DataAccess.Clientes;
using SistemaHotelColibri.DataAccess.Contexto;

namespace SistemaHotelColibri.DataAccess.Clientes;

public class ExisteIdentificacionClienteDA : IExisteIdentificacionClienteDA
{
    private readonly HotelColibriContext _contexto;

    public ExisteIdentificacionClienteDA(HotelColibriContext contexto)
    {
        _contexto = contexto;
    }

    public async Task<bool> ExisteIdentificacionAsync(string identificacion, int idExcluir)
    {
        return await _contexto.Cliente
            .AnyAsync(c => c.Identificacion == identificacion && c.IdCliente != idExcluir);
    }
}