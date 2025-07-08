using Application.Dtos.User;
using Domain.Entities;

namespace Application.Contracts.SeviceContracts;

public interface IAuthService
{
    Task<long> SignUpUserAsync(UserRegisterDto userRegisterDto);
    Task<UserLoginResponseDto> LoginUserAsync(UserLoginDto userLoginDto);
    Task<UserLoginResponseDto> RefreshTokenAsync(RefreshRequestDto request);
    Task EailCodeSender(string email);
    Task LogOut(string token);
    Task<bool> ConfirmCode(string userCode, string email);
    Task<bool> ChangePasswordAsync(UserChangePasswordDto dto);
}