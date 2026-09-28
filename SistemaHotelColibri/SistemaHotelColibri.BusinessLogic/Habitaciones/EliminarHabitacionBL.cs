using SistemaHotelColibri.Abstract.Comun;
using SistemaHotelColibri.Abstract.Interfaces.BusinessLogic.Habitaciones;
using SistemaHotelColibri.Abstract.Interfaces.DataAccess.Habitaciones;

namespace SistemaHotelColibri.BusinessLogic.Habitaciones;

public class EliminarHabitacionBL : IEliminarHabitacionBL
{
    private readonly IEliminarHabitacionDA _eliminarDA;
    private readonly IObtenerHabitacionPorIdDA _obtenerPorIdDA;

    public EliminarHabitacionBL(
        IEliminarHabitacionDA eliminarDA,
        IObtenerHabitacionPorIdDA obtenerPorIdDA)
    {
        _eliminarDA = eliminarDA;
        _obtenerPorIdDA = obtenerPorIdDA;
    }

    public async Task<ResultadoOperacion> EliminarAsync(int idHabitacion)
    {
        var habitacion = await _obtenerPorIdDA.ObtenerPorIdAsync(idHabitacion);
        if (habitacion == null)
        {
            return ResultadoOperacion.Error(Mensajes.NoEncontrado);
        }

        if (!await _eliminarDA.EliminarAsync(idHabitacion))
        {
            return ResultadoOperacion.Error(Mensajes.ErrorEliminarHabitacion);
        }

        return ResultadoOperacion.Ok(Mensajes.HabitacionEliminadaCorrectamente, idHabitacion);
    }
}
