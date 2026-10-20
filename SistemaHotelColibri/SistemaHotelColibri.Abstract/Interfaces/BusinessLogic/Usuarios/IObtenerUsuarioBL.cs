using SistemaHotelColibri.Abstract.Comun;
using SistemaHotelColibri.Abstract.Modelos.Usuarios;

namespace SistemaHotelColibri.Abstract.Interfaces.BusinessLogic.Usuarios;

public interface IObtenerUsuarioBL
{
    Task<ResultadoPaginado<UsuarioDto>> ObtenerAsync(FiltroUsuario filtro);
}
