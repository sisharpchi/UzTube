using Application.Contracts.Repository;
using Application.Contracts.SeviceContracts;
using Application.Dtos.User;
using Application.Helpers.Security;
using Application.Helpers.Settings;
using Application.Helpers.Token;
using Core.Errors;
using Domain.Entities;
using FluentEmail.Core;
using FluentEmail.Smtp;
using FluentValidation;
using System.Net;
using System.Net.Mail;
using System.Security.Claims;

namespace Application.Services;

public class AuthService(IRoleRepository _roleRepo, IValidator<UserRegisterDto> _validator,
    IUserRepository _userRepo, ITokenService _tokenService,
    JwtAppSettings _jwtSetting, IValidator<UserLoginDto> _validatorForLogin,
    IRefreshTokenRepository _refTokRepo) : IAuthService
{
    private readonly int Expires = int.Parse(_jwtSetting.Lifetime);

    public async Task<long> SignUpUserAsync(UserRegisterDto userCreateDto)
    {
        var validatorResult = await _validator.ValidateAsync(userCreateDto);
        if (!validatorResult.IsValid)
        {
            string errorMessages = string.Join("; ", validatorResult.Errors.Select(e => e.ErrorMessage));
            throw new AuthException(errorMessages);
        }

        User isEmailExists;
        try
        {
            isEmailExists = await _userRepo.GetByEmailAsync(userCreateDto.Email);
        }
        catch (Exception ex)
        {
            isEmailExists = null;
        }

        if (isEmailExists == null)
        {

            var tupleFromHasher = PasswordHasher.Hasher(userCreateDto.Password);

            var confirmer = new UserConfirme()
            {
                IsConfirmed = false,
                Gmail = userCreateDto.Email,
            };


            var user = new User()
            {
                Confirmer = confirmer,
                FullName = userCreateDto.FullName,
                Email = userCreateDto.Email,
                PasswordHash = tupleFromHasher.Hash,
                Salt = tupleFromHasher.Salt,
                RoleId = await _roleRepo.GetRoleIdAsync("User")
            };

            long userId = await _userRepo.AddAsync(user);

            confirmer.UserId = userId;

            var foundUser = await _userRepo.GetByIdAsync(userId);

            foundUser.Confirmer!.UserId = userId;

            await _userRepo.UpdateAsync(foundUser);

            return userId;
        }
        else if (isEmailExists.Confirmer!.IsConfirmed is false)
        {

            var tupleFromHasher = PasswordHasher.Hasher(userCreateDto.Password);

            isEmailExists.FullName = userCreateDto.FullName;
            isEmailExists.Email = userCreateDto.Email;
            isEmailExists.PasswordHash = tupleFromHasher.Hash;
            isEmailExists.Salt = tupleFromHasher.Salt;
            isEmailExists.RoleId = await _roleRepo.GetRoleIdAsync("User");

            await _userRepo.UpdateAsync(isEmailExists);
            return isEmailExists.Id;
        }

        throw new NotAllowedException("This email already confirmed");
    }


    public async Task<UserLoginResponseDto> LoginUserAsync(UserLoginDto userLoginDto)
    {
        var resultOfValidator = _validatorForLogin.Validate(userLoginDto);
        if (!resultOfValidator.IsValid)
        {
            string errorMessages = string.Join("; ", resultOfValidator.Errors.Select(e => e.ErrorMessage));
            throw new AuthException(errorMessages);
        }

        var user = await _userRepo.GetByEmailAsync(userLoginDto.Email);

        var checkUserPassword = PasswordHasher.Verify(userLoginDto.Password, user.PasswordHash, user.Salt);

        if (checkUserPassword == false)
        {
            throw new UnauthorizedException("UserName or password incorrect");
        }
        if (user.Confirmer.IsConfirmed == false)
        {
            throw new UnauthorizedException("Email not confirmed");
        }

        var userGetDto = new UserDto()
        {
            Id = user.Id,
            FullName = user.FullName,
            Email = user.Confirmer.Gmail,
            CreatedAt = user.CreatedAt,
            ProfileImageUrl = user.ProfileImageUrl,
            RoleId = await _roleRepo.GetRoleIdAsync(user.Role.Name),
            RoleName = user.Role.Name,
        };

        var token = _tokenService.GenerateToken(userGetDto);
        var refreshToken = _tokenService.GenerateRefreshToken();

        var refreshTokenToDB = new RefreshToken()
        {
            Token = refreshToken,
            Expires = DateTime.UtcNow.AddDays(21),
            IsRevoked = false,
            UserId = user.Id
        };

        await _refTokRepo.AddRefreshToken(refreshTokenToDB);

        var loginResponseDto = new UserLoginResponseDto()
        {
            AccessToken = token,
            RefreshToken = refreshToken,
            TokenType = "Bearer",
            Expires = 24
        };


        return loginResponseDto;
    }


    public async Task<UserLoginResponseDto> RefreshTokenAsync(RefreshRequestDto request)
    {
        ClaimsPrincipal? principal = _tokenService.GetPrincipalFromExpiredToken(request.AccessToken);
        if (principal == null) throw new ForbiddenException("Invalid access token.");


        var userClaim = principal.FindFirst(c => c.Type == "UserId");
        var userId = long.Parse(userClaim.Value);


        var refreshToken = await _refTokRepo.SelectRefreshToken(request.RefreshToken, userId);
        if (refreshToken == null || refreshToken.Expires < DateTime.UtcNow || refreshToken.IsRevoked)
            throw new UnauthorizedException("Invalid or expired refresh token.");

        refreshToken.IsRevoked = true;

        var user = await _userRepo.GetByIdAsync(userId);

        var userGetDto = new UserDto()
        {
            Id = user.Id,
            Email = user.Confirmer.Gmail,
            FullName = user.FullName,
            CreatedAt = user.CreatedAt,
            ProfileImageUrl = user.ProfileImageUrl,
            RoleId = await _roleRepo.GetRoleIdAsync(user.Role.Name),
            RoleName = user.Role.Name,
        };

        var newAccessToken = _tokenService.GenerateToken(userGetDto);
        var newRefreshToken = _tokenService.GenerateRefreshToken();

        var refreshTokenToDB = new RefreshToken()
        {
            Token = newRefreshToken,
            Expires = DateTime.UtcNow.AddDays(21),
            IsRevoked = false,
            UserId = user.Id
        };

        await _refTokRepo.AddRefreshToken(refreshTokenToDB);

        return new UserLoginResponseDto
        {
            AccessToken = newAccessToken,
            RefreshToken = newRefreshToken,
            TokenType = "Bearer",
            Expires = 24
        };
    }

    public async Task LogOut(string token) => await _refTokRepo.DeleteRefreshToken(token);

    public async Task EailCodeSender(string email)
    {
        var user = await _userRepo.GetByEmailAsync(email);

        var sender = new SmtpSender(() => new SmtpClient("smtp.gmail.com")
        {
            EnableSsl = true,
            DeliveryMethod = SmtpDeliveryMethod.Network,
            Port = 587,
            Credentials = new NetworkCredential("qahmadjon11@gmail.com", "nhksnhhxzdbbnqdw")
        });

        Email.DefaultSender = sender;

        var code = Random.Shared.Next(100000, 999999).ToString();

        var sendResponse = await Email
            .From("qahmadjon11@gmail.com")
            .To(email)
            .Subject("Your Confirming Code")
            .Body(code)
            .SendAsync();

        user.Confirmer!.ConfirmingCode = code;
        user.Confirmer.ExpiredDate = DateTime.UtcNow.AddMinutes(10);
        await _userRepo.UpdateAsync(user);
    }

    public async Task<bool> ConfirmCode(string userCode, string email)
    {
        var user = await _userRepo.GetByEmailAsync(email);
        var code = user.Confirmer!.ConfirmingCode;
        if (code == null || code != userCode || user.Confirmer.ExpiredDate < DateTime.UtcNow || user.Confirmer.IsConfirmed is true)
        {
            throw new NotAllowedException("Code is incorrect");
        }
        user.Confirmer.IsConfirmed = true;
        await _userRepo.UpdateAsync(user);
        return true;
    }

    public Task<bool> ChangePasswordAsync(UserChangePasswordDto dto)
    {
        throw new NotImplementedException();
    }

    public async Task<UserDto> GetMeAsync(long userId)
    {
        var user = await _userRepo.GetByIdAsync(userId);
        if (user is null)
        {
            throw new EntityNotFoundException("User not found");
        }
        return ConvertToUserDto(user);
    }

    private UserDto ConvertToUserDto(User user)
    {
        return new UserDto
        {
            Id = user.Id,
            FullName = user.FullName,
            Email = user.Email,
            CreatedAt = user.CreatedAt,
            ProfileImageUrl = user.ProfileImageUrl,
            RoleId = user.RoleId,
            RoleName = user.Role.Name,
        };
    }
}