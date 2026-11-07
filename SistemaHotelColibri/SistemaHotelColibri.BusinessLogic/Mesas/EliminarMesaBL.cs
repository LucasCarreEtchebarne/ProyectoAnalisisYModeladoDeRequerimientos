using SistemaHotelColibri.Abstract.Comun;
using SistemaHotelColibri.Abstract.Interfaces.BusinessLogic.Mesas;
using SistemaHotelColibri.Abstract.Interfaces.DataAccess.Mesas;

namespace SistemaHotelColibri.BusinessLogic.Mesas;

public class EliminarMesaBL : IEliminarMesaBL
{
    private readonly IEliminarMesaDA _eliminarDA;
    private readonly IObtenerMesaPorIdDA _obtenerPorIdDA;
    private readonly ITienePedidoActivoMesaDA _pedidoActivoDA;

    public EliminarMesaBL(
        IEliminarMesaDA eliminarDA,
        IObtenerMesaPorIdDA obtenerPorIdDA,
        ITienePedidoActivoMesaDA pedidoActivoDA)
    {
        _eliminarDA = eliminarDA;
        _obtenerPorIdDA = obtenerPorIdDA;
        _pedidoActivoDA = pedidoActivoDA;
    }

    public async Task<ResultadoOperacion> EliminarAsync(int idMesa)
    {
        var mesa = await _obtenerPorIdDA.ObtenerPorIdAsync(idMesa);
        if (mesa == null)
        {
            return ResultadoOperacion.Error(Mensajes.NoEncontrado);
        }

        if (mesa.EstadoMesa == Estados.Mesa.Inactiva)
        {
            return ResultadoOperacion.Error(Mensajes.YaInactivo);
        }

        if (await _pedidoActivoDA.TienePedidoActivoAsync(idMesa))
        {
            return ResultadoOperacion.Error(Mensajes.MesaConPedidoActivo);
        }

        if (!await _eliminarDA.EliminarAsync(idMesa))
        {
            return ResultadoOperacion.Error(Mensajes.ErrorInactivarMesa);
        }

        return ResultadoOperacion.Ok(Mensajes.MesaInactivada, idMesa);
    }
}
