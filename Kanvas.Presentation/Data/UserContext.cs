using System.Security.Claims;
using Presentation.Exceptions;
using Presentation.Extensions;
using Presentation.Interfaces;

namespace Presentation.Data;

public sealed class UserContext(IHttpContextAccessor httpContextAccessor) : IUserContext
{
    public Guid UserId =>
        httpContextAccessor
            .HttpContext?
            .User
            .GetId() ??
        throw new NotFoundException("User does not exist.");
}