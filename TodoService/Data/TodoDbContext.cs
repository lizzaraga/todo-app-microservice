using Microsoft.EntityFrameworkCore;
using TodoService.Models;

namespace TodoService.Data;

public class TodoDbContext(DbContextOptions<TodoDbContext> options): DbContext(options)
{
    public DbSet<Todo> Todos => Set<Todo>();
}