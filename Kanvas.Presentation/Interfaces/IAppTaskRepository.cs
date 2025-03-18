using Presentation.Entities;

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
}