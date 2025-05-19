using System.Net;
using Microsoft.EntityFrameworkCore;
using TodoService.Config.Exceptions;
using TodoService.Data;
using TodoService.Dto;
using TodoService.Models;
using TodoService.Services.Abstracts;

namespace TodoService.Services;

public class TodoService(TodoDbContext dbContext): ITodoService
{
    private readonly DbSet<TodoItem> _todoItems = dbContext.TodoItems;
    public async Task<TodoItem> Create(string ownerId, CreateTodoDto dto)
    {
        var entry = _todoItems.Add(new TodoItem()
        {
            Title = dto.Title,
            IsCompleted = dto.IsCompleted,
            Owner = Guid.Parse(ownerId)
        });
        await dbContext.SaveChangesAsync();
        return entry.Entity;
    }

    public async Task<TodoItem> Get(string ownerId, int todoId)
    {
        var existingTodo = await _todoItems.FirstOrDefaultAsync(t => t.Owner.Equals(Guid.Parse(ownerId)) && t.Id == todoId);
        if (existingTodo is null) throw new BusinessException("TODO_NOT_FOUND", HttpStatusCode.NotFound);
        return existingTodo;
    }

    public async Task<IEnumerable<TodoItem>> GetAll(string ownerId, int page, int pageSize, string searchTitle)
    {
        var query = _todoItems.AsQueryable().Where(t => t.Owner == Guid.Parse(ownerId));
        if (!string.IsNullOrEmpty(searchTitle))
        {
            query = query.Where(t => t.Title!.Contains(searchTitle));
        }
        query = query.Skip(page - 1).Take(pageSize);
        return await query.AsNoTracking().ToListAsync();
    }

    public async Task Delete(string ownerId, int todoId)
    {
        var existingTodo = await _todoItems.FirstOrDefaultAsync(t => t.Owner.Equals(Guid.Parse(ownerId)) && t.Id == todoId);
        if (existingTodo is null) throw new BusinessException("TODO_NOT_FOUND", HttpStatusCode.NotFound);
        _todoItems.Remove(existingTodo);
        await dbContext.SaveChangesAsync();
    }

    public async Task Update(string ownerId, int todoId, UpdateTodoDto updateTodoDto)
    {
        var existingTodo = await _todoItems.FirstOrDefaultAsync(t => t.Owner.Equals(Guid.Parse(ownerId)) && t.Id == todoId);
        if (existingTodo is null) throw new BusinessException("TODO_NOT_FOUND", HttpStatusCode.NotFound);
        existingTodo.Title = updateTodoDto.Title;
        existingTodo.IsCompleted = updateTodoDto.IsCompleted;
        await dbContext.SaveChangesAsync();

    }
}