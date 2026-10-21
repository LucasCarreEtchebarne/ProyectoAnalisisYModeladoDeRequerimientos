using SistemaHotelColibri.Abstract.Comun;

namespace SistemaHotelColibri.Abstract.Interfaces.DataAccess.Usuarios;

public interface IRestablecerContrasenaUsuarioDA
{
    Task<ResultadoOperacion> EstablecerContrasenaTemporalAsync(int idUsuario, string contrasenaTemporal);
}
