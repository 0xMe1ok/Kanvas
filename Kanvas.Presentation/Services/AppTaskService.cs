using AutoMapper;
using Presentation.DTOs;
using Presentation.Entities;
using Presentation.Enums;
using Presentation.Exceptions;
using Presentation.Interfaces;

namespace Presentation.Services;

public class AppTaskService : IAppTaskService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public AppTaskService(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }
    
    public async Task<AppTask?> CreateNewTask(CreateAppTaskDto taskDto)
    {
        if (taskDto.BoardId != null && 
            !await _unitOfWork.Boards.ExistsAsync(taskDto.BoardId ?? Guid.Empty))
        {
            throw new NotFoundException($"Board by id = {taskDto.BoardId} doesn't exist");
        }
        
        var task = _mapper.Map<AppTask>(taskDto);
        var column = await _unitOfWork.Columns
            .FindAsync(column => column.BoardId == taskDto.BoardId 
                                 && column.Status == task.Status);
        task.ColumnId = column?.Id;
        
        await _unitOfWork.Tasks.AddAsync(task);
        await _unitOfWork.CommitAsync();
        
        return task;
    }

    public async Task<AppTask?> GetTaskAsync(Guid id)
    {
        // TODO: only from current team
        var task = await _unitOfWork.Tasks.GetByIdAsync(id);
        if (task == null) throw new NotFoundException("Task doesn't exist");
        return task;
    }

    public async Task<IEnumerable<AppTask>> GetTasksAsync()
    {
        // TODO: only from selected and accessible team
        var tasks = await _unitOfWork.Tasks.GetAllAsync();
        return tasks;
    }

    public async Task UpdateTaskAsync(Guid id, UpdateAppTaskDto taskDto)
    {
        // TODO: separate boardId
        if (taskDto.BoardId != null && 
            ! await _unitOfWork.Boards.ExistsAsync(taskDto.BoardId ?? Guid.Empty))
        {
            throw new NotFoundException($"Board by id = {taskDto.BoardId} doesn't exist");
        }
        
        var task = await _unitOfWork.Tasks.GetByIdAsync(id);
        if (task == null) throw new NotFoundException($"Task by id = {id} doesn't exist");

        if (task.Status != taskDto.Status)
        {
            var column = await _unitOfWork.Columns
                .FindAsync(column => column.BoardId == taskDto.BoardId 
                                     && column.Status == taskDto.Status);
            task.ColumnId = column?.Id;
        }
        
        _mapper.Map(taskDto, task);
        await _unitOfWork.CommitAsync();
    }

    public async Task MoveTaskAsync(Guid id, int order)
    {
        using (var transaction = await _unitOfWork.BeginTransactionAsync())
        {
            var task = await _unitOfWork.Tasks.GetByIdAsync(id);
            if (task == null) throw new NotFoundException("Task doesn't exist");
            
            if (task.ColumnId == null) throw new ForbiddenException("Task doesn't have column");
            
            if (task.Order != order)
            {
                var currentMaxOrder = await _unitOfWork.Tasks
                    .GetMaxOrderInColumnAsync(task.ColumnId ?? Guid.Empty);
                
                order = Math.Clamp(order, 1, currentMaxOrder + 1);
                
                if (task.Order < order)
                {
                    await _unitOfWork.Tasks.ShiftTasksOrderAsync(
                        task.ColumnId ?? Guid.Empty,
                        task.Order + 1,
                        order,
                        -1);
                }
                else
                {
                    await _unitOfWork.Tasks.ShiftTasksOrderAsync(
                        task.ColumnId ?? Guid.Empty,
                        order,
                        task.Order - 1,
                        1);
                }
                
                task.Order = order;
                _unitOfWork.Tasks.Update(task);

                await _unitOfWork.CommitAsync();
                await transaction.CommitAsync();
            }
        }
    }

    public async Task ChangeTaskStatusAsync(Guid id, Status status)
    {
        using (var transaction = await _unitOfWork.BeginTransactionAsync())
        {
            var task = await _unitOfWork.Tasks.GetByIdAsync(id);
            if (task == null) throw new NotFoundException("Task doesn't exist");

            if (task.Status != status)
            {
                var newColumn = await _unitOfWork.Columns
                    .FindAsync(column => column.BoardId == task.BoardId &&
                                         column.Status == status);
                
                task.ColumnId = newColumn?.Id;
                
                var currentMaxOrder = await _unitOfWork.Tasks
                    .GetMaxOrderInColumnAsync(task.ColumnId ?? Guid.Empty);
                
                task.Order = currentMaxOrder + 1;
            }
            
            await _unitOfWork.CommitAsync();
            await transaction.CommitAsync();
        }
    }

    public async Task DeleteTaskAsync(Guid id)
    {
        var task = await _unitOfWork.Tasks.GetByIdAsync(id);
        if (task == null) throw new NotFoundException($"Task by id = {id} doesn't exist");
        _unitOfWork.Tasks.Remove(task);
        await _unitOfWork.CommitAsync();
    }
}