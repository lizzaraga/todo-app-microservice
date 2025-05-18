using System.ComponentModel.DataAnnotations.Schema;

namespace SimpleTodo.Api.Models;

public class User
{
    public int Id { get; set; }
    
    public string Username { get; set; } = "";
    public string PasswordHash { get; set; } = "";
}