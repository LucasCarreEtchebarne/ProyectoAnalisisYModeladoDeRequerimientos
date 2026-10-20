using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SistemaHotelColibri.Abstract.Interfaces.DataAccess.Clientes;
using SistemaHotelColibri.Abstract.Interfaces.DataAccess.Correo;
using SistemaHotelColibri.Abstract.Interfaces.DataAccess.Habitaciones;
using SistemaHotelColibri.Abstract.Interfaces.DataAccess.Inventarios;
using SistemaHotelColibri.Abstract.Interfaces.DataAccess.Usuarios;
using SistemaHotelColibri.DataAccess.Clientes;
using SistemaHotelColibri.DataAccess.Contexto;
using SistemaHotelColibri.DataAccess.Correo;
using SistemaHotelColibri.DataAccess.Habitaciones;
using SistemaHotelColibri.DataAccess.Identidad;
using SistemaHotelColibri.DataAccess.Inventarios;
using SistemaHotelColibri.DataAccess.Usuarios;

namespace SistemaHotelColibri.DataAccess;

public static class DependencyInjection
{
    public static IServiceCollection AddDataAccess(this IServiceCollection services, IConfiguration configuracion)
    {
        var cadenaConexion = configuracion.GetConnectionString("HotelColibri")
            ?? throw new InvalidOperationException("Falta la cadena de conexión 'ConnectionStrings:HotelColibri'.");

        services.AddDbContext<HotelColibriContext>(opciones => opciones.UseSqlServer(cadenaConexion));
        services.AddDbContext<IdentidadContext>(opciones => opciones.UseSqlServer(cadenaConexion));

        services.AddIdentity<Usuario, Rol>(opciones =>
            {
                opciones.User.RequireUniqueEmail = true;
                opciones.Password.RequiredLength = 8;
                opciones.Password.RequireDigit = true;
                opciones.Password.RequireUppercase = true;
                opciones.Password.RequireLowercase = true;
                opciones.Password.RequireNonAlphanumeric = false;
                opciones.Lockout.AllowedForNewUsers = true;
                opciones.Lockout.MaxFailedAccessAttempts = 5;
                opciones.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(15);
                opciones.SignIn.RequireConfirmedAccount = false;
            })
            .AddEntityFrameworkStores<IdentidadContext>()
            .AddDefaultTokenProviders()
            .AddErrorDescriber<ErroresIdentidadEspanol>()
            .AddClaimsPrincipalFactory<FabricaReclamosUsuario>();

        services.Configure<SecurityStampValidatorOptions>(opciones => opciones.ValidationInterval = TimeSpan.FromMinutes(1));
        services.Configure<DataProtectionTokenProviderOptions>(opciones => opciones.TokenLifespan = TimeSpan.FromHours(2));

        var seccionCorreo = configuracion.GetSection(ConfiguracionCorreo.Seccion);
        services.Configure<ConfiguracionCorreo>(seccionCorreo);
        if (seccionCorreo.Get<ConfiguracionCorreo>()?.EstaConfigurado == true)
        {
            services.AddScoped<IEnviarCorreoDA, EnviarCorreoSmtpDA>();
        }
        else
        {
            services.AddScoped<IEnviarCorreoDA, EnviarCorreoRegistroDA>();
        }

        services.AddScoped<IAgregarInventarioDA, AgregarInventarioDA>();
        services.AddScoped<IExisteNombreInventarioDA, ExisteNombreInventarioDA>();
        services.AddScoped<IObtenerInventarioDA, ObtenerInventarioDA>();
        services.AddScoped<IObtenerInventarioPorIdDA, ObtenerInventarioPorIdDA>();
        services.AddScoped<IEditarInventarioDA, EditarInventarioDA>();
        services.AddScoped<IEliminarInventarioDA, EliminarInventarioDA>();
        services.AddScoped<IObtenerAlertasInventarioDA, ObtenerAlertasInventarioDA>();
        services.AddScoped<IObtenerCategoriaInventarioDA, ObtenerCategoriaInventarioDA>();

        services.AddScoped<IAgregarHabitacionDA, AgregarHabitacionDA>();
        services.AddScoped<IExisteNumeroHabitacionDA, ExisteNumeroHabitacionDA>();
        services.AddScoped<IObtenerHabitacionDA, ObtenerHabitacionDA>();
        services.AddScoped<IObtenerHabitacionPorIdDA, ObtenerHabitacionPorIdDA>();
        services.AddScoped<IEditarHabitacionDA, EditarHabitacionDA>();
        services.AddScoped<IEliminarHabitacionDA, EliminarHabitacionDA>();

        services.AddScoped<IAgregarClienteDA, AgregarClienteDA>();
        services.AddScoped<IExisteIdentificacionClienteDA, ExisteIdentificacionClienteDA>();
        services.AddScoped<IExisteCorreoClienteDA, ExisteCorreoClienteDA>();
        services.AddScoped<IObtenerClienteDA, ObtenerClienteDA>();
        services.AddScoped<IObtenerClientePorIdDA, ObtenerClientePorIdDA>();
        services.AddScoped<IEditarClienteDA, EditarClienteDA>();
        services.AddScoped<IEliminarClienteDA, EliminarClienteDA>();
        services.AddScoped<IBuscarClienteDA, BuscarClienteDA>();

        services.AddScoped<IIniciarSesionDA, IniciarSesionDA>();
        services.AddScoped<ICerrarSesionDA, CerrarSesionDA>();
        services.AddScoped<IAgregarUsuarioDA, AgregarUsuarioDA>();
        services.AddScoped<IObtenerUsuarioDA, ObtenerUsuarioDA>();
        services.AddScoped<IObtenerUsuarioPorIdDA, ObtenerUsuarioPorIdDA>();
        services.AddScoped<IEditarUsuarioDA, EditarUsuarioDA>();
        services.AddScoped<IEliminarUsuarioDA, EliminarUsuarioDA>();
        services.AddScoped<IActivarUsuarioDA, ActivarUsuarioDA>();
        services.AddScoped<IBloquearUsuarioDA, BloquearUsuarioDA>();
        services.AddScoped<IRestablecerContrasenaUsuarioDA, RestablecerContrasenaUsuarioDA>();
        services.AddScoped<ICambiarContrasenaDA, CambiarContrasenaDA>();
        services.AddScoped<IRecuperarContrasenaDA, RecuperarContrasenaDA>();
        services.AddScoped<IObtenerRolDA, ObtenerRolDA>();
        services.AddScoped<IExisteNombreUsuarioDA, ExisteNombreUsuarioDA>();
        services.AddScoped<IExisteCorreoUsuarioDA, ExisteCorreoUsuarioDA>();

        return services;
    }
}
