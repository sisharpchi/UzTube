namespace Application.Dtos.User;

public class UserChangePasswordDto
{
    public string OldPassword { get; set; }
    public string NewPassword { get; set; }
}