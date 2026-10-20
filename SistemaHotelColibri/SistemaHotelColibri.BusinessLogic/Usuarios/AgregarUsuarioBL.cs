using SistemaHotelColibri.Abstract.Comun;
using SistemaHotelColibri.Abstract.Interfaces.BusinessLogic.Usuarios;
using SistemaHotelColibri.Abstract.Interfaces.DataAccess.Usuarios;
using SistemaHotelColibri.Abstract.Modelos.Usuarios;

namespace SistemaHotelColibri.BusinessLogic.Usuarios;

public class AgregarUsuarioBL : IAgregarUsuarioBL
{
    private readonly IAgregarUsuarioDA _agregarDA;
    private readonly IObtenerRolDA _obtenerRolDA;
    private readonly IExisteNombreUsuarioDA _existeNombreDA;
    private readonly IExisteCorreoUsuarioDA _existeCorreoDA;

    public AgregarUsuarioBL(
        IAgregarUsuarioDA agregarDA,
        IObtenerRolDA obtenerRolDA,
        IExisteNombreUsuarioDA existeNombreDA,
        IExisteCorreoUsuarioDA existeCorreoDA)
    {
        _agregarDA = agregarDA;
        _obtenerRolDA = obtenerRolDA;
        _existeNombreDA = existeNombreDA;
        _existeCorreoDA = existeCorreoDA;
    }

    public async Task<ResultadoOperacion> AgregarAsync(UsuarioDto usuario, string contrasena)
    {
        ReglasUsuario.Normalizar(usuario);

        var roles = (await _obtenerRolDA.ObtenerActivosAsync()).Select(r => r.Nombre);
        var error = ReglasUsuario.Validar(usuario, roles);
        if (error != null)
        {
            return ResultadoOperacion.Error(error);
        }

        if (await _existeNombreDA.ExisteNombreUsuarioAsync(usuario.NombreUsuario, 0))
        {
            return ResultadoOperacion.Error(Mensajes.NombreUsuarioDuplicado);
        }

        if (await _existeCorreoDA.ExisteCorreoAsync(usuario.CorreoElectronico, 0))
        {
            return ResultadoOperacion.Error(Mensajes.CorreoUsuarioDuplicado);
        }

        var resultado = await _agregarDA.AgregarAsync(usuario, contrasena);
        if (!resultado.Exito)
        {
            return resultado;
        }

        return ResultadoOperacion.Ok(string.Format(Mensajes.UsuarioRegistrado, usuario.NombreUsuario), resultado.IdGenerado);
    }
}
