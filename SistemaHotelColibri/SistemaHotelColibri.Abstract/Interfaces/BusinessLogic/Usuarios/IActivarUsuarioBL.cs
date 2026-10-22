using SistemaHotelColibri.Abstract.Comun;

namespace SistemaHotelColibri.Abstract.Interfaces.BusinessLogic.Usuarios;

public interface IActivarUsuarioBL
{
    Task<ResultadoOperacion> ActivarAsync(int idUsuario);
}
