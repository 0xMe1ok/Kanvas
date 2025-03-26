using AutoMapper;
using Presentation.DTOs.BoardColumn;
using Presentation.Entities;
using Presentation.Enums;
using Presentation.Exceptions;
using Presentation.Interfaces;

namespace Presentation.Services;

public class BoardColumnService : IBoardColumnService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly IUserContext _userContext;
    private readonly ITeamRoleService _teamRoleService;

    public BoardColumnService(
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
    public async Task<BoardColumn?> CreateNewColumn(Guid teamId, Guid boardId, CreateBoardColumnDto columnDto)
    {
        var userId = _userContext.UserId;
        
        if (!await _unitOfWork.TeamMembers.ExistsAsync(teamId, userId))
            throw new ForbiddenException($"User {userId} does not belong to team");
        
        if (!await _teamRoleService.IsInTeamRoleOrHigherAsync(userId, teamId, TeamRole.Admin))
            throw new ForbiddenException($"User does not have permission to update team {teamId}");
        
        if (!await _unitOfWork.Boards.ExistsAsync(boardId))
            throw new NotFoundException("Board does not exist");

        if (!await _unitOfWork.Columns.ExistsAsync
            (c => c.BoardId == boardId &&
                  c.Status == columnDto.Status))
            throw new ForbiddenException("Column with that status already exists");
        
        await using var transaction = await _unitOfWork.BeginTransactionAsync();
        
        var column = _mapper.Map<BoardColumn>(columnDto);
        column.BoardId = boardId;
        
        var currentMaxOrder = await _unitOfWork.Columns
            .GetMaxOrderInBoardAsync(column.BoardId);
        column.Order = currentMaxOrder + 1;
        
        await _unitOfWork.Columns.AddAsync(column);
        await _unitOfWork.Tasks.SetColumnForTaskInBoardAsync(boardId, columnDto.Status, column.Id);
        
        await _unitOfWork.CommitAsync();
        await transaction.CommitAsync();
        
        return column;
    }
    public async Task<IEnumerable<BoardColumn>> GetColumnsAsync(Guid teamId, Guid boardId)
    {
        var userId = _userContext.UserId;
        
        if (!await _unitOfWork.TeamMembers.ExistsAsync(teamId, userId))
            throw new ForbiddenException($"User {userId} does not belong to team");

        return await _unitOfWork.Columns
            .FindAllAsync(column => column.BoardId == boardId && column.Board.TeamId == teamId);
    }

    public async Task UpdateColumnAsync(Guid teamId, Guid boardId, Guid columnId, UpdateBoardColumnDto columnDto)
    {
        var userId = _userContext.UserId;
        
        if (!await _unitOfWork.TeamMembers.ExistsAsync(teamId, userId))
            throw new ForbiddenException($"User {userId} does not belong to team");
        
        if (!await _unitOfWork.Boards.ExistsAsync(boardId))
            throw new NotFoundException("Board not found");
        
        if (await _unitOfWork.Columns.ExistsAsync
            (c => c.BoardId == boardId &&
                  c.Status == columnDto.Status))
            throw new ForbiddenException("Column with that status already exists");
                
        var column = await _unitOfWork.Columns.GetByIdAsync(columnId);
        
        if (column == null || column.BoardId != boardId || column.Board.TeamId != teamId) 
            throw new NotFoundException("Column with that id does not exist");
        
        await using var transaction = await _unitOfWork.BeginTransactionAsync();
        
        if (column.Order != columnDto.Order)
            await MoveColumnInternalAsync(column, columnDto.Order);
        
        _mapper.Map(columnDto, column);
        _unitOfWork.Columns.Update(column);
        
        await _unitOfWork.Tasks.SetColumnForTaskInBoardAsync(boardId, columnDto.Status, column.Id);
        
        await _unitOfWork.CommitAsync();
        await transaction.CommitAsync();
    }

    public async Task DeleteColumnAsync(Guid teamId, Guid boardId, Guid columnId)
    {
        var userId = _userContext.UserId;
        
        if (!await _unitOfWork.TeamMembers.ExistsAsync(teamId, userId))
            throw new ForbiddenException($"User {userId} does not belong to team");
        
        if (!await _unitOfWork.Boards.ExistsAsync(boardId))
            throw new NotFoundException("Board not found");
        
        var column = await _unitOfWork.Columns.GetByIdAsync(columnId);
        
        if (column == null || column.BoardId != boardId || column.Board.TeamId != teamId) 
            throw new NotFoundException("Column with that id does not exist");
        
        _unitOfWork.Columns.Remove(column);

        await _unitOfWork.Tasks.ClearColumnIdInColumnAsync(columnId);
        await _unitOfWork.CommitAsync();
    }

    public async Task MoveColumnAsync(Guid teamId, Guid boardId, Guid columnId, int newOrder)
    {
        var userId = _userContext.UserId;
        
        if (!await _unitOfWork.TeamMembers.ExistsAsync(teamId, userId))
            throw new ForbiddenException($"User {userId} does not belong to team");
        
        if (!await _unitOfWork.Boards.ExistsAsync(boardId))
            throw new NotFoundException("Board not found");
        
        var column = await _unitOfWork.Columns.GetByIdAsync(columnId);
            
        if (column == null || column.BoardId != boardId || column.Board.TeamId != teamId) 
            throw new NotFoundException("Column with that id does not exist");
        
        if (column.Order == newOrder) return;
            
        await using var transaction = await _unitOfWork.BeginTransactionAsync();
        
        await MoveColumnInternalAsync(column, newOrder);
                
        column.Order = newOrder;
        _unitOfWork.Columns.Update(column);

        await _unitOfWork.CommitAsync();
        await transaction.CommitAsync();
    }

    private async Task MoveColumnInternalAsync(BoardColumn column, int newOrder)
    {
        var currentMaxOrder = await _unitOfWork.Columns
            .GetMaxOrderInBoardAsync(column.BoardId);
                
        newOrder = Math.Clamp(newOrder, 1, currentMaxOrder + 1);
                
        if (column.Order < newOrder)
        {
            await _unitOfWork.Columns.ShiftColumnsOrderAsync(
                column.BoardId,
                column.Order + 1,
                newOrder,
                -1);
        }
        else
        {
            await _unitOfWork.Columns.ShiftColumnsOrderAsync(
                column.BoardId,
                newOrder,
                column.Order - 1,
                1);
        }
    }
}