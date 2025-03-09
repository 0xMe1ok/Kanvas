using Infrastructure;
using Microsoft.EntityFrameworkCore;

namespace Presentation;

public static class DependencyInjection
{
    public static IServiceCollection AddAppDI(this IServiceCollection services)
    {
        services.AddDbContext<ApplicationDbContext>(options => {
                options.UseNpgsql("Server=127.0.0.1;Port=5432;Database=aucura;Username=pguser;Password=secretpassword;");
            }
        );
        
        return services;
    }
}