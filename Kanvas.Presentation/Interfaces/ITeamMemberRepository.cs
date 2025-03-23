using Presentation.Entities;

namespace Presentation.Interfaces;

public interface ITeamMemberRepository
{
    Task<IEnumerable<TeamMember>> GetAll(Guid teamId);
    Task AddAsync(TeamMember teamMember);
    void Remove(TeamMember teamMember);
    Task RemoveAsync(Guid teamId, Guid userId);
    void Update(TeamMember teamMember);
    Task<TeamMember?> GetByIdAsync(Guid teamId, Guid userId);
    Task<bool> ExistsAsync(Guid teamId, Guid userId);
    
    Task<IEnumerable<Guid>> GetTeamsIdAsync(Guid memberId);
}