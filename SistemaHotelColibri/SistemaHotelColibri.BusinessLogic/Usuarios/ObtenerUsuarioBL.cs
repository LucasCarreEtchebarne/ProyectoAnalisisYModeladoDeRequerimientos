using SistemaHotelColibri.Abstract.Comun;
using SistemaHotelColibri.Abstract.Interfaces.BusinessLogic.Usuarios;
using SistemaHotelColibri.Abstract.Interfaces.DataAccess.Usuarios;
using SistemaHotelColibri.Abstract.Modelos.Usuarios;

namespace SistemaHotelColibri.BusinessLogic.Usuarios;

public class ObtenerUsuarioBL : IObtenerUsuarioBL
{
    private readonly IObtenerUsuarioDA _obtenerDA;

    public ObtenerUsuarioBL(IObtenerUsuarioDA obtenerDA)
    {
        _obtenerDA = obtenerDA;
    }

    public Task<ResultadoPaginado<UsuarioDto>> ObtenerAsync(FiltroUsuario filtro)
    {
        filtro.Busqueda = string.IsNullOrWhiteSpace(filtro.Busqueda) ? null : filtro.Busqueda.Trim();
        return _obtenerDA.ObtenerAsync(filtro);
    }
}
