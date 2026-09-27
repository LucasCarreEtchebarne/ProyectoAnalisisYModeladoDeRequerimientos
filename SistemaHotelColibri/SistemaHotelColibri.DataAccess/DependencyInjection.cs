using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using SistemaHotelColibri.DataAccess.Contexto;
using SistemaHotelColibri.DataAccess.Identidad;

namespace SistemaHotelColibri.DataAccess;

public static class DependencyInjection
{
    public static IServiceCollection AddDataAccess(this IServiceCollection services, string cadenaConexion)
    {
        services.AddDbContext<HotelColibriContext>(opciones => opciones.UseSqlServer(cadenaConexion));
        services.AddDbContext<IdentidadContext>(opciones => opciones.UseSqlServer(cadenaConexion));

        return services;
    }
}
