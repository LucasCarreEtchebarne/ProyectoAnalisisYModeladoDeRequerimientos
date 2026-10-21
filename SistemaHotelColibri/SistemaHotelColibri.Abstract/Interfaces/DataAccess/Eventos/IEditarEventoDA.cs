using SistemaHotelColibri.Abstract.Modelos.Eventos;

namespace SistemaHotelColibri.Abstract.Interfaces.DataAccess.Eventos;

public interface IEditarEventoDA
{
    Task<bool> EditarAsync(EventoDto evento);
}
