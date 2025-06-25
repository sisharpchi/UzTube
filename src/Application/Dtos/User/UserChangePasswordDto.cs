namespace Application.Dtos.User;

public class UserChangePasswordDto
{
    public long UserId { get; set; }
    public string OldPassword { get; set; }
    public string NewPassword { get; set; }
}