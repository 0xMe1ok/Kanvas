using Presentation.Data;
using Presentation.Enums;
using Presentation.Interfaces;

namespace Presentation.Services;

public class TeamRoleService : ITeamRoleService
{
    private readonly IUnitOfWork _unitOfWork;

    public TeamRoleService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<bool> IsInTeamRoleOrHigherAsync(Guid userId, Guid teamId, TeamRole requiredRole)
    {
        var membership = await _unitOfWork.TeamMembers.GetByIdAsync(userId, teamId);

        if (membership == null)
        {
            return false;
        }

        return membership.Role >= requiredRole;
    }
}