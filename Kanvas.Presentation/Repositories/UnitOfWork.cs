using Microsoft.EntityFrameworkCore;
using Presentation.Interfaces;

namespace Presentation.Repositories;

public class UnitOfWork : IUnitOfWork
{
    private readonly ApplicationDbContext _context;
    
    public UnitOfWork(ApplicationDbContext context,
        IAppTeamRepository appTeamRepository,
        ITaskBoardRepository taskBoardRepository,
        IBoardColumnRepository boardColumnRepository,
        IAppTaskRepository appTaskRepository)
    {
        _context = context;
        Boards = taskBoardRepository;
        Tasks = appTaskRepository;
        Columns = boardColumnRepository;
        Teams = appTeamRepository;
    }
    
    public IAppTeamRepository Teams { get; }
    public IAppTaskRepository Tasks { get; }
    public IBoardColumnRepository Columns { get; }
    public ITaskBoardRepository Boards { get; }

    public async Task<int> CommitAsync()
    {
        return await _context.SaveChangesAsync();
    }

    public Task RollbackAsync()
    {
        _context.ChangeTracker.Entries()
            .ToList()
            .ForEach(e => e.State = EntityState.Unchanged);
        
        return Task.CompletedTask;
    }
    
    public void Dispose()
    {
        _context.Dispose();
    }
}