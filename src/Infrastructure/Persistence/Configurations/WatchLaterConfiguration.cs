using Domain.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence.Configurations;

public class WatchLaterConfiguration : IEntityTypeConfiguration<WatchLater>
{
    public void Configure(EntityTypeBuilder<WatchLater> builder)
    {
        builder.ToTable("WatchLaters");

        builder.HasKey(w => w.Id);

        builder.HasOne(w => w.User)
            .WithMany(u => u.WatchLaters)
            .HasForeignKey(w => w.UserId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(w => w.Video)
            .WithMany() // Video tarafdan navigation bo'lmasa
            .HasForeignKey(w => w.VideoId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}