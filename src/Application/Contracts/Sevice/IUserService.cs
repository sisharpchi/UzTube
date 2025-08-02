using Application.Dtos.User;

namespace Application.Contracts.Sevice;

public interface IUserService
{
    // 3. Get user by Id
    Task<UserDto> GetByIdAsync(long id);

    // 4. Get user by Email
    Task<UserDto> GetByEmailAsync(string email);

    // 5. Update user's profile
    Task<UserDto> UpdateProfileAsync(long userId, UserUpdateDto dto);

    // 7. Get user with channel info
    Task<UserWithChannelDto> GetUserWithChannelAsync(long userId);

    // 8. Get all users (admin)
    Task<List<UserDto>> GetAllAsync();

    // 9. Delete user
    Task<bool> DeleteAsync(long userId);
    Task UploadProfileImg(long userId, Stream avatar, string fileName);
}