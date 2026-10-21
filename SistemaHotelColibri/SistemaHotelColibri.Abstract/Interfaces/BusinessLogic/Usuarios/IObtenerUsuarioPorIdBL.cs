using SistemaHotelColibri.Abstract.Modelos.Usuarios;

namespace SistemaHotelColibri.Abstract.Interfaces.BusinessLogic.Usuarios;

public interface IObtenerUsuarioPorIdBL
{
    Task<UsuarioDto?> ObtenerPorIdAsync(int idUsuario);
}
