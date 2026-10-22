using SistemaHotelColibri.Abstract.Comun;
using SistemaHotelColibri.Abstract.Interfaces.DataAccess.TareasHousekeeping;
using SistemaHotelColibri.DataAccess.Contexto;

namespace SistemaHotelColibri.DataAccess.TareasHousekeeping;

public class CancelarTareaDA : ICancelarTareaDA
{
    private readonly HotelColibriContext _contexto;

    public CancelarTareaDA(HotelColibriContext contexto)
    {
        _contexto = contexto;
    }

    public async Task<bool> CancelarAsync(int idTarea, string motivo, DateTime fechaCancelacion)
    {
        var entidad = await _contexto.Housekeeping.FindAsync(idTarea);
        if (entidad == null)
        {
            return false;
        }

        entidad.EstadoTarea = Estados.Tarea.Cancelada;
        entidad.MotivoCancelacion = motivo;
        entidad.FechaCancelacion = fechaCancelacion;

        await _contexto.SaveChangesAsync();
        return true;
    }
}