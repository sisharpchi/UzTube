using Application.Contracts.Repository;
using Domain.Entities;
using Infrastructure.Persistence.MainDbContext;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence.Repositories;

public class VideoRepository(AppDbContext appDbContext) : IVideoRepository
{
    public async Task<long> AddAsync(Video video)
    {
        await appDbContext.Videos.AddAsync(video);
        await appDbContext.SaveChangesAsync();
        return video.Id;
    }
    public async Task DeleteAsync(long id)
    {
        var video = await appDbContext.Videos
            .Include(v => v.Comments)
            .Include(v => v.Likes)
            .Include(v => v.Reports)
            .Include(v => v.ViewHistories)
            .FirstOrDefaultAsync(v => v.Id == id);

        if (video == null)
            throw new Exception($"Video with id {id} not found");

        // Bog‘liqlarni o‘chirish
        appDbContext.Comments.RemoveRange(video.Comments);
        appDbContext.LikeDislikes.RemoveRange(video.Likes);
        appDbContext.VideoReports.RemoveRange(video.Reports);
        appDbContext.ViewHistories.RemoveRange(video.ViewHistories);

        appDbContext.Videos.Remove(video);
        await appDbContext.SaveChangesAsync();
    }
    public async Task<IEnumerable<Video>?> GetAllAsync() => await appDbContext.Videos
        .Include(v => v.Channel)
        .Include(v => v.Playlist)
        .Include(v => v.Likes)
        .Include(v => v.ViewHistories)
        .AsNoTracking().ToListAsync();

    public async Task<Video?> GetByIdAsync(long userId, long id) => await appDbContext.Videos
        .Include(v => v.Channel)
        .ThenInclude(ch => ch.Subscribers)
        .Include(v => v.Comments)
        .Include(v => v.Playlist)
        .Include(v => v.Likes)
        .Include(v => v.ViewHistories)
        .AsNoTracking()
        .FirstAsync(v => v.Id == id && v.Channel.OwnerId == userId);

    public async Task<Video?> GetByIdAsync(long id) => await appDbContext.Videos
        .Include(v => v.Channel)
        .ThenInclude(ch => ch.Subscribers)
        .Include(v => v.Comments)
        .Include(v => v.Playlist)
        .Include(v => v.Likes)
        .Include(v => v.ViewHistories)
        .AsNoTracking()
        .FirstAsync(v => v.Id == id);

    public async Task<IEnumerable<Video>> GetByChannelIdAsync(long channelId) => await appDbContext.Videos.Where(v => v.ChannelId == channelId).AsNoTracking().ToListAsync();
    public async Task<IEnumerable<Video>> SearchAsync(string query) => await appDbContext.Videos.Where(v => v.Title.Contains(query)).AsNoTracking().ToListAsync();
    public async Task<IEnumerable<Video>> FilterAsync(string? tag, long? channelId, string? duration, string? sort)
    {
        var videos = appDbContext.Videos.AsQueryable();
        if (channelId.HasValue) videos = videos.Where(v => v.ChannelId == channelId);
        if (!string.IsNullOrEmpty(tag)) videos = videos.Where(v => v.VideoTags.Any(t => t.Tag.Name == tag));
        return await videos.AsNoTracking().ToListAsync();
    }
    public async Task UpdateAsync(Video video)
    {
        appDbContext.Videos.Update(video);
        await appDbContext.SaveChangesAsync();
    }

    public async Task IncrementViewCountAsync(long videoId)
    {
        var video = await appDbContext.Videos.FindAsync(videoId);
        if (video != null)
        {
            video.ViewHistories.Add(new ViewHistory { VideoId = videoId, WatchedAt = DateTime.UtcNow });
        }
    }

    public async Task<Video> GetByNodeAndOwnerId(long userId, string nodeId)
    {
        return await appDbContext.Videos.FirstOrDefaultAsync(v => v.Channel.OwnerId == userId && v.CloudPublicId == nodeId);
    }
}
