using Presentation.Data;
using Presentation.Identity.Tokens;
using Presentation.Interfaces;

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
}