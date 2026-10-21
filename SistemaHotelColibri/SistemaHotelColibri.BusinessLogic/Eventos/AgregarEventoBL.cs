using SistemaHotelColibri.Abstract.Comun;
using SistemaHotelColibri.Abstract.Interfaces.BusinessLogic.Eventos;
using SistemaHotelColibri.Abstract.Interfaces.DataAccess.Clientes;
using SistemaHotelColibri.Abstract.Interfaces.DataAccess.Eventos;
using SistemaHotelColibri.Abstract.Modelos.Eventos;

namespace SistemaHotelColibri.BusinessLogic.Eventos;

public class AgregarEventoBL : IAgregarEventoBL
{
    private readonly IAgregarEventoDA _agregarDA;
    private readonly IObtenerClientePorIdDA _obtenerClientePorIdDA;
    private readonly IObtenerEspacioEventoPorIdDA _obtenerEspacioPorIdDA;
    private readonly IEspacioDisponibleEventoDA _espacioDisponibleDA;

    public AgregarEventoBL(
        IAgregarEventoDA agregarDA,
        IObtenerClientePorIdDA obtenerClientePorIdDA,
        IObtenerEspacioEventoPorIdDA obtenerEspacioPorIdDA,
        IEspacioDisponibleEventoDA espacioDisponibleDA)
    {
        _agregarDA = agregarDA;
        _obtenerClientePorIdDA = obtenerClientePorIdDA;
        _obtenerEspacioPorIdDA = obtenerEspacioPorIdDA;
        _espacioDisponibleDA = espacioDisponibleDA;
    }

    public async Task<ResultadoOperacion> AgregarAsync(EventoDto evento)
    {
        ReglasEvento.Normalizar(evento);

        var cliente = await _obtenerClientePorIdDA.ObtenerPorIdAsync(evento.IdCliente);
        var espacio = await _obtenerEspacioPorIdDA.ObtenerPorIdAsync(evento.IdEspacio);
        var error = ReglasEvento.Validar(evento, null, cliente, espacio);
        if (error != null)
        {
            return ResultadoOperacion.Error(error);
        }

        if (evento.EstadoEvento != Estados.Evento.Cancelado
            && !await _espacioDisponibleDA.EstaDisponibleAsync(
                evento.IdEspacio, evento.FechaEvento, evento.HoraInicio, evento.HoraFin, evento.IdEvento))
        {
            return ResultadoOperacion.Error(Mensajes.EspacioEventoNoDisponible);
        }

        try
        {
            var id = await _agregarDA.AgregarAsync(evento);
            return ResultadoOperacion.Ok(Mensajes.EventoRegistrado, id);
        }
        catch (Exception)
        {
            return ResultadoOperacion.Error(Mensajes.ErrorRegistrarEvento);
        }
    }
}
