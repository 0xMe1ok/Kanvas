using Presentation.Entities;

namespace Presentation.Interfaces;

public interface ITeamMemberRepository
{
    Task<IEnumerable<TeamMember>> GetAllInTeamAsync(Guid teamId);
    Task AddTeamMemberAsync(TeamMember teamMember);
    void RemoveTeamMemberAsync(TeamMember teamMember);
    void UpdateTeamMemberAsync(TeamMember teamMember);
}