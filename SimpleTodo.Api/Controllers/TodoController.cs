using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SimpleTodo.Api.Data;
using SimpleTodo.Api.Models;

namespace SimpleTodo.Api.Controllers;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class TodoController(TodoDbContext dbContext) : ControllerBase
{
    private readonly DbSet<TodoItem> _todos = dbContext.Todos;

    [HttpDelete("{id}")]
    public async Task<ActionResult> Delete(int id)
    {
        TodoItem? existing = await _todos.FindAsync(id);
        if (existing is null) return NotFound();
        _todos.Remove(existing);
        await dbContext.SaveChangesAsync();
        return NoContent();
    }
    
    [HttpPut("{id}")]
    public async Task<ActionResult> Update(int id, TodoItem updatedTodo)
    {
        TodoItem? existing = await _todos.FindAsync(id);
        if (existing is null) return NotFound();
        existing.Title = updatedTodo.Title;
        existing.IsCompleted = updatedTodo.IsCompleted;
        await dbContext.SaveChangesAsync();
        return NoContent();
    }

    [HttpPost]
    public async Task<ActionResult<TodoItem>> Create(TodoItem todo)
    {
        _todos.Add(todo);
        await dbContext.SaveChangesAsync();
        return CreatedAtAction(nameof(Create), new { id = todo.Id }, todo);
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<TodoItem>>> GetAll(
        [FromQuery] int page = 1, [FromQuery] int pageSize = 10, [FromQuery] string? search = null
        )
    {
        var query = _todos.AsQueryable();
        if(!string.IsNullOrEmpty(search))
            query = query.Where(t => t.Title!.Contains(search));
        query = query.Skip(page - 1).Take(pageSize);
        return await query.AsNoTracking().ToListAsync();
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<TodoItem>> Get(int id)
    {
        TodoItem? todoItem = await _todos.FindAsync(id);
        return todoItem is null ? NotFound() : todoItem;
    }
}