using SistemaHotelColibri.Abstract.Modelos.Eventos;

namespace SistemaHotelColibri.Abstract.Interfaces.DataAccess.Eventos;

public interface IObtenerEspacioEventoPorIdDA
{
    Task<EspacioEventoDto?> ObtenerPorIdAsync(int idEspacio);
}
