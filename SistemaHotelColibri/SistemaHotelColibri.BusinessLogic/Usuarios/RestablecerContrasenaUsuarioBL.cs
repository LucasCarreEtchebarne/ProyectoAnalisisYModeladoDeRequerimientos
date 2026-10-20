using SistemaHotelColibri.Abstract.Comun;
using SistemaHotelColibri.Abstract.Interfaces.BusinessLogic.Usuarios;
using SistemaHotelColibri.Abstract.Interfaces.DataAccess.Usuarios;

namespace SistemaHotelColibri.BusinessLogic.Usuarios;

public class RestablecerContrasenaUsuarioBL : IRestablecerContrasenaUsuarioBL
{
    private readonly IRestablecerContrasenaUsuarioDA _restablecerDA;
    private readonly IObtenerUsuarioPorIdDA _obtenerPorIdDA;

    public RestablecerContrasenaUsuarioBL(IRestablecerContrasenaUsuarioDA restablecerDA, IObtenerUsuarioPorIdDA obtenerPorIdDA)
    {
        _restablecerDA = restablecerDA;
        _obtenerPorIdDA = obtenerPorIdDA;
    }

    public async Task<ResultadoOperacion> RestablecerAsync(int idUsuario)
    {
        var usuario = await _obtenerPorIdDA.ObtenerPorIdAsync(idUsuario);
        if (usuario == null)
        {
            return ResultadoOperacion.Error(Mensajes.NoEncontrado);
        }

        var contrasenaTemporal = GeneradorContrasena.Generar();
        var resultado = await _restablecerDA.EstablecerContrasenaTemporalAsync(idUsuario, contrasenaTemporal);
        if (!resultado.Exito)
        {
            return resultado;
        }

        return ResultadoOperacion.Ok(string.Format(Mensajes.ContrasenaTemporalGenerada, usuario.NombreUsuario, contrasenaTemporal), idUsuario);
    }
}
