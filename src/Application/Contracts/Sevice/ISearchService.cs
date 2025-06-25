using Domain.Entities;

namespace Application.Contracts.Sevice;


public interface ISearchService
{
    Task<IEnumerable<Video>> SearchVideosAsync(string query);
    Task<IEnumerable<Channel>> SearchChannelsAsync(string query);
    Task<IEnumerable<Tag>> SuggestTagsAsync(string partial);
}

