using Application.Contracts.Repository;
using Application.Contracts.Sevice;
using Application.Dtos.LikeDislike;
using Domain.Entities;

namespace Application.Services;

public class LikeDislikeService(ILikeDislikeRepository likeDislikeRepository, IChannelRepository channelRepository) : ILikeDislikeService
{
    public async Task<long> ToggleAsync(long userId, LikeDislikeCreateDto dto)
    {
        var existing = await likeDislikeRepository.GetByUserAndVideoIdAsync(userId, dto.VideoId);
        if (existing is null)
        {
            var likeOrDislike = new LikeDislikeCreateDto
            {
                IsLike = dto.IsLike,
                VideoId = dto.VideoId
            };
            var likeDislike = ConvertDtoToLikeDislike(likeOrDislike);
            likeDislike.UserId = userId;
            return await likeDislikeRepository.AddAsync(likeDislike);
        }            
        else
        {
            if (existing.IsLike == dto.IsLike)
            {
                await likeDislikeRepository.RemoveAsync(existing.Id);
            }

            if (existing.IsLike != dto.IsLike)
            {
                existing.IsLike = dto.IsLike;
                await likeDislikeRepository.UpdateAsync(existing);
            }

            return existing.Id;
        }
    }

    private LikeDislike ConvertDtoToLikeDislike(LikeDislikeCreateDto likeDislike)   
    {
        return new LikeDislike()
        {
            IsLike = likeDislike.IsLike,
            VideoId = likeDislike.VideoId,
        };
    }

    public async Task<VideoLikeDislikeStatDto> GetStatAsync(long videoId)
    {
        var countStats = await likeDislikeRepository.CountByVideoIdAsync(videoId);
        var statsDto = new VideoLikeDislikeStatDto()
        {
            VideoId = videoId,
            DislikeCount = countStats.dislikes,
            LikeCount = countStats.likes
        };
        return statsDto;
    }

    public async Task<bool?> GetUserReactionAsync(long videoId, long userId)
    {
        var existing = await likeDislikeRepository.GetByUserAndVideoIdAsync(userId, videoId);
        return existing is null ? null : existing.IsLike;
    }

    public async Task<int> CountAllLikesByChannelId(long userId)
    {
        var channel = await channelRepository.GetByOwnerIdAsync(userId);
        if (channel is null)
        {
            throw new InvalidOperationException("Kanal topilmadi.");
        }
        var likesCount = await likeDislikeRepository.CountAllLikesByChannelId(userId, channel.Id);
        return likesCount;
    }

    public async Task<int> CountAllDislikesByChannelId(long userId)
    {
        var channel = await channelRepository.GetByOwnerIdAsync(userId);
        if (channel is null)
        {
            throw new InvalidOperationException("Kanal topilmadi.");
        }
        var dislikesCount = await likeDislikeRepository.CountAllDislikesByChannelId(userId, channel.Id);
        return dislikesCount;
    }
}
