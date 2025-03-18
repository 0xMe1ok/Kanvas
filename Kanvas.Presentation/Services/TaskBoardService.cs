using AutoMapper;
using Presentation.DTOs.TaskBoard;
using Presentation.Entities;
using Presentation.Exceptions;
using Presentation.Interfaces;

namespace Presentation.Services;

public class TaskBoardService : ITaskBoardService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public TaskBoardService(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }
    public async Task<TaskBoard?> CreateNewBoard(CreateTaskBoardDto boardDto)
    {
        throw new NotImplementedException();
    }

    public async Task<TaskBoard?> GetBoardAsync(Guid boardId)
    {
        var board = await _unitOfWork.Boards.GetByIdAsync(boardId);
        if (board == null) throw new NotFoundException("Board is not found");
        return board;
    }

    public Task<IEnumerable<TaskBoard>> GetBoardsAsync()
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