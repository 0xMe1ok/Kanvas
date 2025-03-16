using Presentation.Entities;

namespace Presentation.Interfaces;

public interface IAppTaskRepository : IRepository<AppTask>
{
    Task<int> ClearColumnIdInColumn(Guid columnId);
    Task<int> ClearBoardIdInBoard(Guid boardId);
}