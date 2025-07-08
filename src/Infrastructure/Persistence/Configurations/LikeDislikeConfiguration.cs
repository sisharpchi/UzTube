using Domain.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence.Configurations;

public class LikeDislikeConfiguration : IEntityTypeConfiguration<LikeDislike>
{
    public void Configure(EntityTypeBuilder<LikeDislike> builder)
    {
        // Jadval nomi
        builder.ToTable("LikeDislikes");

        // Primary Key
        builder.HasKey(ld => ld.Id);

        // IsLike maydoni required
        builder.Property(ld => ld.IsLike)
            .IsRequired();

        // LikeDislike ➝ Video (N:1)
        builder.HasOne(ld => ld.Video)
            .WithMany(v => v.Likes)
            .HasForeignKey(ld => ld.VideoId)
            .OnDelete(DeleteBehavior.NoAction); // 🔥 Eslatma: CASCADE emas, multiple cascade path oldini olish uchun

        // LikeDislike ➝ User (N:1)
        //builder.HasOne(ld => ld.User)
        //    .WithMany(u => u.Likes)
        //    .HasForeignKey(ld => ld.UserId)
        //    .OnDelete(DeleteBehavior.NoAction); // 🔥 Bu ham CASCADE emas

        // ❗ Shu bilan FOREIGN KEY multiple cascade path xatosi bo‘lmaydi
    }
}