using SistemaHotelColibri.Abstract.Comun;
using SistemaHotelColibri.Abstract.Modelos.Usuarios;

namespace SistemaHotelColibri.Abstract.Interfaces.DataAccess.Usuarios;

public interface IObtenerUsuarioDA
{
    Task<ResultadoPaginado<UsuarioDto>> ObtenerAsync(FiltroUsuario filtro);
}
