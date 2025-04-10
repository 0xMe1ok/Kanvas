using Presentation.Data;
using Presentation.Enums;
using Presentation.Interfaces;
using Presentation.Interfaces.Service;

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
        var membership = await _unitOfWork.TeamMembers.GetByIdAsync(teamId, userId);

        if (membership == null)
        {
            return false;
        }

        return membership.Role >= requiredRole;
    }
}