using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace TodoService.Controllers;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class TodoController: ControllerBase
{
    
}