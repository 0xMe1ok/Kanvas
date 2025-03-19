using Microsoft.EntityFrameworkCore;
using Presentation.Data;
using Presentation.Entities;
using Presentation.Interfaces;

namespace Presentation.Repositories;

public class TaskBoardRepository : Repository<TaskBoard>, ITaskBoardRepository
{
    public TaskBoardRepository(ApplicationDbContext context) : base(context)
    {
    }

    public override async Task<TaskBoard?> GetByIdAsync(Guid id)
    {
        return await _context.TaskBoards
            .Include(b => b.Columns)
            .Include(b => b.Tasks)
            .FirstOrDefaultAsync(b => b.Id == id);
    }
}