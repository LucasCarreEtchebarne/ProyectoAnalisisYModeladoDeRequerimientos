using SistemaHotelColibri.Abstract.Comun;
using SistemaHotelColibri.Abstract.Interfaces.BusinessLogic.TareasHousekeeping;
using SistemaHotelColibri.Abstract.Interfaces.DataAccess.TareasHousekeeping;
using SistemaHotelColibri.Abstract.Modelos.TareasHousekeeping;

namespace SistemaHotelColibri.BusinessLogic.TareasHousekeeping;

public class EditarTareaBL : IEditarTareaBL
{
    private readonly IEditarTareaDA _editarDA;
    private readonly IObtenerTareaPorIdDA _obtenerPorIdDA;
    private readonly IValidadorTareaBL _validador;

    public EditarTareaBL(
        IEditarTareaDA editarDA,
        IObtenerTareaPorIdDA obtenerPorIdDA,
        IValidadorTareaBL validador)
    {
        _editarDA = editarDA;
        _obtenerPorIdDA = obtenerPorIdDA;
        _validador = validador;
    }

    public async Task<ResultadoOperacion> EditarAsync(TareaHousekeepingDto datos)
    {
        var tarea = await _obtenerPorIdDA.ObtenerPorIdAsync(datos.IdTarea);
        if (tarea == null)
        {
            return ResultadoOperacion.Error(Mensajes.NoEncontrado);
        }

        if (!tarea.EstaActiva)
        {
            return ResultadoOperacion.Error(Mensajes.TareaNoModificable);
        }

        ReglasTarea.Normalizar(datos);

        // Estos datos no se editan en el formulario: se conservan los originales.
        datos.FechaAsignacion = tarea.FechaAsignacion;
        datos.EstadoTarea = tarea.EstadoTarea;

        var error = await _validador.ValidarAsync(datos);
        if (error != null)
        {
            return ResultadoOperacion.Error(error);
        }

        if (!await _editarDA.EditarAsync(datos))
        {
            return ResultadoOperacion.Error(Mensajes.ErrorModificarTarea);
        }

        return ResultadoOperacion.Ok(Mensajes.TareaModificada, datos.IdTarea);
    }
}