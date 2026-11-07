using SistemaHotelColibri.Abstract.Modelos.Mesas;

namespace SistemaHotelColibri.Abstract.Interfaces.DataAccess.Mesas;

public interface IEditarMesaDA
{
    Task<bool> EditarAsync(MesaDto mesa);
}
