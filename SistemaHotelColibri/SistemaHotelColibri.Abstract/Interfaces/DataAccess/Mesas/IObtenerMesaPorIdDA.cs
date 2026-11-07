using SistemaHotelColibri.Abstract.Modelos.Mesas;

namespace SistemaHotelColibri.Abstract.Interfaces.DataAccess.Mesas;

public interface IObtenerMesaPorIdDA
{
    Task<MesaDto?> ObtenerPorIdAsync(int idMesa);
}
