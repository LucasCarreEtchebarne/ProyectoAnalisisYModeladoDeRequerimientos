using SistemaHotelColibri.Abstract.Comun;
using SistemaHotelColibri.Abstract.Modelos.Usuarios;

namespace SistemaHotelColibri.Abstract.Interfaces.BusinessLogic.Usuarios;

public interface IAgregarUsuarioBL
{
    Task<ResultadoOperacion> AgregarAsync(UsuarioDto usuario, string contrasena);
}
