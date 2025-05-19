using System.ComponentModel.DataAnnotations;

namespace TodoService.Models;

public class TodoItem
{
    public int Id { get; set; }
    
    [Required]
    [StringLength(100, ErrorMessage = "Title must be between 1 and 100 characters.")]
    public string? Title { get; set; }
    public bool IsCompleted { get; set; }

    [Required]
    public Guid Owner { get; set; }
}