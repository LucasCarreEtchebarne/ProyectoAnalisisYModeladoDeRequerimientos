using SistemaHotelColibri.Abstract.Comun;
using SistemaHotelColibri.Abstract.Interfaces.BusinessLogic.TareasHousekeeping;
using SistemaHotelColibri.Abstract.Interfaces.DataAccess.TareasHousekeeping;
using SistemaHotelColibri.Abstract.Modelos.TareasHousekeeping;

namespace SistemaHotelColibri.BusinessLogic.TareasHousekeeping;

public class AgregarTareaBL : IAgregarTareaBL
{
    private readonly IAgregarTareaDA _agregarDA;
    private readonly IValidadorTareaBL _validador;

    public AgregarTareaBL(IAgregarTareaDA agregarDA, IValidadorTareaBL validador)
    {
        _agregarDA = agregarDA;
        _validador = validador;
    }

    public async Task<ResultadoOperacion> AgregarAsync(TareaHousekeepingDto tarea)
    {
        ReglasTarea.Normalizar(tarea);

        // HSK-001: el Id lo genera la BD (IDENTITY).
        tarea.IdTarea = 0;
        tarea.FechaAsignacion = DateTime.Now;
        tarea.EstadoTarea = Estados.Tarea.Pendiente;
        tarea.FechaCompletada = null;
        tarea.MotivoCancelacion = null;
        tarea.FechaCancelacion = null;

        var error = await _validador.ValidarAsync(tarea);
        if (error != null)
        {
            return ResultadoOperacion.Error(error);
        }

        var idGenerado = await _agregarDA.AgregarAsync(tarea);

        return ResultadoOperacion.Ok(string.Format(Mensajes.TareaRegistrada, idGenerado), idGenerado);
    }
}