using Domain.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence.Configurations;

public class ViewHistoryConfiguration : IEntityTypeConfiguration<ViewHistory>
{
    public void Configure(EntityTypeBuilder<ViewHistory> builder)
    {
        // Jadval nomi
        builder.ToTable("ViewHistories");

        // Primary Key
        builder.HasKey(vh => vh.Id);

        // Required fields
        builder.Property(vh => vh.WatchedAt)
            .IsRequired();

        builder.Property(vh => vh.SecondsWatched)
            .IsRequired();

        //// ViewHistory ➝ Video (N:1)
        //builder.HasOne(vh => vh.Video)
        //    .WithMany(v => v.ViewHistories)
        //    .HasForeignKey(vh => vh.VideoId)
        //    .OnDelete(DeleteBehavior.Cascade); // Video o‘chsa, viewlar ham o‘chadi

        // ViewHistory ➝ User (N:1)
        //builder.HasOne(vh => vh.User)
        //    .WithMany(u => u.ViewHistories)
        //    .HasForeignKey(vh => vh.UserId)
        //    .OnDelete(DeleteBehavior.Cascade); // User o‘chsa, viewlar ham o‘chadi
    }
}