using Microsoft.EntityFrameworkCore.Storage;
using Presentation.Interfaces.Repository;

namespace Presentation.Interfaces;

public interface IUnitOfWork : IDisposable
{
    IAppTeamRepository Teams { get; }
    IAppTaskRepository Tasks { get; }
    IBoardColumnRepository Columns { get; }
    ITaskBoardRepository Boards { get; }
    
    ITeamMemberRepository TeamMembers { get; }
    Task<int> CommitAsync();
    Task RollbackAsync();
    
    Task<IDbContextTransaction> BeginTransactionAsync();
}
