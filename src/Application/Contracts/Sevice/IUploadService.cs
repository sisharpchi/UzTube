using Application.Dtos.Upload;

namespace Application.Contracts.Sevice;

public interface IUploadService
{
    Task<UploadResult> UploadVideoOrImageAsync(Stream fileStream, string fileName);
    //Task<string> UploadImageAsync(Stream fileStream, string fileName);
    Task DeleteFileAsync(string fileUrl);
}