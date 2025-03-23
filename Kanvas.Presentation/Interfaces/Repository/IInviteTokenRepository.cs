using Presentation.Entities;

namespace Presentation.Interfaces.Repository;

public interface IInviteTokenRepository
{
    Task AddAsync(InviteToken inviteToken);
    Task<InviteToken?> GetAsync(string inviteToken);
    Task RemoveAsync(string inviteToken);
    Task CommitAsync();
}