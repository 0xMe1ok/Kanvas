using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;

namespace Presentation.Controllers;


[ApiController]
[Route("api/v{version:apiVersion}/teams/boards")]
[ApiVersion("1.0")]
public class TeamBoardController : ControllerBase
{
    
}