using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using SistemaHotelColibri.Abstract.Interfaces.DataAccess.Inventarios;
using SistemaHotelColibri.DataAccess.Contexto;
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

        return services;
    }
}
