namespace Application.Dtos.User;

public class UserDto
{
    public long Id { get; set; }
    public string FullName { get; set; }
    public string Email { get; set; }
    public string? ProfileImageUrl { get; set; }
    public DateTime CreatedAt { get; set; }

    public long RoleId { get; set; }
    public string RoleName { get; set; }
}
