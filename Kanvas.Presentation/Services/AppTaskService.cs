using AutoMapper;
using Presentation.DTOs;
using Presentation.Entities;
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

    public async Task DeleteTaskAsync(Guid id)
    {
        var task = await _unitOfWork.Tasks.GetByIdAsync(id);
        if (task == null) throw new NotFoundException($"Task by id = {id} doesn't exist");
        _unitOfWork.Tasks.Remove(task);
        await _unitOfWork.CommitAsync();
    }
}