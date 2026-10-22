using SistemaHotelColibri.Abstract.Interfaces.BusinessLogic.Usuarios;
using SistemaHotelColibri.Abstract.Interfaces.DataAccess.Usuarios;
using SistemaHotelColibri.Abstract.Modelos.Usuarios;

namespace SistemaHotelColibri.BusinessLogic.Usuarios;

public class ObtenerUsuarioPorIdBL : IObtenerUsuarioPorIdBL
{
    private readonly IObtenerUsuarioPorIdDA _obtenerPorIdDA;

    public ObtenerUsuarioPorIdBL(IObtenerUsuarioPorIdDA obtenerPorIdDA)
    {
        _obtenerPorIdDA = obtenerPorIdDA;
    }

    public Task<UsuarioDto?> ObtenerPorIdAsync(int idUsuario) => _obtenerPorIdDA.ObtenerPorIdAsync(idUsuario);
}
