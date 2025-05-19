using System.Net;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.JsonWebTokens;
using TodoService.Config.Exceptions;
using TodoService.Dto;
using TodoService.Models;
using TodoService.Services.Abstracts;

namespace TodoService.Controllers;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class TodoController(ITodoService todoService) : ControllerBase
{
    [HttpPost]
    public async Task<ActionResult<TodoItem>> Create(CreateTodoDto dto)
    {
        var claim = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier);
        var subject = claim?.Value ?? throw new BusinessException("UNAUTHORIZED", HttpStatusCode.Unauthorized);
        return await todoService.Create(subject, dto);
    }
    
    [HttpGet]
    public async Task<ActionResult<IEnumerable<TodoItem>>> GetAll([FromQuery] int page = 1, [FromQuery] int pageSize = 10, [FromQuery] string searchTitle = "")
    {
        var claim = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier);
        var subject = claim?.Value ?? throw new BusinessException("UNAUTHORIZED", HttpStatusCode.Unauthorized);
        var result = await todoService.GetAll(subject, page, pageSize, searchTitle);
        return Ok(result);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<TodoItem>> Get(int id)
    {
        var claim = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier);
        var subject = claim?.Value ?? throw new BusinessException("UNAUTHORIZED", HttpStatusCode.Unauthorized);
        return await todoService.Get(subject, id);
    }

    [HttpPut("{id}")]
    public async Task<ActionResult> Update(int id, UpdateTodoDto updatedTodo)
    {
        var claim = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier);
        var subject = claim?.Value ?? throw new BusinessException("UNAUTHORIZED", HttpStatusCode.Unauthorized);
        await todoService.Update(subject, id, updatedTodo);
        return NoContent();
    }
    
    [HttpDelete("{id}")]
    public async Task<ActionResult> Delete(int id)
    {
        var claim = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier);
        var subject = claim?.Value ?? throw new BusinessException("UNAUTHORIZED", HttpStatusCode.Unauthorized);
        await todoService.Delete(subject, id);
        return NoContent();
    }
}