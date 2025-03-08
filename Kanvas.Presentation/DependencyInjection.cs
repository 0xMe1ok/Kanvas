using Application;
using Infrastructure;

namespace Presentation;

public static class DependencyInjection
{
    public static IServiceCollection AddAppDI(this IServiceCollection services)
    {
        services.AddApplicationDI()
            .AddInfrastructureDI();
        
        // code here
        
        return services;
    }
}