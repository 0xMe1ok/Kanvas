using Asp.Versioning;
using AutoMapper;
using Microsoft.AspNetCore.JsonPatch;
using Presentation;
using Presentation.DTOs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Presentation.Entities;
using Presentation.Enums;
using Presentation.Interfaces;

namespace Presentation.Controllers;

[ApiController]
[Route("api/v{version:apiVersion}/tasks")]
[ApiVersion("1.0")]
public class AppTaskController : ControllerBase
{
    // TODO: in global - change dbcontext -> CQRS
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public AppTaskController(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }
    
    [HttpGet]
    [Route("{id:guid}")]
    public async Task<IActionResult> GetById(Guid id)
    {
        // TODO: only from current team
        var task = await _unitOfWork.Tasks.GetByIdAsync(id);
        if (task == null) return NotFound();
        return Ok(_mapper.Map<AppTaskDto>(task));
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        // TODO: only from selected and accessible team
        var tasks = await _unitOfWork.Tasks.GetAllAsync();
        return Ok(tasks.Select(task => _mapper.Map<AppTaskDto>(task)));
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateAppTaskRequestDto appTaskDto)
    {
        // TODO: only to selected and accessible team
        if (!ModelState.IsValid) return BadRequest(ModelState);
        
        if (appTaskDto.BoardId != null && 
            !await _unitOfWork.Boards.ExistsAsync(appTaskDto.BoardId ?? Guid.Empty))
        {
            return NotFound("Board doesn't exist");
        }
        
        var task = _mapper.Map<AppTask>(appTaskDto);
        
        var column = await _unitOfWork.Columns
            .FindAsync(column => column.BoardId == appTaskDto.BoardId 
                                           && column.Status == task.Status);
        task.ColumnId = column?.Id;
        
        await _unitOfWork.Tasks.AddAsync(task);
        await _unitOfWork.CommitAsync();
        
        return CreatedAtAction(nameof(GetById), new { id = task.Id }, _mapper.Map<AppTaskDto>(task));
    }

    [HttpPut]
    [Route("{id:guid}")]
    public async Task<IActionResult> Update([FromRoute] Guid id, [FromBody] UpdateAppTaskRequestDto appTaskDto)
    {
        // TODO: only to selected and accessible team
        if (!ModelState.IsValid) return BadRequest(ModelState);
        
        if (appTaskDto.BoardId != null && 
            !await _unitOfWork.Boards.ExistsAsync(appTaskDto.BoardId ?? Guid.Empty))
        {
            return NotFound("Board doesn't exist");
        }
        
        var task = await _unitOfWork.Tasks.GetByIdAsync(id);
        if (task == null) return NotFound();

        if (task.Status != appTaskDto.Status)
        {
            var column = await _unitOfWork.Columns
                .FindAsync(column => column.BoardId == appTaskDto.BoardId 
                                          && column.Status == appTaskDto.Status);
            task.ColumnId = column?.Id;
        }
        
        _mapper.Map(appTaskDto, task);
        await _unitOfWork.CommitAsync();
        
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
        var task = await _unitOfWork.Tasks.GetByIdAsync(id);
        if (task == null) return NotFound();
        _unitOfWork.Tasks.Remove(task);
        await _unitOfWork.CommitAsync();
        
        return NoContent();
    }
}