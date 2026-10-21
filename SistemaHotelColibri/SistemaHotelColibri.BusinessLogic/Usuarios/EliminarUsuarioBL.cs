using SistemaHotelColibri.Abstract.Comun;
using SistemaHotelColibri.Abstract.Interfaces.BusinessLogic.Usuarios;
using SistemaHotelColibri.Abstract.Interfaces.DataAccess.Usuarios;

namespace SistemaHotelColibri.BusinessLogic.Usuarios;

public class EliminarUsuarioBL : IEliminarUsuarioBL
{
    private readonly IEliminarUsuarioDA _eliminarDA;
    private readonly IObtenerUsuarioPorIdDA _obtenerPorIdDA;

    public EliminarUsuarioBL(IEliminarUsuarioDA eliminarDA, IObtenerUsuarioPorIdDA obtenerPorIdDA)
    {
        _eliminarDA = eliminarDA;
        _obtenerPorIdDA = obtenerPorIdDA;
    }

    public async Task<ResultadoOperacion> EliminarAsync(int idUsuario, int idUsuarioActual)
    {
        if (idUsuario == idUsuarioActual)
        {
            return ResultadoOperacion.Error(Mensajes.AccionSobreUsuarioPropio);
        }

        var usuario = await _obtenerPorIdDA.ObtenerPorIdAsync(idUsuario);
        if (usuario == null)
        {
            return ResultadoOperacion.Error(Mensajes.NoEncontrado);
        }

        if (usuario.Estado == Estados.Usuario.Inactivo)
        {
            return ResultadoOperacion.Error(Mensajes.YaInactivo);
        }

        return await _eliminarDA.EliminarAsync(idUsuario)
            ? ResultadoOperacion.Ok(Mensajes.UsuarioInactivado, idUsuario)
            : ResultadoOperacion.Error(Mensajes.NoEncontrado);
    }
}
