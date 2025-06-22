using Domain.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence.Configurations;

public class SavedVideoConfiguration : IEntityTypeConfiguration<SavedVideo>
{
    public void Configure(EntityTypeBuilder<SavedVideo> builder)
    {
        builder.ToTable("SavedVideos");

        builder.HasKey(x => x.Id);

        builder.HasOne(x => x.User)
            .WithMany(u => u.SavedVideos)
            .HasForeignKey(x => x.UserId)
            .OnDelete(DeleteBehavior.Cascade); // foydalanuvchi o‘chirilsa, saved videos ham o‘chadi

        builder.HasOne(x => x.Video)
            .WithMany() // agar Video entity'da ICollection<SavedVideo> bo‘lsa, bu yerga .WithMany(v => v.SavedVideos) yozing
            .HasForeignKey(x => x.VideoId)
            .OnDelete(DeleteBehavior.Cascade); // video o‘chirilsa, saved entry ham o‘chadi
    }
}