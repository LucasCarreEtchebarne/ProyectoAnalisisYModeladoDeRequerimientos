using SistemaHotelColibri.Abstract.Comun;
using SistemaHotelColibri.Abstract.Interfaces.BusinessLogic.Usuarios;
using SistemaHotelColibri.Abstract.Interfaces.DataAccess.Usuarios;

namespace SistemaHotelColibri.BusinessLogic.Usuarios;

public class BloquearUsuarioBL : IBloquearUsuarioBL
{
    private readonly IBloquearUsuarioDA _bloquearDA;

    public BloquearUsuarioBL(IBloquearUsuarioDA bloquearDA)
    {
        _bloquearDA = bloquearDA;
    }

    public async Task<ResultadoOperacion> BloquearAsync(int idUsuario, int idUsuarioActual)
    {
        if (idUsuario == idUsuarioActual)
        {
            return ResultadoOperacion.Error(Mensajes.AccionSobreUsuarioPropio);
        }

        return await _bloquearDA.BloquearAsync(idUsuario)
            ? ResultadoOperacion.Ok(Mensajes.UsuarioBloqueado, idUsuario)
            : ResultadoOperacion.Error(Mensajes.NoEncontrado);
    }

    public async Task<ResultadoOperacion> DesbloquearAsync(int idUsuario)
    {
        return await _bloquearDA.DesbloquearAsync(idUsuario)
            ? ResultadoOperacion.Ok(Mensajes.UsuarioDesbloqueado, idUsuario)
            : ResultadoOperacion.Error(Mensajes.NoEncontrado);
    }
}
