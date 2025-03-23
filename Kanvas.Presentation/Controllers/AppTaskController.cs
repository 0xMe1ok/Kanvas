using Asp.Versioning;
using AutoMapper;
using Microsoft.AspNetCore.JsonPatch;
using Presentation;
using Presentation.DTOs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Presentation.Entities;
using Presentation.Enums;
using Presentation.Exceptions;
using Presentation.Interfaces;

namespace Presentation.Controllers;

[ApiController]
[Route("api/v{version:apiVersion}/teams/{teamId}/tasks")]
[ApiVersion("1.0")]
public class AppTaskController : ControllerBase
{
    // TODO: in global - change dbcontext -> CQRS
    // TODO: maybe use Result approach, not Exception
    private readonly IAppTaskService _appTaskService;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public AppTaskController
        (IAppTaskService appTaskService, 
            IUnitOfWork unitOfWork, 
            IMapper mapper)
    {
        _appTaskService = appTaskService;
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }
    
    [HttpGet]
    [Route("{id:guid}")]
    public async Task<IActionResult> GetById([FromRoute] Guid teamId, [FromRoute] Guid id)
    {
        var task = await _appTaskService.GetTaskAsync(teamId, id);
        return Ok(_mapper.Map<AppTaskDto>(task));
    }

    [HttpGet]
    public async Task<IActionResult> GetAll([FromRoute] Guid teamId)
    {
        var tasks = await _appTaskService.GetTasksAsync(teamId);
        return Ok(tasks.Select(task => _mapper.Map<AppTaskDto>(task)));
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromRoute] Guid teamId, [FromBody] CreateAppTaskDto appTaskDto)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);
        var task = await _appTaskService.CreateNewTask(teamId, appTaskDto);
        return CreatedAtAction(nameof(GetById), new { id = task.Id }, _mapper.Map<AppTaskDto>(task));
    }

    [HttpPut]
    [Route("{id:guid}")]
    public async Task<IActionResult> Update(
        [FromRoute] Guid teamId, 
        [FromRoute] Guid id, 
        [FromBody] UpdateAppTaskDto appTaskDto)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);
        await _appTaskService.UpdateTaskAsync(teamId, id, appTaskDto);
        return Ok(appTaskDto);
    }

    [HttpPatch]
    [Route("{id:guid}/order")]
    public async Task<IActionResult> Move(
        [FromRoute] Guid teamId, 
        [FromRoute] Guid id, 
        [FromBody] MoveAppTaskDto appTaskDto)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);
        await _appTaskService.MoveTaskAsync(teamId, id, appTaskDto.NewOrder);
        return Ok(appTaskDto);
    }

    [HttpPatch]
    [Route("{id:guid}/status")]
    public async Task<IActionResult> ChangeStatus(
        [FromRoute] Guid teamId, 
        [FromRoute] Guid id, 
        [FromBody] ChangeStatusAppTaskDto appTaskDto)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);
        await _appTaskService.ChangeTaskStatusAsync(teamId, id, appTaskDto.NewStatus);
        return Ok(appTaskDto);
    }

    [HttpDelete]
    [Route("{id:guid}")]
    public async Task<IActionResult> Delete([FromRoute] Guid teamId, [FromRoute] Guid id)
    {
        // TODO: only to selected and accessible team, not for viewers
        await _appTaskService.DeleteTaskAsync(teamId, id);
        return NoContent();
    }
}