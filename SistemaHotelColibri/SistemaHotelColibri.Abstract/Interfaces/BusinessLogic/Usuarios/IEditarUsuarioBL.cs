using SistemaHotelColibri.Abstract.Comun;
using SistemaHotelColibri.Abstract.Modelos.Usuarios;

namespace SistemaHotelColibri.Abstract.Interfaces.BusinessLogic.Usuarios;

public interface IEditarUsuarioBL
{
    Task<ResultadoOperacion> EditarAsync(UsuarioDto usuario, int idUsuarioActual);
}
