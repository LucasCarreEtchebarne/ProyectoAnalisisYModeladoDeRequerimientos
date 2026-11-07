using SistemaHotelColibri.Abstract.Modelos.Mesas;

namespace SistemaHotelColibri.Abstract.Interfaces.DataAccess.Mesas;

public interface IAgregarMesaDA
{
    Task<int> AgregarAsync(MesaDto mesa);
}
