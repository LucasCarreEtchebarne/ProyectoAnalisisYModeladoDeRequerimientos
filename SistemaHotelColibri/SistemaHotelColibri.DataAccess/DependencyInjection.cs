using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using SistemaHotelColibri.Abstract.Interfaces.DataAccess.Clientes;
using SistemaHotelColibri.Abstract.Interfaces.DataAccess.Habitaciones;
using SistemaHotelColibri.Abstract.Interfaces.DataAccess.Inventarios;
using SistemaHotelColibri.DataAccess.Clientes;
using SistemaHotelColibri.DataAccess.Contexto;
using SistemaHotelColibri.DataAccess.Habitaciones;
using SistemaHotelColibri.DataAccess.Identidad;
using SistemaHotelColibri.DataAccess.Inventarios;

namespace SistemaHotelColibri.DataAccess;

public static class DependencyInjection
{
    public static IServiceCollection AddDataAccess(this IServiceCollection services, string cadenaConexion)
    {
        services.AddDbContext<HotelColibriContext>(opciones => opciones.UseSqlServer(cadenaConexion));
        services.AddDbContext<IdentidadContext>(opciones => opciones.UseSqlServer(cadenaConexion));

        services.AddScoped<IAgregarInventarioDA, AgregarInventarioDA>();
        services.AddScoped<IExisteNombreInventarioDA, ExisteNombreInventarioDA>();
        services.AddScoped<IObtenerInventarioDA, ObtenerInventarioDA>();
        services.AddScoped<IObtenerInventarioPorIdDA, ObtenerInventarioPorIdDA>();
        services.AddScoped<IEditarInventarioDA, EditarInventarioDA>();
        services.AddScoped<IEliminarInventarioDA, EliminarInventarioDA>();
        services.AddScoped<IObtenerAlertasInventarioDA, ObtenerAlertasInventarioDA>();

        services.AddScoped<IAgregarHabitacionDA, AgregarHabitacionDA>();
        services.AddScoped<IExisteNumeroHabitacionDA, ExisteNumeroHabitacionDA>();
        services.AddScoped<IObtenerHabitacionDA, ObtenerHabitacionDA>();
        services.AddScoped<IObtenerHabitacionPorIdDA, ObtenerHabitacionPorIdDA>();
        services.AddScoped<IEditarHabitacionDA, EditarHabitacionDA>();
        services.AddScoped<IEliminarHabitacionDA, EliminarHabitacionDA>();

        services.AddScoped<IAgregarClienteDA, AgregarClienteDA>();
        services.AddScoped<IExisteIdentificacionClienteDA, ExisteIdentificacionClienteDA>();
        services.AddScoped<IObtenerClienteDA, ObtenerClienteDA>();
        services.AddScoped<IObtenerClientePorIdDA, ObtenerClientePorIdDA>();
        services.AddScoped<IEditarClienteDA, EditarClienteDA>();
        services.AddScoped<IEliminarClienteDA, EliminarClienteDA>();
        services.AddScoped<IBuscarClienteDA, BuscarClienteDA>();

        return services;
    }
}