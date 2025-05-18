using System.Net;
using AuthService.Config;
using AuthService.Config.Exceptions;
using AuthService.Data;
using AuthService.Dto;
using AuthService.Helpers;
using AuthService.Models;
using AuthService.Services.Abstracts;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace AuthService.Services;

public class AuthService(AuthDbContext dbContext, IOptions<JwtConfig> jwtConfig) : IAuthService
{
    private readonly DbSet<User> _users = dbContext.Users;

    public async Task<string> Login(LoginDto loginDto)
    {
        var existingUser = await _users.FirstOrDefaultAsync(u => u.Username == loginDto.Username);
        if (existingUser is null || !BCrypt.Net.BCrypt.Verify(loginDto.Password, existingUser.PasswordHash))
            throw new BusinessException("BAD_CREDENTIALS", HttpStatusCode.Unauthorized);
        return TokenHelper.CreateToken(existingUser, jwtConfig.Value);
    }

    public async Task Register(RegistrationDto registrationDto)
    {
        var existingUser = await _users.FirstOrDefaultAsync(u => u.Username == registrationDto.Username);
        if (existingUser is not null) throw new BusinessException("USERNAME_ALREADY_EXISTS", HttpStatusCode.Conflict);
        var user = new User
        {
            Username = registrationDto.Username,
            PasswordHash = BCrypt.Net.BCrypt.HashPassword(registrationDto.Password)
        };
        await _users.AddAsync(user);
        await dbContext.SaveChangesAsync();
    }
}