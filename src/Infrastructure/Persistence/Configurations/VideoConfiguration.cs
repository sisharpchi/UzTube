using Domain.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence.Configurations;

public class VideoConfiguration : IEntityTypeConfiguration<Video>
{
    public void Configure(EntityTypeBuilder<Video> builder)
    {
        // Jadval nomi
        builder.ToTable("Videos");

        // Primary Key
        builder.HasKey(v => v.Id);

        // Required fields
        builder.Property(v => v.Title).IsRequired();
        builder.Property(v => v.VideoUrl).IsRequired();
        builder.Property(v => v.Duration).IsRequired();
        builder.Property(v => v.UploadedAt).IsRequired();

        // Video ➝ Channel (N:1)
        builder.HasOne(v => v.Channel)
            .WithMany(c => c.Videos)
            .HasForeignKey(v => v.ChannelId)
            .OnDelete(DeleteBehavior.Restrict); // Channel o‘chsa, videolar ham o‘chadi

        // Video ➝ Playlist (N:1, optional)
        builder.HasOne(v => v.Playlist)
            .WithMany(p => p.Videos)
            .HasForeignKey(v => v.PlaylistId)
            .OnDelete(DeleteBehavior.Restrict); //  SetNull edi Playlist o‘chsa, video PlaylistId null bo‘ladi

        // Video ➝ Comments
        builder.HasMany(v => v.Comments)
            .WithOne(c => c.Video)
            .HasForeignKey(c => c.VideoId)
            .OnDelete(DeleteBehavior.Restrict);

        // Video ➝ LikeDislikes
        builder.HasMany(v => v.Likes)
            .WithOne(ld => ld.Video)
            .HasForeignKey(ld => ld.VideoId)
            .OnDelete(DeleteBehavior.Restrict); // ❗ Cycle errorni oldini olish uchun

        // Video ➝ ViewHistories
        builder.HasMany(v => v.ViewHistories)
            .WithOne(vh => vh.Video)
            .HasForeignKey(vh => vh.VideoId)
            .OnDelete(DeleteBehavior.Restrict);

        // Video ➝ VideoTags
        builder.HasMany(v => v.VideoTags)
            .WithOne(vt => vt.Video)
            .HasForeignKey(vt => vt.VideoId)
            .OnDelete(DeleteBehavior.Restrict);

        // Video ➝ Reports
        builder.HasMany(v => v.Reports)
            .WithOne(r => r.Video)
            .HasForeignKey(r => r.VideoId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}