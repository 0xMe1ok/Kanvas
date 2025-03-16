using Microsoft.EntityFrameworkCore;
using Presentation.Entities;
using Presentation.Interfaces;

namespace Presentation.Repositories;

public class AppTaskRepository : Repository<AppTask>, IAppTaskRepository
{
    public AppTaskRepository(ApplicationDbContext context) : base(context)
    {
    }

    public async Task<int> ClearColumnIdInColumn(Guid columnId)
    {
        return await _context.AppTasks.Where(task => task.ColumnId == columnId)
             .ExecuteUpdateAsync(setters => setters
                 .SetProperty(t => t.ColumnId, t => null));
    }

    public async Task<int> ClearBoardIdInBoard(Guid boardId)
    {
        return await _context.AppTasks.Where(task => task.BoardId == boardId)
            .ExecuteUpdateAsync(setters => setters
                .SetProperty(t => t.ColumnId, t=> null)
                .SetProperty(t => t.BoardId, t => null));
    }
}