using Domain.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence.Configurations;

public class PlaylistConfiguration : IEntityTypeConfiguration<Playlist>
{
    public void Configure(EntityTypeBuilder<Playlist> builder)
    {
        // Jadval nomi
        builder.ToTable("Playlists");

        // Primary key
        builder.HasKey(p => p.Id);

        // Name - majburiy
        builder.Property(p => p.Name)
            .IsRequired();

        // Description - optional
        builder.Property(p => p.Description)
            .IsRequired(false);

        // Playlist ➝ Channel (N:1)
        builder.HasOne(p => p.Channel)
            .WithMany(c => c.Playlists) // 👈 `Channel` entity’da `ICollection<Playlist> Playlists` bo‘lishi kerak
            .HasForeignKey(p => p.ChannelId)
            .OnDelete(DeleteBehavior.Cascade); // channel o‘chsa, playlistlar ham o‘chadi

        // Playlist ➝ Videos (1:N)
        builder.HasMany(p => p.Videos)
            .WithOne(v => v.Playlist)
            .HasForeignKey(v => v.PlaylistId)
            .OnDelete(DeleteBehavior.SetNull); // ❗ Video playlistdan ajratilsa, `PlaylistId = null` bo‘ladi
    }
}
