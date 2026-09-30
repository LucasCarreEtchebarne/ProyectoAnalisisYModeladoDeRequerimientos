using SistemaHotelColibri.Abstract.Comun;
using SistemaHotelColibri.Abstract.Interfaces.BusinessLogic.Habitaciones;
using SistemaHotelColibri.Abstract.Interfaces.DataAccess.Habitaciones;
using SistemaHotelColibri.Abstract.Modelos.Habitaciones;

namespace SistemaHotelColibri.BusinessLogic.Habitaciones;

public class EditarHabitacionBL : IEditarHabitacionBL
{
    private readonly IEditarHabitacionDA _editarDA;
    private readonly IObtenerHabitacionPorIdDA _obtenerPorIdDA;
    private readonly IExisteNumeroHabitacionDA _existeNumeroDA;

    public EditarHabitacionBL(
        IEditarHabitacionDA editarDA,
        IObtenerHabitacionPorIdDA obtenerPorIdDA,
        IExisteNumeroHabitacionDA existeNumeroDA)
    {
        _editarDA = editarDA;
        _obtenerPorIdDA = obtenerPorIdDA;
        _existeNumeroDA = existeNumeroDA;
    }

    public async Task<ResultadoOperacion> EditarAsync(HabitacionDto datos)
    {
        var habitacion = await _obtenerPorIdDA.ObtenerPorIdAsync(datos.IdHabitacion);
        if (habitacion == null)
        {
            return ResultadoOperacion.Error(Mensajes.NoEncontrado);
        }

        ReglasHabitacion.Normalizar(datos);

        var error = ReglasHabitacion.Validar(datos);
        if (error != null)
        {
            return ResultadoOperacion.Error(error);
        }

        if (await _existeNumeroDA.ExisteNumeroAsync(datos.NumeroHabitacion, datos.IdHabitacion))
        {
            return ResultadoOperacion.Error(Mensajes.HabitacionDuplicada);
        }

        var cambioNumeroHabitacion = !string.Equals(habitacion.NumeroHabitacion, datos.NumeroHabitacion, StringComparison.OrdinalIgnoreCase);
        if (cambioNumeroHabitacion
            && (habitacion.EstadoHabitacion == Estados.Habitacion.Ocupada
                || habitacion.EstadoHabitacion == Estados.Habitacion.Mantenimiento))
        {
            return ResultadoOperacion.Error(Mensajes.HabitacionNoDisponible);
        }

        habitacion.NumeroHabitacion = datos.NumeroHabitacion;
        habitacion.TipoHabitacion = datos.TipoHabitacion;
        habitacion.EstadoHabitacion = datos.EstadoHabitacion;
        habitacion.Capacidad = datos.Capacidad;
        habitacion.Precio = datos.Precio;
        habitacion.Piso = datos.Piso;
        habitacion.Descripcion = datos.Descripcion;

        if (!await _editarDA.EditarAsync(habitacion))
        {
            return ResultadoOperacion.Error(Mensajes.ErrorModificarHabitacion);
        }

        return ResultadoOperacion.Ok(Mensajes.HabitacionModificadaCorrectamente, habitacion.IdHabitacion);
    }
}
