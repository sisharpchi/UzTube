using Domain.Entities;

namespace Application.Contracts.SeviceContracts;

public interface IAuthService
{
    Task<string> GenerateJwtTokenAsync(User user);
    Task<bool> VerifyPasswordAsync(User user, string password);
    Task<string> HashPasswordAsync(string password);
}