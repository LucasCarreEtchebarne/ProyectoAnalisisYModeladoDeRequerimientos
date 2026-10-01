using SistemaHotelColibri.Abstract.Comun;
using SistemaHotelColibri.Abstract.Interfaces.DataAccess.Clientes;
using SistemaHotelColibri.DataAccess.Contexto;

namespace SistemaHotelColibri.DataAccess.Clientes;

public class EliminarClienteDA : IEliminarClienteDA
{
    private readonly HotelColibriContext _contexto;

    public EliminarClienteDA(HotelColibriContext contexto)
    {
        _contexto = contexto;
    }

    public async Task<bool> EliminarAsync(int idCliente)
    {
        var entidad = await _contexto.Cliente.FindAsync(idCliente);
        if (entidad == null)
        {
            return false;
        }

        entidad.EstadoCliente = Estados.Cliente.Inactivo;
        await _contexto.SaveChangesAsync();
        return true;
    }
}