using SistemaHotelColibri.Abstract.Comun;
using SistemaHotelColibri.Abstract.Interfaces.BusinessLogic.TareasHousekeeping;
using SistemaHotelColibri.Abstract.Interfaces.DataAccess.TareasHousekeeping;

namespace SistemaHotelColibri.BusinessLogic.TareasHousekeeping;

public class CambiarEstadoTareaBL : ICambiarEstadoTareaBL
{
    private readonly ICambiarEstadoTareaDA _cambiarEstadoDA;
    private readonly IObtenerTareaPorIdDA _obtenerPorIdDA;

    public CambiarEstadoTareaBL(ICambiarEstadoTareaDA cambiarEstadoDA, IObtenerTareaPorIdDA obtenerPorIdDA)
    {
        _cambiarEstadoDA = cambiarEstadoDA;
        _obtenerPorIdDA = obtenerPorIdDA;
    }

    public async Task<ResultadoOperacion> CambiarEstadoAsync(int idTarea, string nuevoEstado)
    {
        var tarea = await _obtenerPorIdDA.ObtenerPorIdAsync(idTarea);
        if (tarea == null)
        {
            return ResultadoOperacion.Error(Mensajes.NoEncontrado);
        }

        // La cancelación no pasa por aquí porque exige un motivo (HSK-006).
        if (!Estados.Tarea.Todos.Contains(nuevoEstado) || nuevoEstado == Estados.Tarea.Cancelada)
        {
            return ResultadoOperacion.Error(Mensajes.EstadoTareaInvalido);
        }

        if (!ReglasTarea.PuedeCambiar(tarea.EstadoTarea, nuevoEstado))
        {
            return ResultadoOperacion.Error(string.Format(
                Mensajes.TransicionEstadoInvalida,
                Formato.EstadoTarea(tarea.EstadoTarea),
                Formato.EstadoTarea(nuevoEstado)));
        }

        DateTime? fechaCompletada = nuevoEstado == Estados.Tarea.Completada ? DateTime.Now : null;

        if (!await _cambiarEstadoDA.CambiarEstadoAsync(idTarea, nuevoEstado, fechaCompletada))
        {
            return ResultadoOperacion.Error(Mensajes.ErrorModificarTarea);
        }

        return ResultadoOperacion.Ok(
            string.Format(Mensajes.TareaEstadoActualizado, idTarea, Formato.EstadoTarea(nuevoEstado)),
            idTarea);
    }
}