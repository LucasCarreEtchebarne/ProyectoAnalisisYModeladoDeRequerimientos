using Microsoft.EntityFrameworkCore;
using SistemaHotelColibri.Abstract.Interfaces.DataAccess.Clientes;
using SistemaHotelColibri.DataAccess.Contexto;

namespace SistemaHotelColibri.DataAccess.Clientes;

public class ExisteCorreoClienteDA : IExisteCorreoClienteDA
{
    private readonly HotelColibriContext _contexto;

    public ExisteCorreoClienteDA(HotelColibriContext contexto)
    {
        _contexto = contexto;
    }

    public async Task<bool> ExisteCorreoAsync(string correoElectronico, int idExcluir)
    {
        return await _contexto.Cliente
            .AnyAsync(c => c.CorreoElectronico == correoElectronico && c.IdCliente != idExcluir);
    }
}
