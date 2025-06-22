using Domain.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence.Configurations;

public class VideoReportConfiguration : IEntityTypeConfiguration<VideoReport>
{
    public void Configure(EntityTypeBuilder<VideoReport> builder)
    {
        // Jadval nomi
        builder.ToTable("VideoReports");

        // Primary Key
        builder.HasKey(vr => vr.Id);

        // Required fields
        builder.Property(vr => vr.Reason)
            .IsRequired();

        builder.Property(vr => vr.CreatedAt)
            .IsRequired();

        //// VideoReport ➝ Video (N:1)
        //builder.HasOne(vr => vr.Video)
        //    .WithMany(v => v.Reports)
        //    .HasForeignKey(vr => vr.VideoId)
        //    .OnDelete(DeleteBehavior.Cascade); // Video o‘chsa, reportlar ham o‘chadi

        // VideoReport ➝ User (Reporter) (N:1)
        builder.HasOne(vr => vr.Reporter)
            .WithMany()
            .HasForeignKey(vr => vr.ReporterId)
            .OnDelete(DeleteBehavior.Cascade); // User o‘chsa, reportlar ham o‘chadi
    }
}