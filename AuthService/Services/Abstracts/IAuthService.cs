using AuthService.Dto;

namespace AuthService.Services.Abstracts;

public interface IAuthService
{
    Task<string> Login(LoginDto loginDto);
    Task Register(RegistrationDto registrationDto);
}