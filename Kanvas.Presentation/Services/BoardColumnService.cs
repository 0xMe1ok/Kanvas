using AutoMapper;
using Presentation.DTOs.BoardColumn;
using Presentation.Entities;
using Presentation.Exceptions;
using Presentation.Interfaces;

namespace Presentation.Services;

public class BoardColumnService : IBoardColumnService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public BoardColumnService(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }
    public async Task<BoardColumn?> CreateNewColumn(Guid boardId, CreateBoardColumnDto columnDto)
    {
        // TODO: only for boards in accessible teams, for admins/redactors
        // TODO: check for valid status, there can't be two columns with the same status
        if (!await _unitOfWork.Boards.ExistsAsync(boardId))
        {
            throw new NotFoundException("Board does not exist");
        }

        if (!await _unitOfWork.Columns.ExistsAsync
            (c => c.BoardId == boardId &&
                  c.Status == columnDto.Status))
        {
            throw new ForbiddenException("Column with that status already exists");
        }
        
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
    public async Task<IEnumerable<BoardColumn>> GetColumnsAsync(Guid boardId)
    {
        // TODO: only for boards in accessible teams
        return await _unitOfWork.Columns
            .FindAllAsync(column => column.BoardId == boardId);
    }

    public async Task UpdateColumnAsync(Guid boardId, Guid columnId, UpdateBoardColumnDto columnDto)
    {
        if (!await _unitOfWork.Boards.ExistsAsync(boardId))
        {
            throw new NotFoundException("Board not found");
        }
        
        if (await _unitOfWork.Columns.ExistsAsync
            (c => c.BoardId == boardId &&
                  c.Status == columnDto.Status))
        {
            throw new ForbiddenException("Column with that status already exists");
        }
        
        var column = await _unitOfWork.Columns.GetByIdAsync(columnId);
        if (column == null) throw new NotFoundException("Column with that id does not exist");
        
        await using var transaction = await _unitOfWork.BeginTransactionAsync();
        
        if (column.Order != columnDto.Order)
        {
            await MoveColumnInternalAsync(column, columnDto.Order);
        }
        
        _mapper.Map(columnDto, column);
        _unitOfWork.Columns.Update(column);
        
        await _unitOfWork.Tasks.SetColumnForTaskInBoardAsync(boardId, columnDto.Status, column.Id);
        
        await _unitOfWork.CommitAsync();
        await transaction.CommitAsync();
    }

    public async Task DeleteColumnAsync(Guid boardId, Guid columnId)
    {
        // TODO: check if column in board
        if (!await _unitOfWork.Boards.ExistsAsync(boardId))
        {
            throw new NotFoundException("Board not found");
        }
        
        var column = await _unitOfWork.Columns.GetByIdAsync(columnId);
        if (column == null) throw new NotFoundException("Column with that id does not exist");
        _unitOfWork.Columns.Remove(column);

        await _unitOfWork.Tasks.ClearColumnIdInColumnAsync(columnId);
        await _unitOfWork.CommitAsync();
    }

    public async Task MoveColumnAsync(Guid boardId, Guid columnId, int newOrder)
    {
        // TODO: check if column in board
        if (!await _unitOfWork.Boards.ExistsAsync(boardId))
        {
            throw new NotFoundException("Board not found");
        }
        
        var column = await _unitOfWork.Columns.GetByIdAsync(columnId);
            
        if (column == null) throw new NotFoundException("Task doesn't exist");
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