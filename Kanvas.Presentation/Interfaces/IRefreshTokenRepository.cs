using Presentation.Identity.Tokens;

namespace Presentation.Interfaces;

public interface IRefreshTokenRepository
{
    Task AddAsync(RefreshToken refreshToken);
}