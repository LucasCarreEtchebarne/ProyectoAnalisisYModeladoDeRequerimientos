using SistemaHotelColibri.Abstract.Comun;
using SistemaHotelColibri.Abstract.Interfaces.BusinessLogic.Usuarios;
using SistemaHotelColibri.Abstract.Interfaces.DataAccess.Usuarios;
using SistemaHotelColibri.Abstract.Modelos.Usuarios;

namespace SistemaHotelColibri.BusinessLogic.Usuarios;

public class EditarUsuarioBL : IEditarUsuarioBL
{
    private readonly IEditarUsuarioDA _editarDA;
    private readonly IObtenerUsuarioPorIdDA _obtenerPorIdDA;
    private readonly IObtenerRolDA _obtenerRolDA;
    private readonly IExisteNombreUsuarioDA _existeNombreDA;
    private readonly IExisteCorreoUsuarioDA _existeCorreoDA;

    public EditarUsuarioBL(
        IEditarUsuarioDA editarDA,
        IObtenerUsuarioPorIdDA obtenerPorIdDA,
        IObtenerRolDA obtenerRolDA,
        IExisteNombreUsuarioDA existeNombreDA,
        IExisteCorreoUsuarioDA existeCorreoDA)
    {
        _editarDA = editarDA;
        _obtenerPorIdDA = obtenerPorIdDA;
        _obtenerRolDA = obtenerRolDA;
        _existeNombreDA = existeNombreDA;
        _existeCorreoDA = existeCorreoDA;
    }

    public async Task<ResultadoOperacion> EditarAsync(UsuarioDto datos, int idUsuarioActual)
    {
        var usuario = await _obtenerPorIdDA.ObtenerPorIdAsync(datos.IdUsuario);
        if (usuario == null)
        {
            return ResultadoOperacion.Error(Mensajes.NoEncontrado);
        }

        if (usuario.Estado == Estados.Usuario.Inactivo)
        {
            return ResultadoOperacion.Error(Mensajes.UsuarioInactivoNoEditable);
        }

        ReglasUsuario.Normalizar(datos);

        var roles = (await _obtenerRolDA.ObtenerActivosAsync()).Select(r => r.Nombre);
        var error = ReglasUsuario.Validar(datos, roles);
        if (error != null)
        {
            return ResultadoOperacion.Error(error);
        }

        if (datos.IdUsuario == idUsuarioActual && datos.Rol != usuario.Rol)
        {
            return ResultadoOperacion.Error(Mensajes.AccionSobreUsuarioPropio);
        }

        if (await _existeNombreDA.ExisteNombreUsuarioAsync(datos.NombreUsuario, datos.IdUsuario))
        {
            return ResultadoOperacion.Error(Mensajes.NombreUsuarioDuplicado);
        }

        if (await _existeCorreoDA.ExisteCorreoAsync(datos.CorreoElectronico, datos.IdUsuario))
        {
            return ResultadoOperacion.Error(Mensajes.CorreoUsuarioDuplicado);
        }

        var resultado = await _editarDA.EditarAsync(datos);
        if (!resultado.Exito)
        {
            return resultado;
        }

        return ResultadoOperacion.Ok(Mensajes.UsuarioModificado, datos.IdUsuario);
    }
}
