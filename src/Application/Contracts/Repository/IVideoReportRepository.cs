using Domain.Entities;

namespace Application.Contracts.Repository;


public interface IVideoReportRepository
{
    Task<IEnumerable<VideoReport>> GetByVideoIdAsync(long videoId);
    Task<long> AddAsync(VideoReport report);
    Task<int> CountReportsByVideoIdAsync(long videoId);
}
