using System.ComponentModel.DataAnnotations;

namespace TodoService.Dto;

public class CreateTodoDto
{
    [Required]
    [StringLength(100, ErrorMessage = "Title must be between 1 and 100 characters.")]
    public string Title { get; set; } = string.Empty;
    public bool IsCompleted { get; set; }
    
}