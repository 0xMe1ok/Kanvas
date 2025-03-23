using Microsoft.EntityFrameworkCore;
using Presentation.Data;
using Presentation.Entities;
using Presentation.Identity.Tokens;
using Presentation.Interfaces.Repository;

namespace Presentation.Repositories;

public class InviteTokenRepository : IInviteTokenRepository
{
    private readonly ApplicationDbContext _context;
    
    public InviteTokenRepository(ApplicationDbContext context)
    {
        _context = context;
    }
    
    public async Task AddAsync(InviteToken inviteToken)
    {
        _context.InviteTokens.Add(inviteToken);
        await _context.SaveChangesAsync();
    }

    public async Task<InviteToken?> GetAsync(string inviteToken)
    {
        return await _context.InviteTokens
            .Include(r => r.TeamId)
            .FirstOrDefaultAsync(r => r.Token == inviteToken);
    }

    public async Task RemoveAsync(string inviteToken)
    {
        await _context.InviteTokens
            .Where(r => r.Token == inviteToken)
            .ExecuteDeleteAsync();
    }
    
    // TODO: move to UoW
    public async Task CommitAsync()
    {
        await _context.SaveChangesAsync();
    }
}