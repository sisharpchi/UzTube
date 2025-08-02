using Application.Contracts.Repository;
using Application.Contracts.Sevice;
using Application.Dtos.User;
using CloudinaryDotNet;
using CloudinaryDotNet.Actions;

namespace Application.Services;

public class UserService(IUserRepository userRepository, Cloudinary cloudinary) : IUserService
{
    public Task<bool> DeleteAsync(long userId)
    {
        throw new NotImplementedException();
    }

    public Task<List<UserDto>> GetAllAsync()
    {
        throw new NotImplementedException();
    }

    public Task<UserDto> GetByEmailAsync(string email)
    {
        throw new NotImplementedException();
    }

    public Task<UserDto> GetByIdAsync(long id)
    {
        throw new NotImplementedException();
    }

    public Task<UserWithChannelDto> GetUserWithChannelAsync(long userId)
    {
        throw new NotImplementedException();
    }

    public Task<UserDto> UpdateProfileAsync(long userId, UserUpdateDto dto)
    {
        throw new NotImplementedException();
    }

    public async Task UploadProfileImg(long userId, Stream avatar, string fileName)
    {
        var user = await userRepository.GetByIdAsync(userId);
        if (user == null)
        {
            throw new InvalidOperationException("User not found.");
        }
        if (!string.IsNullOrEmpty(user.ProfileImageUrl) || !string.IsNullOrEmpty(user.ProfileCloudPublicId))
        {
            // 2. Cloudinarydan video va thumbnailni o‘chiramiz
            var deleteImageParams = new DeletionParams(user.ProfileCloudPublicId)
            {
                ResourceType = ResourceType.Image
            };

            var deleteImageResult = await cloudinary.DestroyAsync(deleteImageParams);

            if (deleteImageResult.Result != "ok" && deleteImageResult.Result != "not_found")
                throw new Exception($"Imageni o‘chirishda xatolik: {deleteImageResult.Result}");

            user.ProfileImageUrl = null;
            user.ProfileCloudPublicId = null;
        }

        var imageUploadParams = new ImageUploadParams
        {
            File = new FileDescription(fileName, avatar),
            Folder = "ChannelAvatars",
        };

        var imageUploadResult = await cloudinary.UploadAsync(imageUploadParams);

        if (imageUploadResult.Error != null)
            throw new Exception($"Image upload failed: {imageUploadResult.Error.Message}");


        user.ProfileImageUrl = imageUploadResult.SecureUrl.ToString();
        user.ProfileCloudPublicId = imageUploadResult.PublicId;
        await userRepository.UpdateAsync(user);
    }
}
