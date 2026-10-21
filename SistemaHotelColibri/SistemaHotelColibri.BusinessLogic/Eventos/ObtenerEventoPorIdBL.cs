using SistemaHotelColibri.Abstract.Interfaces.BusinessLogic.Eventos;
using SistemaHotelColibri.Abstract.Interfaces.DataAccess.Eventos;
using SistemaHotelColibri.Abstract.Modelos.Eventos;

namespace SistemaHotelColibri.BusinessLogic.Eventos;

public class ObtenerEventoPorIdBL : IObtenerEventoPorIdBL
{
    private readonly IObtenerEventoPorIdDA _obtenerPorIdDA;

    public ObtenerEventoPorIdBL(IObtenerEventoPorIdDA obtenerPorIdDA)
    {
        _obtenerPorIdDA = obtenerPorIdDA;
    }

    public Task<EventoDto?> ObtenerPorIdAsync(int idEvento) => _obtenerPorIdDA.ObtenerPorIdAsync(idEvento);
}
