using Microsoft.Extensions.DependencyInjection;
using SistemaHotelColibri.Abstract.Interfaces.BusinessLogic.Inventarios;
using SistemaHotelColibri.BusinessLogic.Inventarios;

namespace SistemaHotelColibri.BusinessLogic;

public static class DependencyInjection
{
    public static IServiceCollection AddBusinessLogic(this IServiceCollection services)
    {
        services.AddScoped<IAgregarInventarioBL, AgregarInventarioBL>();

        return services;
    }
}
