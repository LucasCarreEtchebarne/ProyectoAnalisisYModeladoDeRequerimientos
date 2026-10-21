using SistemaHotelColibri.Abstract.Modelos.Usuarios;

namespace SistemaHotelColibri.Abstract.Interfaces.DataAccess.Usuarios;

public interface IObtenerRolDA
{
    Task<List<RolDto>> ObtenerActivosAsync();
}
