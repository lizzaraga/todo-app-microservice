using Microsoft.AspNetCore.Mvc;
using SimpleTodo.Api.Models;

namespace SimpleTodo.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class TodoController : ControllerBase
{
    private static List<TodoItem> todos = new();

    [HttpDelete("{id}")]
    public ActionResult Delete(int id)
    {
        TodoItem? existing = todos.FirstOrDefault(t => t.Id == id);
        if (existing is null) return NotFound();
        todos.Remove(existing);
        return NoContent();
    }
    
    [HttpPut("{id}")]
    public ActionResult Update(int id, TodoItem updatedTodo)
    {
        TodoItem? existing = todos.FirstOrDefault(t => t.Id == id);
        if (existing is null) return NotFound();
        existing.Title = updatedTodo.Title;
        existing.IsCompleted = updatedTodo.IsCompleted;
        return NoContent();
    }

    [HttpPost]
    public ActionResult<TodoItem> Create(TodoItem todo)
    {
        todo.Id = todos.Count + 1;
        todos.Add(todo);
        return CreatedAtAction(nameof(Create), new { id = todo.Id }, todo);
    }

    [HttpGet]
    public ActionResult<IEnumerable<TodoItem>> GetAll()
    {
        return todos;
    }

    [HttpGet("{id}")]
    public ActionResult<TodoItem> Get(int id)
    {
        TodoItem? todoItem = todos.FirstOrDefault(todo => todo.Id == id);
        return todoItem is null ? NotFound() : todoItem;
    }
}