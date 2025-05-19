using TodoService.Dto;
using TodoService.Models;

namespace TodoService.Services.Abstracts;

public interface ITodoService
{
    Task<TodoItem> Create(string ownerId, CreateTodoDto dto);
    Task<TodoItem> Get(string ownerId, int todoId);
    Task<IEnumerable<TodoItem>> GetAll(string ownerId, int page, int pageSize, string searchTitle);
    Task Delete(string ownerId, int todoId);
    Task Update(string ownerId, int todoId, UpdateTodoDto updateTodoDto);
}