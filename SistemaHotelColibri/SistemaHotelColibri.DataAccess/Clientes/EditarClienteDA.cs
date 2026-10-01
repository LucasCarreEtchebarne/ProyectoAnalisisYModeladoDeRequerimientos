using SistemaHotelColibri.Abstract.Interfaces.DataAccess.Clientes;
using SistemaHotelColibri.Abstract.Modelos.Clientes;
using SistemaHotelColibri.DataAccess.Contexto;

namespace SistemaHotelColibri.DataAccess.Clientes;

public class EditarClienteDA : IEditarClienteDA
{
    private readonly HotelColibriContext _contexto;

    public EditarClienteDA(HotelColibriContext contexto)
    {
        _contexto = contexto;
    }

    public async Task<bool> EditarAsync(ClienteDto cliente)
    {
        var entidad = await _contexto.Cliente.FindAsync(cliente.IdCliente);
        if (entidad == null)
        {
            return false;
        }

        _contexto.Entry(entidad).CurrentValues.SetValues(cliente.AEntidad());
        await _contexto.SaveChangesAsync();
        return true;
    }
}