using SistemaHotelColibri.Abstract.Comun;
using SistemaHotelColibri.Abstract.Interfaces.BusinessLogic.Habitaciones;
using SistemaHotelColibri.Abstract.Interfaces.DataAccess.Habitaciones;
using SistemaHotelColibri.Abstract.Modelos.Habitaciones;

namespace SistemaHotelColibri.BusinessLogic.Habitaciones;

public class AgregarHabitacionBL : IAgregarHabitacionBL
{
    private readonly IAgregarHabitacionDA _agregarDA;
    private readonly IExisteNumeroHabitacionDA _existeNumeroDA;

    public AgregarHabitacionBL(IAgregarHabitacionDA agregarDA, IExisteNumeroHabitacionDA existeNumeroDA)
    {
        _agregarDA = agregarDA;
        _existeNumeroDA = existeNumeroDA;
    }

    public async Task<ResultadoOperacion> AgregarAsync(HabitacionDto habitacion)
    {
        ReglasHabitacion.Normalizar(habitacion);

        var error = ReglasHabitacion.Validar(habitacion);
        if (error != null)
        {
            return ResultadoOperacion.Error(error);
        }

        if (await _existeNumeroDA.ExisteNumeroAsync(habitacion.NumeroHabitacion, 0))
        {
            return ResultadoOperacion.Error(Mensajes.HabitacionDuplicada);
        }

        habitacion.IdHabitacion = 0;

        var idGenerado = await _agregarDA.AgregarAsync(habitacion);

        return ResultadoOperacion.Ok(string.Format(Mensajes.HabitacionRegistrada, idGenerado), idGenerado);
    }
}
