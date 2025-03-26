using Presentation.Enums;

namespace Presentation.Interfaces.Service;

public interface ITeamRoleService
{
    Task<bool> IsInTeamRoleOrHigherAsync(Guid userId, Guid teamId, TeamRole requiredRole);
}