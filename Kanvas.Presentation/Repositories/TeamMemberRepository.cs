using Microsoft.EntityFrameworkCore;
using Presentation.Data;
using Presentation.Entities;
using Presentation.Interfaces;

namespace Presentation.Repositories;

public class TeamMemberRepository : ITeamMemberRepository
{
    private readonly ApplicationDbContext _context;
    
    public TeamMemberRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<TeamMember>> GetAllInTeamAsync(Guid teamId)
    {
        return await _context.TeamMembers.Where(tm => tm.TeamId == teamId).ToListAsync();
    }
    
    public async Task AddTeamMemberAsync(TeamMember teamMember)
    {
        await _context.TeamMembers.AddAsync(teamMember);
    }

    public void RemoveTeamMemberAsync(TeamMember teamMember)
    {
        _context.TeamMembers.Remove(teamMember);
    }

    public void UpdateTeamMemberAsync(TeamMember teamMember)
    {
        _context.TeamMembers.Update(teamMember);
    }
}