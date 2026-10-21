using SistemaHotelColibri.Abstract.Comun;

namespace SistemaHotelColibri.Abstract.Interfaces.BusinessLogic.Usuarios;

public interface IRecuperarContrasenaBL
{
    Task SolicitarAsync(string correoElectronico, string urlRestablecer);
    Task<ResultadoOperacion> RestablecerAsync(string correoElectronico, string token, string contrasenaNueva);
}
