using Presentation.DTOs.TaskBoard;
using Presentation.Entities;
using Presentation.Interfaces;

namespace Presentation.Services;

public class TaskBoardService : ITaskBoardService
{
    public Task<TaskBoard?> CreateNewBoard(CreateTaskBoardDto boardDto)
    {
        throw new NotImplementedException();
    }

    public Task<TaskBoard?> GetBoardAsync(Guid id)
    {
        throw new NotImplementedException();
    }

    public Task<IEnumerable<TaskBoard>> GetBoardAsync()
    {
        throw new NotImplementedException();
    }

    public Task UpdateBoardAsync(Guid id, UpdateTaskBoardDto boardDto)
    {
        throw new NotImplementedException();
    }

    public Task DeleteBoardAsync(Guid id)
    {
        throw new NotImplementedException();
    }

    public Task<bool> IsBoardExistsAsync(Guid id)
    {
        throw new NotImplementedException();
    }

    public Task<bool> IsBoardAccessibleAsync(Guid id)
    {
        throw new NotImplementedException();
    }
}