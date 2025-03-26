using Microsoft.EntityFrameworkCore;
using Presentation.Data;
using Presentation.Entities;
using Presentation.Interfaces;
using Presentation.Interfaces.Repository;

namespace Presentation.Repositories;

public class TeamMemberRepository : ITeamMemberRepository
{
    private readonly ApplicationDbContext _context;
    
    public TeamMemberRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<TeamMember>> GetAll(Guid teamId)
    {
        return await _context.TeamMembers.Where(tm => tm.TeamId == teamId).ToListAsync();
    }
    
    public async Task AddAsync(TeamMember teamMember)
    {
        await _context.TeamMembers.AddAsync(teamMember);
    }

    public void Remove(TeamMember teamMember)
    {
        _context.TeamMembers.Remove(teamMember);
    }
    
    public async Task RemoveAsync(Guid teamId, Guid userId)
    {
        await _context.TeamMembers
            .Where(t => t.TeamId == teamId && t.MemberId == userId)
            .ExecuteDeleteAsync();
    }

    public void Update(TeamMember teamMember)
    {
        _context.TeamMembers.Update(teamMember);
    }

    public async Task<TeamMember?> GetByIdAsync(Guid teamId, Guid userId)
    {
        return await _context.TeamMembers
            .FirstOrDefaultAsync(t => t.TeamId == teamId && t.MemberId == userId);
    }
    
    public async Task<bool> ExistsAsync(Guid teamId, Guid userId)
    {
        return await _context.TeamMembers
            .AnyAsync(t => t.TeamId == teamId && t.MemberId == userId);
    }

    public async Task<IEnumerable<Guid>> GetTeamsIdAsync(Guid memberId)
    {
        return await _context.TeamMembers
            .Where(tm => tm.MemberId == memberId)
            .Select(tm => tm.TeamId)
            .ToListAsync();
    }
}