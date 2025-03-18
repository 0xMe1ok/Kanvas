using Presentation.DTOs.Team;
using Presentation.Entities;
using Presentation.Interfaces;

namespace Presentation.Services;

public class AppTeamService : IAppTeamService
{
    public Task<AppTeam?> CreateNewTeam(CreateAppTeamDto teamDto)
    {
        throw new NotImplementedException();
    }

    public Task<AppTeam?> GetTeamAsync(Guid id)
    {
        throw new NotImplementedException();
    }

    public Task<IEnumerable<AppTeam>> GetTeamsAsync()
    {
        throw new NotImplementedException();
    }

    public Task UpdateTeamAsync(Guid id, UpdateAppTeamDto teamDto)
    {
        throw new NotImplementedException();
    }

    public Task DeleteTeamAsync(Guid id)
    {
        throw new NotImplementedException();
    }

    public Task<bool> IsTeamExistsAsync(Guid id)
    {
        throw new NotImplementedException();
    }
}