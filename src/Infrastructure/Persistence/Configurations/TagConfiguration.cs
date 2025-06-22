using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configurations;

public class TagConfiguration : IEntityTypeConfiguration<Tag>
{
    public void Configure(EntityTypeBuilder<Tag> builder)
    {
        // Jadval nomi
        builder.ToTable("Tags");

        // Primary Key
        builder.HasKey(t => t.Id);

        // Name - required
        builder.Property(t => t.Name)
            .IsRequired();

        // Tag ➝ VideoTags (1:N)
        builder.HasMany(t => t.VideoTags)
            .WithOne(vt => vt.Tag)
            .HasForeignKey(vt => vt.TagId)
            .OnDelete(DeleteBehavior.Cascade); // Tag o‘chsa, VideoTag ham o‘chadi
    }
}