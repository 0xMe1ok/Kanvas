using System.Security.Claims;

namespace Presentation.Extensions;

public static class ClaimsExtensions
{
    public static string GetUsername(this ClaimsPrincipal user) {
        return user.Claims.SingleOrDefault(x => x.Type.Equals(ClaimTypes.GivenName)).Value;
    }
}