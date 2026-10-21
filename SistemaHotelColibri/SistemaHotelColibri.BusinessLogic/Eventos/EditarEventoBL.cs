using SistemaHotelColibri.Abstract.Comun;
using SistemaHotelColibri.Abstract.Interfaces.BusinessLogic.Eventos;
using SistemaHotelColibri.Abstract.Interfaces.DataAccess.Clientes;
using SistemaHotelColibri.Abstract.Interfaces.DataAccess.Eventos;
using SistemaHotelColibri.Abstract.Modelos.Eventos;

namespace SistemaHotelColibri.BusinessLogic.Eventos;

public class EditarEventoBL : IEditarEventoBL
{
    private readonly IEditarEventoDA _editarDA;
    private readonly IObtenerEventoPorIdDA _obtenerPorIdDA;
    private readonly IObtenerClientePorIdDA _obtenerClientePorIdDA;
    private readonly IObtenerEspacioEventoPorIdDA _obtenerEspacioPorIdDA;
    private readonly IEspacioDisponibleEventoDA _espacioDisponibleDA;

    public EditarEventoBL(
        IEditarEventoDA editarDA,
        IObtenerEventoPorIdDA obtenerPorIdDA,
        IObtenerClientePorIdDA obtenerClientePorIdDA,
        IObtenerEspacioEventoPorIdDA obtenerEspacioPorIdDA,
        IEspacioDisponibleEventoDA espacioDisponibleDA)
    {
        _editarDA = editarDA;
        _obtenerPorIdDA = obtenerPorIdDA;
        _obtenerClientePorIdDA = obtenerClientePorIdDA;
        _obtenerEspacioPorIdDA = obtenerEspacioPorIdDA;
        _espacioDisponibleDA = espacioDisponibleDA;
    }

    public async Task<ResultadoOperacion> EditarAsync(EventoDto evento)
    {
        var original = await _obtenerPorIdDA.ObtenerPorIdAsync(evento.IdEvento);
        if (original == null)
        {
            return ResultadoOperacion.Error(Mensajes.NoEncontrado);
        }

        ReglasEvento.Normalizar(evento);

        var cliente = await _obtenerClientePorIdDA.ObtenerPorIdAsync(evento.IdCliente);
        var espacio = await _obtenerEspacioPorIdDA.ObtenerPorIdAsync(evento.IdEspacio);
        var error = ReglasEvento.Validar(evento, original, cliente, espacio);
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
            await _editarDA.EditarAsync(evento);
            return ResultadoOperacion.Ok(Mensajes.EventoModificado, evento.IdEvento);
        }
        catch (Exception)
        {
            return ResultadoOperacion.Error(Mensajes.ErrorModificarEvento);
        }
    }
}
