using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configurations;

public class ChannelConfiguration : IEntityTypeConfiguration<Channel>
{
    public void Configure(EntityTypeBuilder<Channel> builder)
    {
        // Jadval nomi
        builder.ToTable("Channels");

        // Primary key
        builder.HasKey(c => c.Id);

        // Name
        builder.Property(c => c.Name)
            .IsRequired();

        // Description (optional)
        builder.Property(c => c.Description)
            .IsRequired(false);

        // Owner: Channel ➝ User (1:1)
        builder.HasOne(c => c.Owner)
            .WithOne(u => u.Channel)
            .HasForeignKey<Channel>(c => c.OwnerId)
            .OnDelete(DeleteBehavior.Cascade);

        // Videos: Channel ➝ Videos (1:N)
        builder.HasMany(c => c.Videos)
            .WithOne(v => v.Channel)
            .HasForeignKey(v => v.ChannelId)
            .OnDelete(DeleteBehavior.Cascade);

        // Subscribers: Channel ➝ Subscriptions (1:N)
        builder.HasMany(c => c.Subscribers)
            .WithOne(s => s.Channel)
            .HasForeignKey(s => s.ChannelId)
            .OnDelete(DeleteBehavior.Restrict); // ❗ Prevent multiple cascade paths
    }
}
