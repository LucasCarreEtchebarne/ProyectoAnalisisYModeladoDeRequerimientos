using SistemaHotelColibri.Abstract.Interfaces.DataAccess.Clientes;
using SistemaHotelColibri.Abstract.Modelos.Clientes;
using SistemaHotelColibri.DataAccess.Contexto;

namespace SistemaHotelColibri.DataAccess.Clientes;

public class AgregarClienteDA : IAgregarClienteDA
{
    private readonly HotelColibriContext _contexto;

    public AgregarClienteDA(HotelColibriContext contexto)
    {
        _contexto = contexto;
    }

    public async Task<int> AgregarAsync(ClienteDto cliente)
    {
        var entidad = cliente.AEntidad();
        _contexto.Cliente.Add(entidad);
        await _contexto.SaveChangesAsync();
        return entidad.IdCliente;
    }
}