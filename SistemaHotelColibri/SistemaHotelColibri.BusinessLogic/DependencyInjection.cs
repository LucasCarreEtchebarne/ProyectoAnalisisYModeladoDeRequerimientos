using Microsoft.Extensions.DependencyInjection;
using SistemaHotelColibri.Abstract.Interfaces.BusinessLogic.Habitaciones;
using SistemaHotelColibri.Abstract.Interfaces.BusinessLogic.Inventarios;
using SistemaHotelColibri.BusinessLogic.Habitaciones;
using SistemaHotelColibri.BusinessLogic.Inventarios;

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

        services.AddScoped<IAgregarHabitacionBL, AgregarHabitacionBL>();
        services.AddScoped<IObtenerHabitacionBL, ObtenerHabitacionBL>();
        services.AddScoped<IObtenerHabitacionPorIdBL, ObtenerHabitacionPorIdBL>();
        services.AddScoped<IEditarHabitacionBL, EditarHabitacionBL>();
        services.AddScoped<IEliminarHabitacionBL, EliminarHabitacionBL>();

        return services;
    }
}
