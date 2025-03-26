using System.Text.Json.Serialization;
using Asp.Versioning;
using Presentation.Mapper;
using Microsoft.EntityFrameworkCore;
using Presentation.Data;
using Presentation.Identity;
using Presentation.Interfaces;
using Presentation.Interfaces.Repository;
using Presentation.Interfaces.Service;
using Presentation.Repositories;
using Presentation.Services;

namespace Presentation;

public static class DependencyInjection
{
    public static IServiceCollection AddAppDi(this IServiceCollection services, IConfiguration configuration)
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
                options.UseNpgsql(configuration.GetConnectionString("DefaultConnection"));
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
        services.AddScoped<IRefreshTokenRepository, RefreshTokenRepository>();
        services.AddScoped<IInviteTokenRepository, InviteTokenRepository>();

        services.AddScoped<IAppTaskService, AppTaskService>();
        services.AddScoped<IAppTeamService, AppTeamService>();
        services.AddScoped<IBoardColumnService, BoardColumnService>();
        services.AddScoped<ITaskBoardService, TaskBoardService>();
        services.AddScoped<ITeamMemberRepository, TeamMemberRepository>();
        
        services.AddScoped<ITokenService, TokenService>();
        
        services.AddHttpContextAccessor();
        services.AddScoped<IUserContext, UserContext>();
        services.AddScoped<ITeamRoleService, TeamRoleService>();

        services.AddScoped<IUserContext, UserContext>();

        services.AddAutoMapper(typeof(AutomapperConfig).Assembly);
        
        return services;
    }
}