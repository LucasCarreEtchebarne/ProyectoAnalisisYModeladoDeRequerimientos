using SistemaHotelColibri.Abstract.Comun;
using SistemaHotelColibri.Abstract.Modelos.Usuarios;

namespace SistemaHotelColibri.Abstract.Interfaces.DataAccess.Usuarios;

public interface IAgregarUsuarioDA
{
    Task<ResultadoOperacion> AgregarAsync(UsuarioDto usuario, string contrasena);
}
