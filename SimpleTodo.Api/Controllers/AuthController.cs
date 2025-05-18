using System.Text;
using System.Text.Unicode;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.IdentityModel.Tokens;
using SimpleTodo.Api.Configs;
using SimpleTodo.Api.Data;
using SimpleTodo.Api.Dto;
using SimpleTodo.Api.Models;

namespace SimpleTodo.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController(TodoDbContext dbContext, IOptions<JwtConfig> jwtOption) : ControllerBase
{
    private readonly DbSet<User> _users = dbContext.Users;

    [HttpPost("register")]
    public async Task<ActionResult> Register(UserDto request)
    {
        var existingUser = _users.FirstOrDefault(u => u.Username == request.Username);
        if (existingUser is not null) return BadRequest("Username already exists");
        _users.Add(new User()
        {
            Username = request.Username,
            PasswordHash = BCrypt.Net.BCrypt.HashPassword(request.Password)
        });
        await dbContext.SaveChangesAsync();
        return Ok();
    }

    [HttpPost("login")]
    public async Task<ActionResult> Login(UserDto dto)
    {
        var existingUser = await _users.FirstOrDefaultAsync(u => u.Username == dto.Username);
        if (existingUser is null) return Unauthorized();
        if (!BCrypt.Net.BCrypt.Verify(dto.Password, existingUser.PasswordHash)) return Unauthorized();
        return Ok(new { token = CreateToken(existingUser) });
    }

    private string CreateToken(User user)
    {
        string secret = jwtOption.Value.Secret;
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secret));

        var tokenDescriptor = new SecurityTokenDescriptor()
        {
            Audience = "simple-todo",
            Issuer = "simple-todo-api",
            IssuedAt = DateTime.UtcNow,
            Expires = DateTime.Now.AddDays(2),
            SigningCredentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256)
        };

        return new JsonWebTokenHandler().CreateToken(tokenDescriptor);
    }
}