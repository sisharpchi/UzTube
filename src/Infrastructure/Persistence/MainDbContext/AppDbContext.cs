using Domain.Entities;
using Infrastructure.Persistence.Configurations;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence.MainDbContext;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    // mssql
    public DbSet<Channel> Channels { get; set; }
    public DbSet<Video> Videos { get; set; }
    public DbSet<Comment> Comments { get; set; }
    public DbSet<CommentLike> CommentLikes { get; set; }
    public DbSet<LikeDislike> LikeDislikes { get; set; }
    public DbSet<Subscription> Subscriptions { get; set; }
    public DbSet<ViewHistory> ViewHistories { get; set; }
    public DbSet<Playlist> Playlists { get; set; }
    public DbSet<SavedVideo> SavedVideos { get; set; }
    public DbSet<WatchLater> WatchLaters { get; set; }
    public DbSet<Tag> Tags { get; set; }
    public DbSet<VideoTag> VideoTags { get; set; }
    public DbSet<VideoReport> VideoReports { get; set; }
    public DbSet<Notification> Notifications { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        //modelBuilder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);
        modelBuilder.ApplyConfiguration(new ChannelConfiguration());
        modelBuilder.ApplyConfiguration(new VideoConfiguration());
        modelBuilder.ApplyConfiguration(new CommentConfiguration());
        modelBuilder.ApplyConfiguration(new CommentLikeConfiguration());
        modelBuilder.ApplyConfiguration(new LikeDislikeConfiguration());
        modelBuilder.ApplyConfiguration(new SubscriptionConfiguration());
        modelBuilder.ApplyConfiguration(new LikeDislikeConfiguration());
        modelBuilder.ApplyConfiguration(new ViewHistoryConfiguration());
        modelBuilder.ApplyConfiguration(new PlaylistConfiguration());
        modelBuilder.ApplyConfiguration(new SavedVideoConfiguration());
        modelBuilder.ApplyConfiguration(new WatchLaterConfiguration());
        modelBuilder.ApplyConfiguration(new TagConfiguration());
        modelBuilder.ApplyConfiguration(new VideoTagConfiguration());
        modelBuilder.ApplyConfiguration(new VideoReportConfiguration());
        modelBuilder.ApplyConfiguration(new NotificationConfiguration());

        base.OnModelCreating(modelBuilder);
    }
}