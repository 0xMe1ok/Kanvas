using Infrastructure;
using Infrastructure.DTOs;
using Infrastructure.Mapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Presentation.Controllers;

[ApiController]
[Route("api/task")]
public class AppTaskController : ControllerBase
{
    public ApplicationDbContext _context;

    public AppTaskController(ApplicationDbContext context)
    {
        _context = context;
    }
    
    [HttpGet]
    [Route("{taskId}")]
    public async Task<IActionResult> GetById(Guid taskId)
    {
        var task = await _context.AppTasks.FirstOrDefaultAsync(t => t.Id == taskId);
        if (task == null) return NotFound();
        return Ok(task.ToAppTaskDto());
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var tasks = await _context.AppTasks.ToListAsync();
        
        return Ok(tasks.Select(t => t.ToAppTaskDto()));
    }

    [HttpPost]
    public async Task<IActionResult> Create(CreateAppTaskRequestDto appTaskDto)
    {
        var task = appTaskDto.ToAppTask();

        await _context.AppTasks.AddAsync(task);
        await _context.SaveChangesAsync();
        
        return Ok();
    }

    [HttpPut]
    [Route("{taskId}")]
    public async Task<IActionResult> Update([FromRoute] Guid taskId, [FromBody] UpdateAppTaskRequestDto appTaskDto)
    {
        var task = await _context.AppTasks.FirstOrDefaultAsync(t => t.Id == taskId);
        if (task == null) return NotFound();
        
        task.Name = appTaskDto.Name;
        task.Description = appTaskDto.Description;
        task.DueDate = appTaskDto.DueDate;
        task.ColumnId = appTaskDto.ColumnId;
        task.AssigneeId = appTaskDto.AssigneeId;
        task.Order = appTaskDto.Order;
        
        await _context.SaveChangesAsync();
        return Ok(appTaskDto);
    }

    [HttpDelete]
    [Route("{taskId}")]
    public async Task<IActionResult> Delete([FromRoute] Guid taskId)
    {
        var task = await _context.AppTasks.FirstOrDefaultAsync(t => t.Id == taskId);
        if (task == null) return NotFound();
        _context.AppTasks.Remove(task);
        await _context.SaveChangesAsync();
        return NoContent();
    }
}