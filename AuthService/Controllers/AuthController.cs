using AuthService.Dto;
using AuthService.Services.Abstracts;
using Microsoft.AspNetCore.Mvc;

namespace AuthService.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController(IAuthService authService): ControllerBase
{
    [HttpPost("login")]
    public async Task<ActionResult> Login(LoginDto dto)
    {
        return Ok(new { token = await authService.Login(dto)});
    }
    
    [HttpPost("register")]
    public async Task<ActionResult> Register(RegistrationDto dto)
    {
        await authService.Register(dto);
        return Ok();
    }

    [HttpGet("test")]
    public async Task<ActionResult> Test()
    {
        return Ok("Hello");
    }
}