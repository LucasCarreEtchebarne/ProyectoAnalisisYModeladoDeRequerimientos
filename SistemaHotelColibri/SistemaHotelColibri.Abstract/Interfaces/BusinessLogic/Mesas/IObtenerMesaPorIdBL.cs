using SistemaHotelColibri.Abstract.Modelos.Mesas;

namespace SistemaHotelColibri.Abstract.Interfaces.BusinessLogic.Mesas;

public interface IObtenerMesaPorIdBL
{
    Task<MesaDto?> ObtenerPorIdAsync(int idMesa);
}
