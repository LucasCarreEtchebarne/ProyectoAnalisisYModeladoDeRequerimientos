using SistemaHotelColibri.Abstract.Comun;
using SistemaHotelColibri.Abstract.Interfaces.BusinessLogic.Habitaciones;
using SistemaHotelColibri.Abstract.Interfaces.DataAccess.Habitaciones;
using SistemaHotelColibri.Abstract.Interfaces.DataAccess.Pedidos;
using SistemaHotelColibri.Abstract.Interfaces.DataAccess.Reservas;

namespace SistemaHotelColibri.BusinessLogic.Habitaciones;

public class EliminarHabitacionBL : IEliminarHabitacionBL
{
    private readonly IEliminarHabitacionDA _eliminarDA;
    private readonly IObtenerHabitacionPorIdDA _obtenerPorIdDA;
    private readonly IObtenerReservasVigentesDA _reservasDA;
    private readonly IObtenerPedidosAbiertosDA _pedidosDA;

    public EliminarHabitacionBL(
        IEliminarHabitacionDA eliminarDA,
        IObtenerHabitacionPorIdDA obtenerPorIdDA,
        IObtenerReservasVigentesDA reservasDA,
        IObtenerPedidosAbiertosDA pedidosDA)
    {
        _eliminarDA = eliminarDA;
        _obtenerPorIdDA = obtenerPorIdDA;
        _reservasDA = reservasDA;
        _pedidosDA = pedidosDA;
    }

    public async Task<ResultadoOperacion> EliminarAsync(int idHabitacion)
    {
        var habitacion = await _obtenerPorIdDA.ObtenerPorIdAsync(idHabitacion);
        if (habitacion == null)
        {
            return ResultadoOperacion.Error(Mensajes.NoEncontrado);
        }

        if (habitacion.EstadoHabitacion == Estados.Habitacion.Inactiva)
        {
            return ResultadoOperacion.Error(Mensajes.YaInactivo);
        }

        var motivo = await UsoHabitacion.MotivoParaNoInactivarAsync(idHabitacion, _reservasDA, _pedidosDA);
        if (motivo != null)
        {
            return ResultadoOperacion.Error(motivo);
        }

        if (!await _eliminarDA.EliminarAsync(idHabitacion))
        {
            return ResultadoOperacion.Error(Mensajes.ErrorEliminarHabitacion);
        }

        return ResultadoOperacion.Ok(Mensajes.HabitacionEliminadaCorrectamente, idHabitacion);
    }
}
