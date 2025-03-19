using Presentation.Identity;

namespace Presentation.Interfaces;

public interface ITokenService
{
    string CreateToken(AppUser user);
}