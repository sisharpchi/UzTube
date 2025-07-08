using Application.Dtos.User;
using Application.Helpers.Settings;
using Application.Helpers.Time;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace Application.Helpers.Token;

public class TokenService : ITokenService
{
    private readonly string _lifetime;
    private readonly string _issuer;
    private readonly string _audience;
    private readonly string _securityKey;

    public TokenService(JwtAppSettings jwtSetting)
    {
        _lifetime = jwtSetting.Lifetime;
        _issuer = jwtSetting.Issuer;
        _audience = jwtSetting.Audience;
        _securityKey = jwtSetting.SecurityKey;
    }

    public string GenerateToken(UserDto user)
    {
        var IdentityClaims = new Claim[]
        {
            new Claim("UserId", user.Id.ToString()),
            new Claim(JwtRegisteredClaimNames.UniqueName, user.Email),
            new Claim("RoleId", user.RoleId.ToString()),
            new Claim(ClaimTypes.Role, user.RoleName)
        };

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_securityKey!));
        var keyCredentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var expiresHours = int.Parse(_lifetime);
        var token = new JwtSecurityToken(
            issuer: _issuer,
            audience: _audience,
            claims: IdentityClaims,
            expires: TimeHelper.GetDateTime().AddHours(expiresHours),
            signingCredentials: keyCredentials
            );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }

    public string GenerateRefreshToken()
    {
        var randomBytes = new byte[64];
        using var rng = RandomNumberGenerator.Create();
        rng.GetBytes(randomBytes);
        return Convert.ToBase64String(randomBytes);
    }

    public ClaimsPrincipal? GetPrincipalFromExpiredToken(string token)
    {
        var tokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidIssuer = _issuer,
            ValidateAudience = true,
            ValidAudience = _audience,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_securityKey!))
        };

        var tokenHandler = new JwtSecurityTokenHandler();
        return tokenHandler.ValidateToken(token, tokenValidationParameters, out _);
    }
}