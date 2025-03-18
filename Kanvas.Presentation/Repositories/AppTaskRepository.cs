using Microsoft.EntityFrameworkCore;
using Presentation.Entities;
using Presentation.Interfaces;

namespace Presentation.Repositories;

public class AppTaskRepository : Repository<AppTask>, IAppTaskRepository
{
    public AppTaskRepository(ApplicationDbContext context) : base(context)
    {
    }

    public async Task<int> ClearColumnIdInColumnAsync(Guid columnId)
    {
        return await _context.AppTasks.Where(task => task.ColumnId == columnId)
             .ExecuteUpdateAsync(setters => setters
                 .SetProperty(t => t.ColumnId, t => null));
    }

    public async Task<int> ClearBoardIdInBoardAsync(Guid boardId)
    {
        return await _context.AppTasks.Where(task => task.BoardId == boardId)
            .ExecuteUpdateAsync(setters => setters
                .SetProperty(t => t.ColumnId, t=> null)
                .SetProperty(t => t.BoardId, t => null));
    }

    public async Task<int> GetMaxOrderInColumnAsync(Guid columnId)
    {
        return await _context.AppTasks
            .Where(t => t.ColumnId == columnId)
            .MaxAsync(t => (int?)t.Order) ?? 0;
    }
    
    public async Task ShiftTasksOrderAsync(
        Guid columnId,
        int startOrder,
        int endOrder,
        int shiftBy)
    {
        await _context.AppTasks
            .Where(t => t.ColumnId == columnId)
            .Where(t => t.Order >= startOrder)
            .Where(t => t.Order <= endOrder)
            .ExecuteUpdateAsync(setters => 
                setters.SetProperty(
                    t => t.Order,
                    t => t.Order + shiftBy));
    }
}