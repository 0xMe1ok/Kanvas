using Presentation.DTOs.Team;
using Presentation.Entities;

namespace Presentation.Interfaces;

public interface IAppTeamService
{
    Task<AppTeam?> CreateNewTeam(CreateAppTeamDto teamDto);
    Task<AppTeam?> GetTeamAsync(Guid id);
    
    Task<IEnumerable<AppTeam>> GetTeamsAsync();
    
    Task UpdateTeamAsync(Guid id, UpdateAppTeamDto teamDto);
    Task DeleteTeamAsync(Guid id);
    
    Task<bool> IsTeamExistsAsync(Guid id);
}