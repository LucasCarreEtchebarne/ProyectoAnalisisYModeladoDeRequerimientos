using SistemaHotelColibri.Abstract.Comun;
using SistemaHotelColibri.Abstract.Interfaces.BusinessLogic.TareasHousekeeping;
using SistemaHotelColibri.Abstract.Interfaces.DataAccess.TareasHousekeeping;

namespace SistemaHotelColibri.BusinessLogic.TareasHousekeeping;

public class CancelarTareaBL : ICancelarTareaBL
{
    private const int LongitudMaximaMotivo = 250;

    private readonly ICancelarTareaDA _cancelarDA;
    private readonly IObtenerTareaPorIdDA _obtenerPorIdDA;

    public CancelarTareaBL(ICancelarTareaDA cancelarDA, IObtenerTareaPorIdDA obtenerPorIdDA)
    {
        _cancelarDA = cancelarDA;
        _obtenerPorIdDA = obtenerPorIdDA;
    }

    public async Task<ResultadoOperacion> CancelarAsync(int idTarea, string? motivo)
    {
        var tarea = await _obtenerPorIdDA.ObtenerPorIdAsync(idTarea);
        if (tarea == null)
        {
            return ResultadoOperacion.Error(Mensajes.NoEncontrado);
        }

        if (!tarea.EstaActiva)
        {
            return ResultadoOperacion.Error(Mensajes.TareaNoCancelable);
        }

        var motivoLimpio = motivo?.Trim();
        if (string.IsNullOrEmpty(motivoLimpio))
        {
            return ResultadoOperacion.Error(Mensajes.MotivoCancelacionObligatorio);
        }

        if (motivoLimpio.Length > LongitudMaximaMotivo)
        {
            return ResultadoOperacion.Error(string.Format(Mensajes.LongitudMaxima, "motivo", LongitudMaximaMotivo));
        }

        if (!await _cancelarDA.CancelarAsync(idTarea, motivoLimpio, DateTime.Now))
        {
            return ResultadoOperacion.Error(Mensajes.ErrorModificarTarea);
        }

        return ResultadoOperacion.Ok(string.Format(Mensajes.TareaCancelada, idTarea), idTarea);
    }
}