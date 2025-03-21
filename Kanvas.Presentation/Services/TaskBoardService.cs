using AutoMapper;
using Presentation.DTOs.TaskBoard;
using Presentation.Entities;
using Presentation.Enums;
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
    public async Task<TaskBoard> CreateNewBoard(CreateTaskBoardDto boardDto)
    {
        // TODO: only from selected and accessible team
        if (!await _unitOfWork.Teams.ExistsAsync(boardDto.TeamId))
        {
            throw new NotFoundException($"Team with id {boardDto.TeamId} does not exist.");
        }
        
        var board = _mapper.Map<TaskBoard>(boardDto);
        await _unitOfWork.Boards.AddAsync(board);

        var columnsStarterPack = new List<BoardColumn>
        {
            new BoardColumn
            {
                BoardId = board.Id, Name = "ToDo", Order = 1, Status = Status.ToDo, TaskLimit = null
            },
            new BoardColumn
            {
                BoardId = board.Id, Name = "InProgress", Order = 2, Status = Status.InProgress, TaskLimit = null
            },
            new BoardColumn
            {
                BoardId = board.Id, Name = "Done", Order = 3, Status = Status.Done, TaskLimit = null
            },
        };
        
        await _unitOfWork.Columns.AddRangeAsync(columnsStarterPack);
        await _unitOfWork.CommitAsync();

        return board;
    }

    public async Task<TaskBoard?> GetBoardAsync(Guid boardId)
    {
        // TODO: only from selected and accessible team, for admins/redactors
        var board = await _unitOfWork.Boards.GetByIdAsync(boardId);
        if (board == null) throw new NotFoundException("Board is not found");
        return board;
    }

    public async Task<IEnumerable<TaskBoard>> GetBoardsAsync()
    {
        // TODO: only from selected and accessible team, for admins/redactors
        return await _unitOfWork.Boards.GetAllAsync();
    }

    public async Task UpdateBoardAsync(Guid id, UpdateTaskBoardDto boardDto)
    {
        var board = await _unitOfWork.Boards.GetByIdAsync(id);
        if (board == null) throw new NotFoundException("Board is not found");
        _mapper.Map(boardDto, board);
        await _unitOfWork.CommitAsync();
    }

    public async Task DeleteBoardAsync(Guid id)
    {
        var board = await _unitOfWork.Boards.GetByIdAsync(id);
        if (board == null) throw new NotFoundException("Board is not found");

        await _unitOfWork.Tasks.ClearBoardIdInBoardAsync(id);
        _unitOfWork.Boards.Remove(board);
        await _unitOfWork.CommitAsync();

    }
}