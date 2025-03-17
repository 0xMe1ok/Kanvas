using System.Text.Json.Serialization;
using Asp.Versioning;
using Presentation;
using Presentation.Mapper;
using Microsoft.EntityFrameworkCore;
using Presentation.Exceptions;
using Presentation.Interfaces;
using Presentation.Repositories;
using Presentation.Services;

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
        
        services.AddApiVersioning(options =>
        {
            options.DefaultApiVersion = new ApiVersion(1, 0);
            options.AssumeDefaultVersionWhenUnspecified = true;
            options.ReportApiVersions = true;
        })
        .AddApiExplorer(options =>
        {
            options.GroupNameFormat = "'v'VVV";
            options.SubstituteApiVersionInUrl = true;
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

        services.AddScoped<IUnitOfWork, UnitOfWork>();
        services.AddScoped<IAppTaskRepository, AppTaskRepository>();
        services.AddScoped<IAppTeamRepository, AppTeamRepository>();
        services.AddScoped<IBoardColumnRepository, BoardColumnRepository>();
        services.AddScoped<ITaskBoardRepository, TaskBoardRepository>();

        services.AddScoped<IAppTaskService, AppTaskService>();

        services.AddAutoMapper(typeof(AutomapperConfig).Assembly);
        
        return services;
    }
}