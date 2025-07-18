﻿using Application.Dtos.Upload;
using Application.Dtos.Video;

namespace Application.Contracts.Sevice;

public interface IUploadService
{
    Task<UploadResult> UploadVideoOrImageAsync(long userId, VideoUploadDto videoUpload, Stream fileStream, string fileName, Stream thumbnailStream, string thumbnailName);
        //Task<UploadResult> UploadVideoOrImageAsync(Stream fileStream, string fileName);
        //Task<string> UploadImageAsync(Stream fileStream, string fileName);
    Task DeleteFileAsync(long userId, string fileUrl);
}