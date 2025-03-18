using Presentation.Entities;
using Presentation.Enums;

namespace Presentation.Interfaces;

public interface IAppTaskRepository : IRepository<AppTask>
{
    Task<int> ClearColumnIdInColumnAsync(Guid columnId);
    Task<int> ClearBoardIdInBoardAsync(Guid boardId);
    
    Task<int> GetMaxOrderInColumnAsync(Guid columnId);

    Task ShiftTasksOrderAsync(
        Guid columnId,
        int startOrder,
        int endOrder,
        int shiftBy);
    
    Task SetColumnForTaskInBoardAsync(Guid boardId, Status status, Guid newColumnId);
}