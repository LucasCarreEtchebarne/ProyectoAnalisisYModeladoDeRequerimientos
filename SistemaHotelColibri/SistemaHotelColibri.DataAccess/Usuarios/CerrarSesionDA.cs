using Microsoft.AspNetCore.Identity;
using SistemaHotelColibri.Abstract.Interfaces.DataAccess.Usuarios;
using SistemaHotelColibri.DataAccess.Identidad;

namespace SistemaHotelColibri.DataAccess.Usuarios;

public class CerrarSesionDA : ICerrarSesionDA
{
    private readonly SignInManager<Usuario> _gestorSesion;

    public CerrarSesionDA(SignInManager<Usuario> gestorSesion)
    {
        _gestorSesion = gestorSesion;
    }

    public Task CerrarSesionAsync() => _gestorSesion.SignOutAsync();
}
