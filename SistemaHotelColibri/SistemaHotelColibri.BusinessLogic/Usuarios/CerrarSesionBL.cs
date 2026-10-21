using SistemaHotelColibri.Abstract.Interfaces.BusinessLogic.Usuarios;
using SistemaHotelColibri.Abstract.Interfaces.DataAccess.Usuarios;

namespace SistemaHotelColibri.BusinessLogic.Usuarios;

public class CerrarSesionBL : ICerrarSesionBL
{
    private readonly ICerrarSesionDA _cerrarSesionDA;

    public CerrarSesionBL(ICerrarSesionDA cerrarSesionDA)
    {
        _cerrarSesionDA = cerrarSesionDA;
    }

    public Task CerrarSesionAsync() => _cerrarSesionDA.CerrarSesionAsync();
}
