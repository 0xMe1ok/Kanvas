using System.Text.Json.Serialization;
using Presentation;
using Presentation.Mapper;
using Microsoft.EntityFrameworkCore;

namespace Presentation;

public static class DependencyInjection
{
    public static IServiceCollection AddAppDI(this IServiceCollection services)
    {
        services.AddControllers()
            .AddJsonOptions(options =>
            {
                options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
            });
        
        services.AddDbContext<ApplicationDbContext>(options => {
                options.UseNpgsql("Server=127.0.0.1;Port=5432;Database=kanvasdb;Username=pguser;Password=secretpassword;");
            }
        );
        
        services.AddControllers().AddJsonOptions(options =>
        {
            options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());

            options.JsonSerializerOptions.DefaultIgnoreCondition =
                JsonIgnoreCondition.WhenWritingNull;
        });

        services.AddAutoMapper(typeof(AutomapperConfig).Assembly);
        
        return services;
    }
}