using Microsoft.Extensions.DependencyInjection;
using SistemaHotelColibri.Abstract.Interfaces.BusinessLogic.Clientes;
using SistemaHotelColibri.Abstract.Interfaces.BusinessLogic.Eventos;
using SistemaHotelColibri.BusinessLogic.Eventos;
using SistemaHotelColibri.Abstract.Interfaces.BusinessLogic.Habitaciones;
using SistemaHotelColibri.Abstract.Interfaces.BusinessLogic.Inventarios;
using SistemaHotelColibri.Abstract.Interfaces.BusinessLogic.Reservas;
using SistemaHotelColibri.Abstract.Interfaces.BusinessLogic.Usuarios;
using SistemaHotelColibri.BusinessLogic.Reservas;
using SistemaHotelColibri.BusinessLogic.Clientes;
using SistemaHotelColibri.BusinessLogic.Habitaciones;
using SistemaHotelColibri.BusinessLogic.Inventarios;
using SistemaHotelColibri.BusinessLogic.Usuarios;

namespace SistemaHotelColibri.BusinessLogic;

public static class DependencyInjection
{
    public static IServiceCollection AddBusinessLogic(this IServiceCollection services)
    {
        services.AddScoped<IAgregarInventarioBL, AgregarInventarioBL>();
        services.AddScoped<IObtenerInventarioBL, ObtenerInventarioBL>();
        services.AddScoped<IObtenerInventarioPorIdBL, ObtenerInventarioPorIdBL>();
        services.AddScoped<IEditarInventarioBL, EditarInventarioBL>();
        services.AddScoped<IEliminarInventarioBL, EliminarInventarioBL>();
        services.AddScoped<IObtenerAlertasInventarioBL, ObtenerAlertasInventarioBL>();
        services.AddScoped<IObtenerCategoriaInventarioBL, ObtenerCategoriaInventarioBL>();

        services.AddScoped<IAgregarHabitacionBL, AgregarHabitacionBL>();
        services.AddScoped<IObtenerHabitacionBL, ObtenerHabitacionBL>();
        services.AddScoped<IObtenerHabitacionPorIdBL, ObtenerHabitacionPorIdBL>();
        services.AddScoped<IEditarHabitacionBL, EditarHabitacionBL>();
        services.AddScoped<IEliminarHabitacionBL, EliminarHabitacionBL>();

        services.AddScoped<IAgregarClienteBL, AgregarClienteBL>();
        services.AddScoped<IObtenerClienteBL, ObtenerClienteBL>();
        services.AddScoped<IObtenerClientePorIdBL, ObtenerClientePorIdBL>();
        services.AddScoped<IEditarClienteBL, EditarClienteBL>();
        services.AddScoped<IEliminarClienteBL, EliminarClienteBL>();
        services.AddScoped<IBuscarClienteBL, BuscarClienteBL>();

        services.AddScoped<IAgregarReservaBL, AgregarReservaBL>();
        services.AddScoped<IObtenerReservaBL, ObtenerReservaBL>();
        services.AddScoped<IObtenerReservaPorIdBL, ObtenerReservaPorIdBL>();
        services.AddScoped<IEditarReservaBL, EditarReservaBL>();
        services.AddScoped<IEliminarReservaBL, EliminarReservaBL>();
        services.AddScoped<IObtenerOpcionesReservaBL, ObtenerOpcionesReservaBL>();

        services.AddScoped<IAgregarEventoBL, AgregarEventoBL>();
        services.AddScoped<IObtenerEventoBL, ObtenerEventoBL>();
        services.AddScoped<IObtenerEventoPorIdBL, ObtenerEventoPorIdBL>();
        services.AddScoped<IEditarEventoBL, EditarEventoBL>();
        services.AddScoped<IEliminarEventoBL, EliminarEventoBL>();
        services.AddScoped<IObtenerEspacioEventoBL, ObtenerEspacioEventoBL>();

        services.AddScoped<IIniciarSesionBL, IniciarSesionBL>();
        services.AddScoped<ICerrarSesionBL, CerrarSesionBL>();
        services.AddScoped<IAgregarUsuarioBL, AgregarUsuarioBL>();
        services.AddScoped<IObtenerUsuarioBL, ObtenerUsuarioBL>();
        services.AddScoped<IObtenerUsuarioPorIdBL, ObtenerUsuarioPorIdBL>();
        services.AddScoped<IEditarUsuarioBL, EditarUsuarioBL>();
        services.AddScoped<IEliminarUsuarioBL, EliminarUsuarioBL>();
        services.AddScoped<IActivarUsuarioBL, ActivarUsuarioBL>();
        services.AddScoped<IBloquearUsuarioBL, BloquearUsuarioBL>();
        services.AddScoped<IRestablecerContrasenaUsuarioBL, RestablecerContrasenaUsuarioBL>();
        services.AddScoped<ICambiarContrasenaBL, CambiarContrasenaBL>();
        services.AddScoped<IRecuperarContrasenaBL, RecuperarContrasenaBL>();
        services.AddScoped<IObtenerRolBL, ObtenerRolBL>();

        return services;
    }
}