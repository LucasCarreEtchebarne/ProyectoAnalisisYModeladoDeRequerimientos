using SistemaHotelColibri.Abstract.Interfaces.BusinessLogic.Clientes;
using SistemaHotelColibri.Abstract.Interfaces.DataAccess.Clientes;
using SistemaHotelColibri.Abstract.Modelos.Clientes;

namespace SistemaHotelColibri.BusinessLogic.Clientes;

public class ObtenerClientePorIdBL : IObtenerClientePorIdBL
{
    private readonly IObtenerClientePorIdDA _obtenerPorIdDA;

    public ObtenerClientePorIdBL(IObtenerClientePorIdDA obtenerPorIdDA)
    {
        _obtenerPorIdDA = obtenerPorIdDA;
    }

    public Task<ClienteDto?> ObtenerPorIdAsync(int idCliente)
    {
        return _obtenerPorIdDA.ObtenerPorIdAsync(idCliente);
    }
}