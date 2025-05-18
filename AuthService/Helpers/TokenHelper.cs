using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using AuthService.Config;
using AuthService.Models;
using Microsoft.IdentityModel.Tokens;

namespace AuthService.Helpers;

public static class TokenHelper
{
    public static String CreateToken(User user, JwtConfig config)
    {
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config.Secret));
        var securityToken = new JwtSecurityToken(
            issuer: config.Issuer,
            audience: config.Audience,
            claims: new List<Claim>(),
            expires: DateTime.Now.AddDays(2),
            signingCredentials: new SigningCredentials(key, SecurityAlgorithms.HmacSha256)
            );
        return new JwtSecurityTokenHandler().WriteToken(securityToken);
    }
}