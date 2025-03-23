using Presentation.Enums;

namespace Presentation.Interfaces;

public interface ITeamRoleService
{
    Task<bool> IsInTeamRoleOrHigherAsync(Guid userId, Guid teamId, TeamRole requiredRole);
}