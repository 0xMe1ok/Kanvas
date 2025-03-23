using Microsoft.EntityFrameworkCore;
using Presentation.Data;
using Presentation.Identity.Tokens;
using Presentation.Interfaces;
using Presentation.Interfaces.Repository;

namespace Presentation.Repositories;

public class RefreshTokenRepository : IRefreshTokenRepository
{
    private readonly ApplicationDbContext _context;
    
    public RefreshTokenRepository(ApplicationDbContext context)
    {
        _context = context;
    }
    
    public async Task AddAsync(RefreshToken refreshToken)
    {
        _context.RefreshTokens.Add(refreshToken);
        await _context.SaveChangesAsync();
    }

    public async Task<RefreshToken?> GetAsync(string refreshToken)
    {
        return await _context.RefreshTokens
            .Include(r => r.UserId)
            .FirstOrDefaultAsync(r => r.Token == refreshToken);
    }

    public async Task RemoveAsync(Guid userId)
    {
        await _context.RefreshTokens
            .Where(r => r.UserId == userId)
            .ExecuteDeleteAsync();
    }
    
    // TODO: move to UoW
    public async Task CommitAsync()
    {
        await _context.SaveChangesAsync();
    }
}