using Application.Dtos.Playlist;
using Application.Dtos.Video;

namespace Application.Contracts.Sevice;

public interface IPlaylistService
{
    Task<PlaylistDto> CreateAsync(PlaylistCreateDto dto);
    Task<PlaylistDto> UpdateAsync(long playlistId, PlaylistUpdateDto dto);
    Task<bool> DeleteAsync(long playlistId);

    Task<bool> AddVideoAsync(long playlistId, long videoId);
    Task<bool> RemoveVideoAsync(long playlistId, long videoId);

    Task<List<PlaylistDto>> GetByChannelAsync(long channelId);
    Task<List<VideoListItemDto>> GetVideosAsync(long playlistId);
}