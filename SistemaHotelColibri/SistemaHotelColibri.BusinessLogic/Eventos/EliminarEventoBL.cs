using SistemaHotelColibri.Abstract.Comun;
using SistemaHotelColibri.Abstract.Interfaces.BusinessLogic.Eventos;
using SistemaHotelColibri.Abstract.Interfaces.DataAccess.Eventos;

namespace SistemaHotelColibri.BusinessLogic.Eventos;

public class EliminarEventoBL : IEliminarEventoBL
{
    private readonly IEliminarEventoDA _eliminarDA;
    private readonly IObtenerEventoPorIdDA _obtenerPorIdDA;

    public EliminarEventoBL(IEliminarEventoDA eliminarDA, IObtenerEventoPorIdDA obtenerPorIdDA)
    {
        _eliminarDA = eliminarDA;
        _obtenerPorIdDA = obtenerPorIdDA;
    }

    public async Task<ResultadoOperacion> EliminarAsync(int idEvento)
    {
        var evento = await _obtenerPorIdDA.ObtenerPorIdAsync(idEvento);
        if (evento == null)
        {
            return ResultadoOperacion.Error(Mensajes.NoEncontrado);
        }

        if (evento.EstadoEvento == Estados.Evento.Cancelado)
        {
            return ResultadoOperacion.Error(Mensajes.EventoYaCancelado);
        }

        if (evento.EstadoEvento == Estados.Evento.Realizado)
        {
            return ResultadoOperacion.Error(Mensajes.EventoYaRealizado);
        }

        try
        {
            await _eliminarDA.EliminarAsync(idEvento);            return ResultadoOperacion.Ok(Mensajes.EventoCancelado);
        }
        catch (Exception)
        {
            return ResultadoOperacion.Error(Mensajes.ErrorCancelarEvento);
        }
    }
}
