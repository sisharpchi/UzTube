namespace Application.Dtos.User;

public class UserLoginResponseDto
{
    public string AccessToken { get; set; }
    public string RefreshToken { get; set; } = null;
    public string TokenType { get; set; }
    public int Expires { get; set; }
}
