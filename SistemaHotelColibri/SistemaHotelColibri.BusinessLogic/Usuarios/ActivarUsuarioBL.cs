using SistemaHotelColibri.Abstract.Comun;
using SistemaHotelColibri.Abstract.Interfaces.BusinessLogic.Usuarios;
using SistemaHotelColibri.Abstract.Interfaces.DataAccess.Usuarios;

namespace SistemaHotelColibri.BusinessLogic.Usuarios;

public class ActivarUsuarioBL : IActivarUsuarioBL
{
    private readonly IActivarUsuarioDA _activarDA;
    private readonly IObtenerUsuarioPorIdDA _obtenerPorIdDA;

    public ActivarUsuarioBL(IActivarUsuarioDA activarDA, IObtenerUsuarioPorIdDA obtenerPorIdDA)
    {
        _activarDA = activarDA;
        _obtenerPorIdDA = obtenerPorIdDA;
    }

    public async Task<ResultadoOperacion> ActivarAsync(int idUsuario)
    {
        var usuario = await _obtenerPorIdDA.ObtenerPorIdAsync(idUsuario);
        if (usuario == null)
        {
            return ResultadoOperacion.Error(Mensajes.NoEncontrado);
        }

        if (usuario.Estado == Estados.Usuario.Activo)
        {
            return ResultadoOperacion.Error(Mensajes.UsuarioYaActivo);
        }

        return await _activarDA.ActivarAsync(idUsuario)
            ? ResultadoOperacion.Ok(Mensajes.UsuarioActivado, idUsuario)
            : ResultadoOperacion.Error(Mensajes.NoEncontrado);
    }
}
