using System.ComponentModel.DataAnnotations;

namespace SimpleTodo.Api.Models;

public class TodoItem
{
    public int Id { get; set; }
    
    [Required]
    [StringLength(100, ErrorMessage = "Title must be between 1 and 100 characters.")]
    public string? Title { get; set; }
    public bool IsCompleted { get; set; }
}