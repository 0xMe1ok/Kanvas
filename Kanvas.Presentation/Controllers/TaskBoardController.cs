using Asp.Versioning;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Presentation.DTOs;
using Presentation.DTOs.BoardColumn;
using Presentation.DTOs.TaskBoard;
using Presentation.Entities;
using Presentation.Enums;

namespace Presentation.Controllers;

[ApiController]
[Route("api/v{version:apiVersion}/boards")]
[ApiVersion("1.0")]
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
        // TODO: only from selected and accessible team
        var board = await _context.TaskBoards
            .Include(b => b.Columns)
            .Include(b => b.Tasks)
            .FirstOrDefaultAsync(b => b.Id == id);
        
        if (board == null) return NotFound();

        var boardDto = _mapper.Map<TaskBoardDto>(board);
        
        return Ok(boardDto);
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        // TODO: only from selected and accessible team
        var boards = await _context.TaskBoards
            .ToListAsync();
        return Ok(_mapper.Map<List<TaskBoardDto>>(boards));
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateTaskBoardRequestDto taskBoardDto)
    {
        // TODO: only from selected and accessible team, for admins/redactors
        if (!ModelState.IsValid) return BadRequest(ModelState);

        if (taskBoardDto.TeamId != null && !_context.AppTeams.Any(team => team.Id == taskBoardDto.TeamId))
        {
            return NotFound("Team doesn't exist");
        }
        
        var board = _mapper.Map<TaskBoard>(taskBoardDto);
        await _context.TaskBoards.AddAsync(board);

        var columnsStarterPack = new List<BoardColumn>
        {
            new BoardColumn
            {
                Id = Guid.NewGuid(),
                BoardId = board.Id,
                Name = "ToDo",
                Order = 0,
                Status = Status.ToDo,
                TaskLimit = null
            },
            new BoardColumn
            {
                Id = Guid.NewGuid(),
                BoardId = board.Id,
                Name = "InProgress",
                Order = 1,
                Status = Status.InProgress,
                TaskLimit = null
            },
            new BoardColumn
            {
                Id = Guid.NewGuid(),
                BoardId = board.Id,
                Name = "Done",
                Order = 2,
                Status = Status.Done,
                TaskLimit = null
            },
        };
        
        await _context.BoardColumns.AddRangeAsync(columnsStarterPack);
        
        await _context.SaveChangesAsync();
        
        return CreatedAtAction(nameof(GetById), new {id = board.Id}, _mapper.Map<TaskBoardDto>(board));
    }

    [HttpPut]
    [Route("{id:guid}")]
    public async Task<IActionResult> Update([FromRoute] Guid id, [FromBody] UpdateTaskBoardRequestDto taskBoardDto)
    {
        // TODO: only from selected and accessible team, for admins/redactors
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
        // TODO: only from selected and accessible team, for admins/redactors
        if (!ModelState.IsValid) return BadRequest(ModelState);
        var board = _context.TaskBoards.FirstOrDefault(b => b.Id == id);
        if (board == null) return NotFound();
        
        var tasks = _context.AppTasks.Where(task => task.BoardId == id);
        
        foreach (var task in tasks)
            task.BoardId = null;
        
        _context.TaskBoards.Remove(board);
        await _context.SaveChangesAsync();
        
        return NoContent();
    }
}