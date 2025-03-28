using AutoMapper;
using Presentation.DTOs.TaskBoard;
using Presentation.Entities;
using Presentation.Enums;
using Presentation.Exceptions;
using Presentation.Interfaces;
using Presentation.Interfaces.Service;

namespace Presentation.Services;

public class TaskBoardService : ITaskBoardService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly IUserContext _userContext;
    private readonly ITeamRoleService _teamRoleService;

    public TaskBoardService(
        IUnitOfWork unitOfWork, 
        IMapper mapper,
        IUserContext userContext,
        ITeamRoleService teamRoleService)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _userContext = userContext;
        _teamRoleService = teamRoleService;
    }
    public async Task<TaskBoard> CreateNewBoard(Guid teamId, CreateTaskBoardDto boardDto)
    {
        var userId = _userContext.UserId;
        
        if (!await _unitOfWork.TeamMembers.ExistsAsync(teamId, userId))
            throw new ForbiddenException($"User {userId} does not belong to team");
        
        if (!await _teamRoleService.IsInTeamRoleOrHigherAsync(userId, teamId, TeamRole.Admin))
            throw new ForbiddenException($"User does not have permission to create board in team {teamId}");
        
        if (!await _unitOfWork.Teams.ExistsAsync(teamId))
            throw new NotFoundException($"Team with id {teamId} does not exist.");
        
        var board = _mapper.Map<TaskBoard>(boardDto);
        board.TeamId = teamId;
        await _unitOfWork.Boards.AddAsync(board);

        var columnsStarterPack = new List<BoardColumn>
        {
            new() { BoardId = board.Id, Name = "ToDo", Order = 1, Status = Status.ToDo, TaskLimit = null },
            new() { BoardId = board.Id, Name = "InProgress", Order = 2, Status = Status.InProgress, TaskLimit = null },
            new() { BoardId = board.Id, Name = "Done", Order = 3, Status = Status.Done, TaskLimit = null },
        };
        
        await _unitOfWork.Columns.AddRangeAsync(columnsStarterPack);
        await _unitOfWork.CommitAsync();

        return board;
    }

    public async Task<TaskBoard?> GetBoardAsync(Guid teamId, Guid boardId)
    {
        var userId = _userContext.UserId;
        
        if (!await _unitOfWork.TeamMembers.ExistsAsync(teamId, userId))
            throw new ForbiddenException($"User {userId} does not belong to team");
        
        var board = await _unitOfWork.Boards.GetByIdAsync(boardId);
        if (board == null || board.TeamId != teamId) throw new NotFoundException("Board is not found");
        return board;
    }

    public async Task<IEnumerable<TaskBoard>> GetBoardsAsync()
    {
        // Get absolutely all boards
        return await _unitOfWork.Boards.GetAllAsync();
    }
    
    public async Task<IEnumerable<TaskBoard>> GetBoardsAsync(Guid teamId)
    {
        var userId = _userContext.UserId;
        
        if (!await _unitOfWork.TeamMembers.ExistsAsync(teamId, userId))
            throw new ForbiddenException($"User {userId} does not belong to team");
        
        return await _unitOfWork.Boards.FindAllAsync(t => t.TeamId == teamId);
    }

    public async Task UpdateBoardAsync(Guid teamId, Guid id, UpdateTaskBoardDto boardDto)
    {
        var userId = _userContext.UserId;
        
        if (!await _unitOfWork.TeamMembers.ExistsAsync(teamId, userId))
            throw new ForbiddenException($"User {userId} does not belong to team");
        
        if (!await _teamRoleService.IsInTeamRoleOrHigherAsync(userId, teamId, TeamRole.Admin))
            throw new ForbiddenException($"User does not have permission to update board in team {teamId}");
        
        var board = await _unitOfWork.Boards.GetByIdAsync(id);
        if (board == null || board.TeamId != teamId) throw new NotFoundException("Board is not found");
        _mapper.Map(boardDto, board);
        await _unitOfWork.CommitAsync();
    }

    public async Task DeleteBoardAsync(Guid teamId, Guid id)
    {
        var userId = _userContext.UserId;
        
        if (!await _unitOfWork.TeamMembers.ExistsAsync(teamId, userId))
            throw new ForbiddenException($"User {userId} does not belong to team");
        
        if (!await _teamRoleService.IsInTeamRoleOrHigherAsync(userId, teamId, TeamRole.Admin))
            throw new ForbiddenException($"User does not have permission to delete board in team {teamId}");
        
        var board = await _unitOfWork.Boards.GetByIdAsync(id);
        if (board == null) throw new NotFoundException("Board is not found");

        await _unitOfWork.Tasks.ClearBoardIdInBoardAsync(id);
        _unitOfWork.Boards.Remove(board);
        await _unitOfWork.CommitAsync();

    }
}