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
    private readonly IUserContext _userContext;
    private readonly ITeamRoleService _teamRoleService;

    public AppTaskService(
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
    
    public async Task<AppTask?> CreateNewTask(Guid teamId, CreateAppTaskDto taskDto)
    {
        var userId = _userContext.UserId;
        
        if (!await _unitOfWork.TeamMembers.ExistsAsync(teamId, userId))
            throw new ForbiddenException($"User {userId} does not belong to team");

        if (!await _teamRoleService.IsInTeamRoleOrHigherAsync(userId, teamId, TeamRole.Editor))
            throw new ForbiddenException($"User does not have permission to create task in team {teamId}");
        
        if (taskDto.BoardId != null && 
            !await _unitOfWork.Boards.ExistsAsync(taskDto.BoardId ?? Guid.Empty))
            throw new NotFoundException($"Board by id = {taskDto.BoardId} doesn't exist");

        if (!await _unitOfWork.Teams.ExistsAsync(teamId))
            throw new NotFoundException($"Team by id = {teamId} doesn't exist");
        
        var task = _mapper.Map<AppTask>(taskDto);
        task.TeamId = teamId;
        var column = await _unitOfWork.Columns
            .FindAsync(column => column.BoardId == taskDto.BoardId 
                                 && column.Status == task.Status);
        task.ColumnId = column?.Id;
        
        var currentMaxOrder = await _unitOfWork.Tasks
            .GetMaxOrderInColumnAsync(task.ColumnId ?? Guid.Empty);
        
        task.Order = currentMaxOrder + 1;
        
        await _unitOfWork.Tasks.AddAsync(task);
        await _unitOfWork.CommitAsync();
        
        return task;
    }

    public async Task<AppTask?> GetTaskAsync(Guid teamId, Guid id)
    {
        var userId = _userContext.UserId;
        
        if (!await _unitOfWork.TeamMembers.ExistsAsync(teamId, userId))
            throw new ForbiddenException($"User {userId} does not belong to team");
        
        if (!await _unitOfWork.Teams.ExistsAsync(teamId))
            throw new NotFoundException($"Team by id = {teamId} doesn't exist");
        
        var task = await _unitOfWork.Tasks.GetByIdAsync(id);
        if (task == null || task.TeamId != teamId) throw new NotFoundException("Task doesn't exist");
        return task;
    }

    public async Task<IEnumerable<AppTask>> GetTasksAsync(Guid teamId)
    {
        var userId = _userContext.UserId;
        
        if (!await _unitOfWork.TeamMembers.ExistsAsync(teamId, userId))
            throw new ForbiddenException($"User {userId} does not belong to team");
        
        var tasks = await _unitOfWork.Tasks.FindAllAsync(t => t.TeamId == teamId);
        return tasks;
    }

    public async Task UpdateTaskAsync(Guid teamId, Guid id, UpdateAppTaskDto taskDto)
    {
        var userId = _userContext.UserId;
        
        if (!await _unitOfWork.TeamMembers.ExistsAsync(teamId, userId))
            throw new ForbiddenException($"User {userId} does not belong to team");
        
        if (!await _teamRoleService.IsInTeamRoleOrHigherAsync(userId, teamId, TeamRole.Editor))
            throw new ForbiddenException($"User does not have permission to update task in team {teamId}");
        
        if (taskDto.BoardId != null && 
            !await _unitOfWork.Boards.ExistsAsync(taskDto.BoardId ?? Guid.Empty))
            throw new NotFoundException($"Board by id = {taskDto.BoardId} doesn't exist");
        
        if (!await _unitOfWork.Teams.ExistsAsync(teamId))
            throw new NotFoundException($"Team by id = {teamId} doesn't exist");
        
        var task = await _unitOfWork.Tasks.GetByIdAsync(id);
        if (task == null || task.TeamId != teamId) throw new NotFoundException("Task doesn't exist");
        
        await using var transaction = await _unitOfWork.BeginTransactionAsync();

        if (task.Status != taskDto.Status)
            await ChangeTaskStatusInternalAsync(task, taskDto.Status);
        else if (task.Order != taskDto.Order)
            await MoveTaskInternalAsync(task, taskDto.Order);
        
        _mapper.Map(taskDto, task);
        _unitOfWork.Tasks.Update(task);
        await _unitOfWork.CommitAsync();
        await transaction.CommitAsync();
    }

    public async Task MoveTaskAsync(Guid teamId, Guid id, int order)
    {
        var userId = _userContext.UserId;
        
        if (!await _unitOfWork.TeamMembers.ExistsAsync(teamId, userId))
            throw new ForbiddenException($"User {userId} does not belong to team");
        
        if (!await _teamRoleService.IsInTeamRoleOrHigherAsync(userId, teamId, TeamRole.Editor))
            throw new ForbiddenException($"User does not have permission to update task in team {teamId}");
        
        var task = await _unitOfWork.Tasks.GetByIdAsync(id);
            
        if (task == null || task.TeamId != teamId) throw new NotFoundException("Task doesn't exist");
        if (task.ColumnId == null) throw new ForbiddenException("Task doesn't have column");
        if (task.Order == order) return;
            
        await using var transaction = await _unitOfWork.BeginTransactionAsync();
        
        await MoveTaskInternalAsync(task, order);
                
        task.Order = order;
        _unitOfWork.Tasks.Update(task);

        await _unitOfWork.CommitAsync();
        await transaction.CommitAsync();
    }

    public async Task ChangeTaskStatusAsync(Guid teamId, Guid id, Status status)
    {
        var userId = _userContext.UserId;
        
        if (!await _unitOfWork.TeamMembers.ExistsAsync(teamId, userId))
            throw new ForbiddenException($"User {userId} does not belong to team");
        
        if (!await _teamRoleService.IsInTeamRoleOrHigherAsync(userId, teamId, TeamRole.Editor))
            throw new ForbiddenException($"User does not have permission to update task in team {teamId}");
        
        var task = await _unitOfWork.Tasks.GetByIdAsync(id);
        
        if (task == null || task.TeamId != teamId) throw new NotFoundException("Task doesn't exist");
        if (task.Status == status) return;
        
        await using var transaction = await _unitOfWork.BeginTransactionAsync();
        
        await ChangeTaskStatusInternalAsync(task, status);
            
        await _unitOfWork.CommitAsync();
        await transaction.CommitAsync();
    }

    public async Task DeleteTaskAsync(Guid teamId, Guid id)
    {
        var userId = _userContext.UserId;
        
        if (!await _unitOfWork.TeamMembers.ExistsAsync(teamId, userId))
            throw new ForbiddenException($"User {userId} does not belong to team");
        
        if (!await _teamRoleService.IsInTeamRoleOrHigherAsync(userId, teamId, TeamRole.Editor))
            throw new ForbiddenException($"User does not have permission to update task in team {teamId}");
        
        var task = await _unitOfWork.Tasks.GetByIdAsync(id);
        if (task == null || task.TeamId != teamId) throw new NotFoundException($"Task by id = {id} doesn't exist");
        _unitOfWork.Tasks.Remove(task);
        await _unitOfWork.CommitAsync();
    }

    private async Task MoveTaskInternalAsync(AppTask task, int order)
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
    }

    private async Task ChangeTaskStatusInternalAsync(AppTask task, Status status)
    {
        var newColumn = await _unitOfWork.Columns
            .FindAsync(column => column.BoardId == task.BoardId &&
                                 column.Status == status);
            
        task.ColumnId = newColumn?.Id;
            
        var currentMaxOrder = await _unitOfWork.Tasks
            .GetMaxOrderInColumnAsync(task.ColumnId ?? Guid.Empty);
            
        task.Order = currentMaxOrder + 1;
    }
}