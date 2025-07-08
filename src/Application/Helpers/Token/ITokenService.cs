using Application.Dtos.User;
using System.Security.Claims;

namespace Application.Helpers.Token;

public interface ITokenService
{
    public string GenerateToken(UserDto user);
    public string GenerateRefreshToken();
    public ClaimsPrincipal? GetPrincipalFromExpiredToken(string token);
}