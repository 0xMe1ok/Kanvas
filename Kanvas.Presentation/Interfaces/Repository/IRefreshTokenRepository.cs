using Presentation.Identity.Tokens;

namespace Presentation.Interfaces.Repository;

public interface IRefreshTokenRepository
{
    Task AddAsync(RefreshToken refreshToken);
    Task<RefreshToken?> GetAsync(string refreshToken);
    
    Task RemoveAsync(Guid userId);
    
    Task CommitAsync();
}