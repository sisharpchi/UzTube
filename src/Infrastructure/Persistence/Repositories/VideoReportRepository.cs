using Application.Contracts.Repository;
using Domain.Entities;
using Infrastructure.Persistence.MainDbContext;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence.Repositories;

public class VideoReportRepository(AppDbContext appDbContext) : IVideoReportRepository
{
    public async Task<long> AddAsync(VideoReport report)
    {
        await appDbContext.VideoReports.AddAsync(report);
        await appDbContext.SaveChangesAsync();
        return report.Id;
    }

    public async Task<int> CountReportsByVideoIdAsync(long videoId)
    {
        return await appDbContext.VideoReports
            .AsNoTracking()
            .CountAsync(r => r.VideoId == videoId);
    }

    public async Task<IEnumerable<VideoReport>> GetByVideoIdAsync(long videoId)
    {
        return await appDbContext.VideoReports
            .AsNoTracking()
            .Where(r => r.VideoId == videoId)
            .ToListAsync();
    }
}
