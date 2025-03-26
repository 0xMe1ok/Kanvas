using Presentation.DTOs.Account;

namespace Presentation.Interfaces.Service;

public interface IAuthService
{
    Task<LoginUserDto> Register(RegisterDto dto);
    Task<LoginUserDto> Login(LoginDto dto);
    Task Logout();
    Task<TokensDto> RefreshToken(RefreshTokenDto refreshTokenDto);
    Task RevokeRefreshToken();
}