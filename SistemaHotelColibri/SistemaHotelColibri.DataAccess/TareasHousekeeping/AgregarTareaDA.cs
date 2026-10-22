using SistemaHotelColibri.Abstract.Interfaces.DataAccess.TareasHousekeeping;
using SistemaHotelColibri.Abstract.Modelos.TareasHousekeeping;
using SistemaHotelColibri.DataAccess.Contexto;

namespace SistemaHotelColibri.DataAccess.TareasHousekeeping;

public class AgregarTareaDA : IAgregarTareaDA
{
    private readonly HotelColibriContext _contexto;

    public AgregarTareaDA(HotelColibriContext contexto)
    {
        _contexto = contexto;
    }

    public async Task<int> AgregarAsync(TareaHousekeepingDto tarea)
    {
        var entidad = tarea.AEntidad();
        _contexto.Housekeeping.Add(entidad);
        await _contexto.SaveChangesAsync();
        return entidad.IdTarea;
    }
}