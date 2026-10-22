using SistemaHotelColibri.Abstract.Modelos.Eventos;

namespace SistemaHotelColibri.Abstract.Interfaces.DataAccess.Eventos;

public interface IAgregarEventoDA
{
    Task<int> AgregarAsync(EventoDto evento);
}
