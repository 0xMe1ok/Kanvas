using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Presentation.DTOs.TaskBoard;
using Presentation.Entities;

namespace Presentation.Controllers;

[ApiController]
[Route("api/boards")]
public class TaskBoardController : ControllerBase
{
    private readonly ApplicationDbContext _context;
    private readonly IMapper _mapper;

    public TaskBoardController(ApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    [HttpGet]
    [Route("{id:guid}")]
    public async Task<IActionResult> GetById(Guid id)
    {
        var board = await _context.TaskBoards
            .Include(board => board.Columns)
            .ThenInclude(columns => columns.Tasks)
            .FirstOrDefaultAsync(b => b.Id == id);
        if (board == null) return NotFound();
        
        return Ok(_mapper.Map<TaskBoardDto>(board));
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var boards = await _context.TaskBoards
            .ToListAsync();
        return Ok(_mapper.Map<List<TaskBoardDto>>(boards));
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromRoute] TaskBoardDto taskBoardDto)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);
        
        var board = _mapper.Map<TaskBoard>(taskBoardDto);
        await _context.TaskBoards.AddAsync(board);
        await _context.SaveChangesAsync();
        
        return CreatedAtAction(nameof(GetById), new {id = board.Id}, _mapper.Map<TaskBoardDto>(board));
    }

    [HttpPut]
    [Route("{id:guid}")]
    public async Task<IActionResult> Update([FromRoute] Guid id, [FromBody] TaskBoardDto taskBoardDto)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);

        var board = _context.TaskBoards.FirstOrDefault(b => b.Id == id);
        if (board == null) return NotFound();
        _mapper.Map(taskBoardDto, board);
        await _context.SaveChangesAsync();
        
        return Ok(taskBoardDto);
    }

    [HttpDelete]
    [Route("{id:guid}")]
    public async Task<IActionResult> Delete([FromRoute] Guid id)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);
        var board = _context.TaskBoards.FirstOrDefault(b => b.Id == id);
        if (board == null) return NotFound();
        _context.TaskBoards.Remove(board);
        await _context.SaveChangesAsync();
        
        return NoContent();
    }
}