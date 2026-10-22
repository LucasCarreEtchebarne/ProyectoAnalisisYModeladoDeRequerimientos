using SistemaHotelColibri.Abstract.Interfaces.DataAccess.TareasHousekeeping;
using SistemaHotelColibri.DataAccess.Contexto;

namespace SistemaHotelColibri.DataAccess.TareasHousekeeping;

public class CambiarEstadoTareaDA : ICambiarEstadoTareaDA
{
    private readonly HotelColibriContext _contexto;

    public CambiarEstadoTareaDA(HotelColibriContext contexto)
    {
        _contexto = contexto;
    }

    public async Task<bool> CambiarEstadoAsync(int idTarea, string nuevoEstado, DateTime? fechaCompletada)
    {
        var entidad = await _contexto.Housekeeping.FindAsync(idTarea);
        if (entidad == null)
        {
            return false;
        }

        entidad.EstadoTarea = nuevoEstado;
        entidad.FechaCompletada = fechaCompletada;

        await _contexto.SaveChangesAsync();
        return true;
    }
}