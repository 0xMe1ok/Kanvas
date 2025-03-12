using AutoMapper;
using Presentation;
using Presentation.DTOs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Presentation.Entities;

namespace Presentation.Controllers;

[ApiController]
[Route("api/tasks")]
public class AppTaskController : ControllerBase
{
    // TODO: in global - change dbcontext -> CQRS
    private readonly ApplicationDbContext _context;
    private readonly IMapper _mapper;

    public AppTaskController(ApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }
    
    [HttpGet]
    [Route("{id:guid}")]
    public async Task<IActionResult> GetById(Guid id)
    {
        // TODO: only from current team
        var task = await _context.AppTasks.FirstOrDefaultAsync(t => t.Id == id);
        if (task == null) return NotFound();
        return Ok(_mapper.Map<AppTaskDto>(task));
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        // TODO: only from selected and accessible team
        var tasks = await _context.AppTasks.ToListAsync();
        return Ok(tasks.Select(task => _mapper.Map<AppTaskDto>(task)));
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateAppTaskRequestDto appTaskDto)
    {
        // TODO: only to selected and accessible team
        if (!ModelState.IsValid) return BadRequest(ModelState);
        
        if (appTaskDto.BoardId != null && !_context.TaskBoards.Any(b => b.Id == appTaskDto.BoardId))
        {
            return NotFound("Board doesn't exist");
        }
        
        var task = _mapper.Map<AppTask>(appTaskDto);
        
        task.ColumnId = _context.BoardColumns
            .FirstOrDefault(column => column.BoardId == appTaskDto.BoardId 
            && column.Status == task.Status)?.Id;
        
        await _context.AppTasks.AddAsync(task);
        await _context.SaveChangesAsync();
        
        return CreatedAtAction(nameof(GetById), new { id = task.Id }, _mapper.Map<AppTaskDto>(task));
    }

    [HttpPut]
    [Route("{id:guid}")]
    public async Task<IActionResult> Update([FromRoute] Guid id, [FromBody] UpdateAppTaskRequestDto appTaskDto)
    {
        // TODO: only to selected and accessible team
        if (!ModelState.IsValid) return BadRequest(ModelState);
        
        if (appTaskDto.BoardId != null && !_context.TaskBoards.Any(b => b.Id == appTaskDto.BoardId))
        {
            return NotFound("Board doesn't exist");
        }
        
        var task = await _context.AppTasks.FirstOrDefaultAsync(t => t.Id == id);
        if (task == null) return NotFound();

        if (task.Status != appTaskDto.Status)
        {
            task.ColumnId = _context.BoardColumns
                .FirstOrDefault(column => column.BoardId == appTaskDto.BoardId 
                                          && column.Status == task.Status)?.Id;
        }
        
        _mapper.Map(appTaskDto, task);
        await _context.SaveChangesAsync();
        
        return Ok(appTaskDto);
    }

    [HttpDelete]
    [Route("{id:guid}")]
    public async Task<IActionResult> Delete([FromRoute] Guid id)
    {
        // TODO: only to selected and accessible team, not for viewers
        var task = await _context.AppTasks.FirstOrDefaultAsync(t => t.Id == id);
        if (task == null) return NotFound();
        _context.AppTasks.Remove(task);
        await _context.SaveChangesAsync();
        
        return NoContent();
    }
}