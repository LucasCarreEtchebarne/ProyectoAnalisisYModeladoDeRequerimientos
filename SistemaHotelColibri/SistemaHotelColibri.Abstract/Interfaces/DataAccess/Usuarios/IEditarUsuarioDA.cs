using SistemaHotelColibri.Abstract.Comun;
using SistemaHotelColibri.Abstract.Modelos.Usuarios;

namespace SistemaHotelColibri.Abstract.Interfaces.DataAccess.Usuarios;

public interface IEditarUsuarioDA
{
    Task<ResultadoOperacion> EditarAsync(UsuarioDto usuario);
}
