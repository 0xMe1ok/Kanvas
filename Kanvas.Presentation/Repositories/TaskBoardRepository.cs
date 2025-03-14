using Presentation.Entities;
using Presentation.Interfaces;

namespace Presentation.Repositories;

public class TaskBoardRepository : Repository<TaskBoard>, ITaskBoardRepository
{
    public TaskBoardRepository(ApplicationDbContext context) : base(context)
    {
    }
}