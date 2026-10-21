using SistemaHotelColibri.Abstract.Modelos.Usuarios;

namespace SistemaHotelColibri.Abstract.Interfaces.BusinessLogic.Usuarios;

public interface IObtenerRolBL
{
    Task<List<RolDto>> ObtenerActivosAsync();
}
