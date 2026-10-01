using SistemaHotelColibri.Abstract.Interfaces.BusinessLogic.Clientes;
using SistemaHotelColibri.Abstract.Interfaces.DataAccess.Clientes;
using SistemaHotelColibri.Abstract.Modelos.Clientes;

namespace SistemaHotelColibri.BusinessLogic.Clientes;

public class ObtenerClienteBL : IObtenerClienteBL
{
    private readonly IObtenerClienteDA _obtenerDA;

    public ObtenerClienteBL(IObtenerClienteDA obtenerDA)
    {
        _obtenerDA = obtenerDA;
    }

    public Task<List<ClienteDto>> ObtenerAsync(string? estado)
    {
        return _obtenerDA.ObtenerAsync(estado);
    }
}