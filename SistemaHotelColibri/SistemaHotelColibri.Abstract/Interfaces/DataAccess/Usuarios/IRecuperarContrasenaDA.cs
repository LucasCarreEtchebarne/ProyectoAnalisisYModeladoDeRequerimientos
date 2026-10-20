using SistemaHotelColibri.Abstract.Comun;
using SistemaHotelColibri.Abstract.Modelos.Usuarios;

namespace SistemaHotelColibri.Abstract.Interfaces.DataAccess.Usuarios;

public interface IRecuperarContrasenaDA
{
    Task<SolicitudRecuperacionDto?> GenerarTokenAsync(string correoElectronico);
    Task<ResultadoOperacion> RestablecerAsync(string correoElectronico, string token, string contrasenaNueva);
}
