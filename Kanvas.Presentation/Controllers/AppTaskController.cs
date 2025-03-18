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
[Route("api/v{version:apiVersion}/tasks")]
[ApiVersion("1.0")]
public class AppTaskController : ControllerBase
{
    // TODO: in global - change dbcontext -> CQRS
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
    public async Task<IActionResult> GetById(Guid id)
    {
        var task = await _appTaskService.GetTaskAsync(id);
        return Ok(_mapper.Map<AppTaskDto>(task));
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var tasks = await _appTaskService.GetTasksAsync();
        return Ok(tasks.Select(task => _mapper.Map<AppTaskDto>(task)));
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateAppTaskDto appTaskDto)
    {
        // TODO: only to selected and accessible team
        if (!ModelState.IsValid) return BadRequest(ModelState);
        var task = await _appTaskService.CreateNewTask(appTaskDto);
        return CreatedAtAction(nameof(GetById), new { id = task.Id }, _mapper.Map<AppTaskDto>(task));
    }

    [HttpPut]
    [Route("{id:guid}")]
    public async Task<IActionResult> Update([FromRoute] Guid id, [FromBody] UpdateAppTaskDto appTaskDto)
    {
        // TODO: only to selected and accessible team
        if (!ModelState.IsValid) return BadRequest(ModelState);
        await _appTaskService.UpdateTaskAsync(id, appTaskDto);
        return Ok(appTaskDto);
    }

    [HttpPatch]
    [Route("{id:guid}/order")]
    public async Task<IActionResult> Move([FromRoute] Guid id, [FromBody] MoveAppTaskDto appTaskDto)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);
        
        await _appTaskService.MoveTaskAsync(id, appTaskDto.NewOrder);
        
        return Ok(appTaskDto);
    }

    [HttpPatch]
    [Route("{id:guid}/status")]
    public async Task<IActionResult> ChangeStatus([FromRoute] Guid id, [FromBody] ChangeStatusAppTaskDto appTaskDto)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);
        
        await _appTaskService.ChangeTaskStatusAsync(id, appTaskDto.NewStatus);
        
        return Ok(appTaskDto);
    }
    
    /*
    [HttpPatch]
    [Route("{id:guid}")]
    public async Task<IActionResult> Patch([FromRoute] Guid id, [FromBody] JsonPatchDocument<AppTaskDto>? patchDoc)
    {
        if (patchDoc == null) return BadRequest();
        
        var task = await _unitOfWork.Tasks.GetByIdAsync(id);
        if (task == null) return NotFound();
        
        var taskDto = _mapper.Map<AppTaskDto>(task);
        patchDoc.ApplyTo(taskDto, ModelState);
        
        if (!ModelState.IsValid) return BadRequest(ModelState);
        _mapper.Map(taskDto, task);
        await _unitOfWork.CommitAsync();
        
        return Ok(_mapper.Map<AppTaskDto>(task));
    }
    */

    [HttpDelete]
    [Route("{id:guid}")]
    public async Task<IActionResult> Delete([FromRoute] Guid id)
    {
        // TODO: only to selected and accessible team, not for viewers
        await _appTaskService.DeleteTaskAsync(id);
        return NoContent();
    }
}