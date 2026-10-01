using SistemaHotelColibri.Abstract.Comun;
using SistemaHotelColibri.Abstract.Interfaces.BusinessLogic.Clientes;
using SistemaHotelColibri.Abstract.Interfaces.DataAccess.Clientes;

namespace SistemaHotelColibri.BusinessLogic.Clientes;

public class EliminarClienteBL : IEliminarClienteBL
{
    private readonly IEliminarClienteDA _eliminarDA;
    private readonly IObtenerClientePorIdDA _obtenerPorIdDA;

    public EliminarClienteBL(IEliminarClienteDA eliminarDA, IObtenerClientePorIdDA obtenerPorIdDA)
    {
        _eliminarDA = eliminarDA;
        _obtenerPorIdDA = obtenerPorIdDA;
    }

    public async Task<ResultadoOperacion> EliminarAsync(int idCliente)
    {
        var cliente = await _obtenerPorIdDA.ObtenerPorIdAsync(idCliente);
        if (cliente == null)
        {
            return ResultadoOperacion.Error(Mensajes.NoEncontrado);
        }

        if (cliente.EstadoCliente == Estados.Cliente.Inactivo)
        {
            return ResultadoOperacion.Error(Mensajes.ClienteYaInactivo);
        }

        // Eliminación lógica: el cliente tiene reservas, facturas y pedidos asociados,
        // por lo que no se borra físicamente, solo se marca como Inactivo.
        if (!await _eliminarDA.EliminarAsync(idCliente))
        {
            return ResultadoOperacion.Error(Mensajes.NoEncontrado);
        }

        return ResultadoOperacion.Ok(Mensajes.ClienteEliminado, idCliente);
    }
}