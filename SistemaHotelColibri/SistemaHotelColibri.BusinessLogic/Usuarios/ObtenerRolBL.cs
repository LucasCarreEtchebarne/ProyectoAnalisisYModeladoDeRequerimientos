using SistemaHotelColibri.Abstract.Interfaces.BusinessLogic.Usuarios;
using SistemaHotelColibri.Abstract.Interfaces.DataAccess.Usuarios;
using SistemaHotelColibri.Abstract.Modelos.Usuarios;

namespace SistemaHotelColibri.BusinessLogic.Usuarios;

public class ObtenerRolBL : IObtenerRolBL
{
    private readonly IObtenerRolDA _obtenerRolDA;

    public ObtenerRolBL(IObtenerRolDA obtenerRolDA)
    {
        _obtenerRolDA = obtenerRolDA;
    }

    public Task<List<RolDto>> ObtenerActivosAsync() => _obtenerRolDA.ObtenerActivosAsync();
}
