namespace Application.Dtos.User;

public class UserLoginResponseDto
{
    public string AccessToken { get; set; }
    public DateTime AccessTokenExpiresAt { get; set; }

    public string RefreshToken { get; set; }
    public DateTime RefreshTokenExpiresAt { get; set; }

    public long Id { get; set; }
    public string FullName { get; set; }
    public string Email { get; set; }
    public string? ProfileImageUrl { get; set; }

    public long RoleId { get; set; }
    public string RoleName { get; set; }
}
