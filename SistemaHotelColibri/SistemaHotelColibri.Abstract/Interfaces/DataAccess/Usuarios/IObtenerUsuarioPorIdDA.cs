using SistemaHotelColibri.Abstract.Modelos.Usuarios;

namespace SistemaHotelColibri.Abstract.Interfaces.DataAccess.Usuarios;

public interface IObtenerUsuarioPorIdDA
{
    Task<UsuarioDto?> ObtenerPorIdAsync(int idUsuario);
}
