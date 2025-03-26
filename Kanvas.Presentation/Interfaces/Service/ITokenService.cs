using Presentation.Identity;

namespace Presentation.Interfaces.Service;

public interface ITokenService
{
    string CreateToken(AppUser user);
    string CreateRefreshToken();
    string CreateInvitationToken();
}