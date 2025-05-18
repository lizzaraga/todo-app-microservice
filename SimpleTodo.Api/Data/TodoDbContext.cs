using Microsoft.EntityFrameworkCore;
using SimpleTodo.Api.Models;

namespace SimpleTodo.Api.Data;

public class TodoDbContext(DbContextOptions<TodoDbContext> options): DbContext(options)
{
    public DbSet<TodoItem> Todos => Set<TodoItem>();
    public DbSet<User> Users => Set<User>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.Entity<User>()
            .HasIndex(u => u.Username).IsUnique();
    }
}