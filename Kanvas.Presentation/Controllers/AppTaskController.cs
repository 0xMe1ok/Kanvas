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
        var task = await _context.AppTasks.FirstOrDefaultAsync(t => t.Id == id);
        if (task == null) return NotFound();
        return Ok(_mapper.Map<AppTaskDto>(task));
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var tasks = await _context.AppTasks.ToListAsync();
        return Ok(tasks.Select(task => _mapper.Map<AppTaskDto>(task)));
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateAppTaskRequestDto appTaskDto)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);
        
        if (appTaskDto.BoardId != null && !_context.TaskBoards.Any(b => b.Id == appTaskDto.BoardId))
        {
            return NotFound("Board doesn't exist");
        }
        
        var task = _mapper.Map<AppTask>(appTaskDto);
        await _context.AppTasks.AddAsync(task);
        await _context.SaveChangesAsync();
        
        return CreatedAtAction(nameof(GetById), new { id = task.Id }, _mapper.Map<AppTaskDto>(task));
    }

    [HttpPut]
    [Route("{id:guid}")]
    public async Task<IActionResult> Update([FromRoute] Guid id, [FromBody] UpdateAppTaskRequestDto appTaskDto)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);
        
        if (appTaskDto.BoardId != null && !_context.TaskBoards.Any(b => b.Id == appTaskDto.BoardId))
        {
            return NotFound("Board doesn't exist");
        }
        
        var task = await _context.AppTasks.FirstOrDefaultAsync(t => t.Id == id);
        if (task == null) return NotFound();
        _mapper.Map(appTaskDto, task);
        await _context.SaveChangesAsync();
        
        return Ok(appTaskDto);
    }

    [HttpDelete]
    [Route("{id:guid}")]
    public async Task<IActionResult> Delete([FromRoute] Guid id)
    {
        var task = await _context.AppTasks.FirstOrDefaultAsync(t => t.Id == id);
        if (task == null) return NotFound();
        _context.AppTasks.Remove(task);
        await _context.SaveChangesAsync();
        
        return NoContent();
    }
}