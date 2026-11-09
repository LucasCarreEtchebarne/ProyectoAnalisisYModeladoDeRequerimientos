using SistemaHotelColibri.Abstract.Interfaces.BusinessLogic.Clientes;
using SistemaHotelColibri.Abstract.Interfaces.DataAccess.Clientes;
using SistemaHotelColibri.Abstract.Modelos.Clientes;

namespace SistemaHotelColibri.BusinessLogic.Clientes;

public class ObtenerHistorialClienteBL : IObtenerHistorialClienteBL
{
    private readonly IObtenerHistorialClienteDA _historialDA;
    private readonly IObtenerClientePorIdDA _obtenerClienteDA;

    public ObtenerHistorialClienteBL(
        IObtenerHistorialClienteDA historialDA,
        IObtenerClientePorIdDA obtenerClienteDA)
    {
        _historialDA = historialDA;
        _obtenerClienteDA = obtenerClienteDA;
    }

    public async Task<HistorialClienteDto?> ObtenerAsync(int idCliente)
    {
        var cliente = await _obtenerClienteDA.ObtenerPorIdAsync(idCliente);
        if (cliente == null)
        {
            return null;
        }

        var historial = await _historialDA.ObtenerAsync(idCliente);
        historial.Cliente = cliente;
        return historial;
    }
}