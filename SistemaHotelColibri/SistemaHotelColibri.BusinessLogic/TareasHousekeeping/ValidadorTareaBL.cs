using SistemaHotelColibri.Abstract.Comun;
using SistemaHotelColibri.Abstract.Interfaces.BusinessLogic.TareasHousekeeping;
using SistemaHotelColibri.Abstract.Interfaces.DataAccess.Habitaciones;
using SistemaHotelColibri.Abstract.Interfaces.DataAccess.TareasHousekeeping;
using SistemaHotelColibri.Abstract.Modelos.TareasHousekeeping;

namespace SistemaHotelColibri.BusinessLogic.TareasHousekeeping;

public class ValidadorTareaBL : IValidadorTareaBL
{
    private readonly IObtenerHabitacionPorIdDA _obtenerHabitacionDA;
    private readonly IObtenerPersonalHousekeepingDA _obtenerPersonalDA;
    private readonly IExisteTareaActivaDA _existeTareaActivaDA;

    public ValidadorTareaBL(
        IObtenerHabitacionPorIdDA obtenerHabitacionDA,
        IObtenerPersonalHousekeepingDA obtenerPersonalDA,
        IExisteTareaActivaDA existeTareaActivaDA)
    {
        _obtenerHabitacionDA = obtenerHabitacionDA;
        _obtenerPersonalDA = obtenerPersonalDA;
        _existeTareaActivaDA = existeTareaActivaDA;
    }

    public async Task<string?> ValidarAsync(TareaHousekeepingDto tarea)
    {
        var error = ReglasTarea.Validar(tarea);
        if (error != null)
        {
            return error;
        }

        if (await _obtenerHabitacionDA.ObtenerPorIdAsync(tarea.IdHabitacion) == null)
        {
            return Mensajes.HabitacionTareaNoExiste;
        }

        var personal = await _obtenerPersonalDA.ObtenerAsync();
        if (!personal.Any(p => p.IdUsuario == tarea.IdUsuario))
        {
            return Mensajes.PersonalNoValido;
        }

        if (await _existeTareaActivaDA.ExisteTareaActivaAsync(tarea.IdHabitacion, tarea.TipoTarea, tarea.IdTarea))
        {
            return Mensajes.TareaDuplicada;
        }

        return null;
    }
}