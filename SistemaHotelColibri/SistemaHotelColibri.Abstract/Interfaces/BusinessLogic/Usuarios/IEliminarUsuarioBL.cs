using SistemaHotelColibri.Abstract.Comun;

namespace SistemaHotelColibri.Abstract.Interfaces.BusinessLogic.Usuarios;

public interface IEliminarUsuarioBL
{
    Task<ResultadoOperacion> EliminarAsync(int idUsuario, int idUsuarioActual);
}
